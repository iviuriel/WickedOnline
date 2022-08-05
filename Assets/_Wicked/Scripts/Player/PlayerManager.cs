using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wicked
{
    public class PlayerManager : MonoBehaviour
    {
        public int id;
        public Character character;
        [ReadOnly] public int power;
        public Transform handTransform;

        [ReadOnly]
        public List<Card> handCards = new List<Card>();
        private int MAX_HAND_CARDS = 4;

        private Location curLocation = null;
        private Location lastLocation = null;

        private Action lastAction = null;


        ///Vanquish
        [HideInInspector] public List<Card> vanquishAttackers = new();
        [HideInInspector] public Card vanquishDefender;
        [HideInInspector] public int strengthAttackers = 0;
        [HideInInspector] public int strengthDefender = 0;


        public void Init(int _id)
        {
            id = _id;
            power = 0;           

            if (character == null) character = GetComponent<Character>();
            character.Init(this);
            character.UpdatePowerUI(power);

            handCards = character.DrawCards(MAX_HAND_CARDS);
            UpdateHand();
        }

        #region Hand Actions
        private void UpdateHand()
        {
            for(int i = 0; i < handCards.Count; i++)
            {
                Card card = handCards[i];
                card.transform.SetParent(handTransform);
                card.transform.localPosition = new Vector3(1f * i, 0f, -0.1f * i);
                card.ShowCard();

                int layer = LayerMask.NameToLayer("Hand");
                WickedUtils.SetLayerToGameObject(card.gameObject, layer);
            }
        }

        #endregion

        #region Turn Cycle

        /// <summary>
        /// Starts the player's turn
        /// </summary>
        public void StartTurn()
        {
            /*if (character.objective.checkObjectiveWhenTurnBegins)
            {
                if (character.objective.IsCompleted())
                {
                    ObjectiveCompleted();
                }
            }*/
            character.domain.ActivateLocations();

        }

        /// <summary>
        /// Ends the player's turn
        /// </summary>
        public void EndTurn()
        {
            /// Draw cards left
            int cardsLeft = MAX_HAND_CARDS - handCards.Count;
            handCards.AddRange(character.DrawCards(cardsLeft));
            UpdateHand();

            curLocation.DeactivateActions();
            character.domain.DeactivateLocations();
        }

        /// <summary>
        /// Tell the game manager that this player has completed the 
        /// objective and won the game
        /// </summary>
        public void ObjectiveCompleted()
        {
            return;
        }

        public void SetLastAction(Action action)
        {
            lastAction = action;
        }

        public Action GetLastAction()
        {
            return lastAction;
        }

        public bool CanPlayCard(Card card)
        {
            /// Fate cards no need cost
            if (card.cardType == CardType.Fate) return true;

            return card.powerCost <= power;
        }

        #endregion

        #region Locations

        public void SelectLocation(Location location)
        {
            lastLocation = curLocation;
            curLocation = location;
            curLocation.state = LocationState.Current;
            if(lastLocation != null)
                lastLocation.state = LocationState.Unlocked;
            character.MoveAvatar(curLocation);

            character.domain.DeactivateLocations();

            curLocation.ActivateActions();
        }

        public void ActivateCardLocationSelector(CardType type, Location adyacentTo = null)
        {
            character.domain.ActivateCardLocations(type, adyacentTo);
        }
        public void DectivateCardLocationSelector()
        {
            character.domain.DeactivateCardLocations();
        }


        #endregion

        #region Action - Gain Power

        /// <summary>
        /// Check amount in bank to gain the amount given
        /// </summary>
        /// <param name="amount"></param>
        public void GainPower(int amount)
        {
            GameManager.Instance.GainPower(this, amount);
            Debug.Log("Player with ID ["+id+"] has this amount of power: "+power);

            lastAction.SetToUsed();
            lastAction.Deactivate();

            character.UpdatePowerUI(power);
        }

        #endregion

        #region Action - Discard Cards

        /// <summary>
        /// Discard a selection of cards and send them to muck deck
        /// </summary>
        /// <param name="cardsToMuck"></param>
        private void DiscardCards(List<Card> cardsToMuck)
        {   
            foreach (Card c in cardsToMuck)
            {
                if (handCards.Contains(c))
                {
                    handCards.Remove(c);
                }
            }

            character.normalDiscardDeck.AddCards(cardsToMuck);
        }

        /// <summary>
        /// Select the cards to muck from your hand and muck them
        /// </summary>
        /// <param name="cardsToMuck"></param>
        public void SelectCardsToDiscard()
        {
            /// Deactivate other actions
            curLocation.DeactivateActions();
            /// Enable cards for select
            BoardManager.Instance.EnableCardsForSelectionInPlayerHand(this);
            /// Enable UI buttons
            character.ShowDiscardUI();         
        }

        public void EndSelectCardsToDiscard(bool completed = true)
        {
            /// Deactivate other actions
            curLocation.ActivateActions();
            /// Enable cards for select
            BoardManager.Instance.DisableCardsForSelectionInPlayerHand(this);
            /// Enable UI buttons
            character.HideDiscardUI();

            if (completed)
            {
                List<Card> cardsToMuck = handCards.FindAll(x => x.selected);
                if (cardsToMuck.Count == 0) return;
                DiscardCards(cardsToMuck);
                UpdateHand();
            }
            else
            {
                handCards.ForEach(x => x.selected = false);
            }            
        }        

        #endregion

        #region Action - Play Card

        public void SelectCardToPlay()
        {
            /// Deactivate other actions
            curLocation.DeactivateActions();

            /// Enable cards for drag and drop
            BoardManager.Instance.EnableCardsForDragInPlayerHand(this);
        }

        public void PlayCardAtLocation(Card card, Location location)
        {
            /// Retire card selection from Hand
            BoardManager.Instance.DisableCardsForDragInPlayerHand(this);

            /// Substract power from player
            if (card.cardType == CardType.Normal) 
            {
                power -= card.powerCost;
                character.UpdatePowerUI(power);
            }

            /// Add card to location
            handCards.Remove(card);
            location.PlayCard(card);

            /// Deactivate action and activate actions left
            lastAction.SetToUsed();
            lastAction.Deactivate();
            curLocation.ActivateActions();
        }
        
        public void PlayCardAtLocation(Card card, Location location, Card attachedCard)
        {
            /// 1. Place the card into that location
            return;
        }

        #endregion

        #region Action - Vanquish

        public void StartVanquish()
        {
            /// Deactivate other actions
            curLocation.DeactivateActions();
            /// Enable cards for select
            BoardManager.Instance.EnableCardOfTypesAllLocations(this, BoardManager.Instance.vanquishGroupType);
            /// Enable UI buttons
            character.ShowVanquishUI();  
            /// Reset cards
            vanquishDefender = null;
            vanquishAttackers.Clear();
            strengthAttackers = 0;
            strengthDefender = 0;
        }

        public void TryVanquish()
        {
            if(vanquishDefender == null) character.AlertUI(WickedUtils.AlertStrings.HeroMissing);
            if(vanquishAttackers.Count == 0) character.AlertUI(WickedUtils.AlertStrings.AttackersMissing);

            Location vanquishLocation = vanquishDefender.location;
            
            foreach(Card card in vanquishAttackers)
            {
                if(!card.CanVanquishAtLocation(vanquishLocation))
                {
                    character.AlertUI(WickedUtils.AlertStrings.WrongLocationVanquish);
                    return;
                }
            }

            if(strengthAttackers < strengthDefender) 
            {
                character.AlertUI(WickedUtils.AlertStrings.NotEnoughStrength);
            }
            else
            {
                Vanquish();
            }
        }

        public void Vanquish()
        {
            if(vanquishDefender.strength != 0)
            {
                /// We have to discard also all items associated to cards
                List<Card> additionalCards = new List<Card>();
                foreach(Card card in vanquishAttackers)
                {
                    additionalCards.AddRange(card.itemsAttached);
                }

                vanquishAttackers.AddRange(additionalCards);
                character.normalDiscardDeck.AddCards(vanquishAttackers);
            }

            character.fateDiscardDeck.AddCards(vanquishDefender.itemsAttached);
            character.fateDiscardDeck.AddCard(vanquishDefender);

            /// Activate other actions
            lastAction.SetToUsed();
            lastAction.Deactivate();
            curLocation.ActivateActions();
            /// Disable cards for select
            BoardManager.Instance.DisableAllCardAllLocations(this);
            /// Enable UI buttons
            character.HideVanquishUI();  
            /// Reset cards
            vanquishDefender = null;
            vanquishAttackers.Clear();
        }

        public void AddCardToVanquish(Card card)
        {
            if(card.cardType == CardType.Normal)
            {
                vanquishAttackers.Add(card);
                strengthAttackers += card.strength;
            }
            else if(card.cardType == CardType.Fate)
            {
                if(vanquishDefender != null)
                {
                    character.AlertUI(WickedUtils.AlertStrings.CannotAddAnotherHero);
                }
                else
                {
                    vanquishDefender = card;
                    strengthDefender = vanquishDefender.strength;
                }
            }

            character.UpdateVanquishUI(strengthDefender, strengthAttackers);
        }

        public void RemoveCardToVanquish(Card card)
        {
            if(card.cardType == CardType.Normal)
            {
                vanquishAttackers.Remove(card);
                strengthAttackers -= card.strength;
            }
            else if(card.cardType == CardType.Fate)
            {
                if(vanquishDefender != null && vanquishDefender == card)
                {
                    vanquishDefender = null;
                    strengthDefender = 0;
                }
            }

            character.UpdateVanquishUI(strengthDefender, strengthAttackers);
        }

        #endregion

        #region Action - Move Card

        public void SelectCardToMove(CardType cardType)
        {
            /// 1. Deactivate actions
            /// 2. activate cards to select
            /// 3. activate locations to select
            return;
        }

        public void PerformFate()
        {
            /// 1. Deactivate actions
            /// 2. choose player
        }
        #endregion

        #region UI Buttons
        

        public void CancelSelectCards()
        {
            /// 1. Deactivate selection cards and get back to actions
            return;
        }
        #endregion


    }
}
