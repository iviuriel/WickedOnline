using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Wicked
{
    [System.Serializable]
    public class CardGroup
    {
        public CardType cardType;
        [ShowIf("cardType", CardType.Normal)] public NormalCardType normalCardType;
        [ShowIf("cardType", CardType.Fate)] public FateCardType fateCardType;

        public bool CheckCard(Card card)
        {
            return card.cardType == cardType && 
                ((cardType == CardType.Normal && card.normalCardType == normalCardType) ||
                (cardType == CardType.Fate && card.fateCardType == fateCardType));
        }
    }

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

        /// CARD GROUPS
        public List<CardGroup> vanquishGroupType = new List<CardGroup>();

        #region Play a Card

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

        #endregion

        #region Discard Cards

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

        #endregion

        #region Vanquish

        public void EnableCardOfTypesAllLocations(PlayerManager player, List<CardGroup> groupType)
        {
            List<Location> locations = player.character.domain.locations;

            foreach(Location loc in locations)
            {
                loc.ActivateCardsByGroupType(groupType);
            }
        }

        public void DisableAllCardAllLocations(PlayerManager player)
        {
            List<Location> locations = player.character.domain.locations;

            foreach(Location loc in locations)
            {
                loc.DeactivateAllCards();
            }
        }

        #endregion


    }
}
