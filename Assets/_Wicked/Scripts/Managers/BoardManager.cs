using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked
{
    public class BoardManager : SingletonScene<BoardManager>
    {
        public static BoardManager Instance
        {
            get
            {
                return ((BoardManager)mInstance);
            }
            set
            {
                mInstance = value;
            }
        }

        private int cardsRequired;
        private NormalCardType normalCardTypeRequired;
        private FateCardType fateCardTypeRequired;

        public void EnableCardsForDragInPlayerHand(PlayerManager player)
        {
            List<Card> handCards = player.handCards;

            foreach(Card c in handCards)
            {
                c.EnableForDrag();
            }
        }

        public void DisableCardsForDragInPlayerHand(PlayerManager player)
        {
            List<Card> handCards = player.handCards;

            foreach (Card c in handCards)
            {
                c.DisableForDrag();
            }
        }

        public void EnableCardsForSelectionInPlayerHand(PlayerManager player)
        {
            List<Card> handCards = player.handCards;

            foreach (Card c in handCards)
            {
                c.EnableForSelection();
            }
        }

        public void DisableCardsForSelectionInPlayerHand(PlayerManager player)
        {
            List<Card> handCards = player.handCards;

            foreach (Card c in handCards)
            {
                c.DisableForSelection();
            }
        }


    }
}
