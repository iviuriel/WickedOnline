using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Wicked
{
    public class Deck : MonoBehaviour
    {
        public bool isDiscardDeck;

        [HideIf("isDiscardDeck")]
        public CardDeckSO deckSO;

        [ReadOnly] public List<Card> cardPile = new List<Card>();

        [Title("Additional")]
        public Transform cardParent;


        [HideInInspector]
        public Character character;
        
        public void Init(Character _character)
        {
            character = _character;

            if (!isDiscardDeck)
            {
                if (deckSO == null)
                {
                    Debug.LogError("No Card Deck in character: " + character);
                    return;
                }

                List<CardOption> cardOption = deckSO.GetListOfCards();
                InstantiateCards(cardOption);
                InitializeCards();
                Shuffle();
            }
        }

        public void InstantiateCards(List<CardOption> cards)
        {
            foreach(CardOption cardOption in cards)
            {
                for(int i = 0; i < cardOption.quantity; i++)
                {
                    GameObject go = Instantiate(cardOption.cardPrefab, cardParent, false);
                    Card card = go.GetComponent<Card>();

                    card.transform.localPosition = new Vector3(0f, 0.0f, 0.01f * cardPile.Count);
                    card.HideCard();

                    cardPile.Add(card);
                }
            }
        }

        private void InitializeCards()
        {
            foreach(Card card in cardPile)
            {
                card.Init(character);
            }
        }

        public void Shuffle()
        {
            List<Card> tmp = new List<Card>();

            int max = cardPile.Count;
            while (max > 0)
            {
                int offset = UnityEngine.Random.Range(0, max);
                tmp.Add(cardPile[offset]);
                cardPile.RemoveAt(offset);
                max -= 1;
            }
            cardPile = tmp;

            /// Re-order in hierarchy
            for(int i = 0; i < cardPile.Count; i++)
            {
                cardPile[i].transform.SetSiblingIndex(i);
            }
        }

        private Card DrawCard()
        {
            Card c = cardPile[0];
            cardPile.RemoveAt(0);
            return c;
        }

        public List<Card> DrawCards(int amount)
        {
            List<Card> c = new();
            for(int i = 0; i < amount; i++)
            {
                c.Add(DrawCard());
            }
            return c;
        }

    }
}
