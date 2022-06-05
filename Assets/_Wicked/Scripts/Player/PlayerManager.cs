using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked
{
    public class PlayerManager : MonoBehaviour
    {
        public int id;
        public Character character;
        [ReadOnly] public int power;

        [HideInInspector]
        public Deck muckDeck;

        [ReadOnly]
        public List<Card> handCards = new List<Card>();
        private int MAX_HAND_CARDS = 4;

        private Location curLocation = null;
        private Location lastLocation = null;

        private Action lastAction = null;


        public void Init(int _id)
        {
            id = _id;
            power = 0;           

            if (character == null) character = GetComponent<Character>();
            character.Init(this);
            character.UpdatePowerUI(power);
        }

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
            foreach (Card c in handCards)
            {
                if (handCards.Contains(c))
                {
                    handCards.Remove(c);
                }
            }

            //muckDeck.AddCards(cardsToMuck);
        }

        /// <summary>
        /// Select the cards to muck from your hand and muck them
        /// </summary>
        /// <param name="cardsToMuck"></param>
        public void SelectCardsToDiscard()
        {
            /// 1. Deactivate Actions
            /// 2. Activate Cards and UI buttons for discard           
            return;
        }

        public void EndSelectCardsToDiscard()
        {
            /// 1. Find all cards "selected"
            /// 2. Discard them
            /// 3. Activate actions
            return;
        }        

        #endregion

        #region Action - Play Card

        public void SelectCardToPlay()
        {
            /// 1. Deactivate actions
            /// 2. Activate selection for 1 card
            return;
        }

        public void PlayCard(Card card, Location location)
        {
            /// 1. Place the card into that location
            return;
        }

        #endregion

        #region Action - Vanquish

        public void StartVanquish()
        {
            /// 1. Deactivate actions
            /// 2. Activate selection for cards
            return;
        }

        public void TryVanquish(Card defender, List<Card> attackers)
        {
            /// 1. Check if combined attackers power is greater or equal to defender
            return;
        }

        public void Vanquish(Card defender, List<Card> attackers)
        {
            /// 1. Execute vanquish
            return;
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
