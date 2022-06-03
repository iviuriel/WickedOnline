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
        public int power;

        [HideInInspector]
        public Deck muckDeck;

        [ReadOnly]
        public List<Card> handCards = new List<Card>();

        private Location curLocation = null;
        private Location lastLocation = null;


        public void Init(int _id)
        {
            id = _id;
            power = 0;
            if (character == null) character = GetComponent<Character>();
            character.Init(this);
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
            Debug.Log("Start turn");
            character.domain.ActivateLocations();

        }

        /// <summary>
        /// Ends the player's turn
        /// </summary>
        public void EndTurn()
        {
            return;
        }

        /// <summary>
        /// Tell the game manager that this player has completed the 
        /// objective and won the game
        /// </summary>
        public void ObjectiveCompleted()
        {
            return;
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

        #region Actions

        /// <summary>
        /// Gain power to play cards
        /// </summary>
        /// <param name="amount"></param>
        public void GainPower(int amount)
        {
            power += amount;
        }

        /// <summary>
        /// Discard a selection of cards and send them to muck deck
        /// </summary>
        /// <param name="cardsToMuck"></param>
        public void DiscardCards(List<Card> cardsToMuck)
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
        #endregion


    }
}
