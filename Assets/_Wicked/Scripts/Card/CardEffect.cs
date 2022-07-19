using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Wicked
{
    public abstract class CardEffect : MonoBehaviour
    {
        [HideInInspector]
        public Card card;

        [TextArea]
        public string description;

        public virtual void Init(Card _card)
        {
            card = _card;
        }

        public virtual void Activate() { }

        public virtual void AddListeners() { }

        public virtual void RemoveListeners() { }

    }
}
