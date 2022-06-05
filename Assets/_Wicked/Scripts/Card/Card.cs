using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked
{
    public enum CardType
    {
        Normal,
        Fate
    }

    public enum NormalCardType
    {
        Ally, 
        Item, 
        Effect,
        Condition,
        Curse
    }

    public enum FateCardType
    {
        Hero,
        Item,
        Effect,
    }

    public abstract class Card : MonoBehaviour
    {
        [Title("Card Info")]
        public CardType cardType;

        [ShowIfGroup("Normal Card Info/cardType", Value = CardType.Normal)]
        [BoxGroup("Normal Card Info")]
        [ReadOnly]
        public NormalCardType normalCardType;

        [ShowIfGroup("Fate Card Info/cardType", Value = CardType.Fate)]
        [BoxGroup("Fate Card Info")]
        [ReadOnly]
        public NormalCardType fateCardType;

        public Sprite cardSprite;


        [HideInInspector]
        public Character character;

        #region Odin Inspector
        #endregion

        public virtual void Init(Character _character)
        {
            character = _character;

            SetCardType();
            SetNormalCardType();
            SetFateCardType();
        }

        public abstract void SetCardType();
        public abstract void SetNormalCardType();
        public abstract void SetFateCardType();
    }
}
