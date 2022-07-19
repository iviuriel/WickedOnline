using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Wicked
{
    [System.Serializable]
    public class CardOption
    {
        public GameObject cardPrefab;
        public int quantity;
    }
    [CreateAssetMenu(menuName ="Wicked Online/Card/Card Deck")]
    public class CardDeckSO : ScriptableObject
    {
        [OnValueChanged("CheckOutOfRange")]
        public CardType type;

        [OnCollectionChanged("UpdateNumberOfCards")]
        [ListDrawerSettings(ShowIndexLabels = false, 
            ListElementLabelName = "@cardPrefab != null ? cardPrefab.name + \" x\"+quantity : \"x0\"")]
        public List<CardOption> cards = new List<CardOption>();

        [ReadOnly]
        [InfoBox("$errorMessage", InfoMessageType.Error, "outOfRange")]
        public int numberOfCards = 0;

        [HideInInspector]
        public bool outOfRange = false;
        private readonly int MAX_NORMAL_CARDS = 30;
        private readonly int MAX_FATE_CARDS = 15;
        private string errorMessage = "";

        [Button]
        private void UpdateNumberOfCards()
        {
            numberOfCards = 0;
            foreach (CardOption c in cards)
            {
                numberOfCards += c.quantity;
            }
            CheckOutOfRange();
        }

        private void CheckOutOfRange()
        {
            int maxCards = type == CardType.Normal ? MAX_NORMAL_CARDS : MAX_FATE_CARDS;
            outOfRange = numberOfCards > maxCards;
            errorMessage = "You added " + numberOfCards 
                + " cards and in this type of deck only " 
                + maxCards + " cards are allowed.";
        }

        public List<CardOption> GetListOfCards() { return cards; }
    }
}
