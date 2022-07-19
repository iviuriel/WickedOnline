using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked
{
    public class SelectionCard : MonoBehaviour
    {
        [Title("Object references")]
        public GameObject hoverSprite;

        [Title("In-Game Info")]
        [ReadOnly] public bool selected;
        private PlayerManager player;

        public void Select()
        {
            selected = true;
            hoverSprite.SetActive(selected);
        }

        public void Deselect()
        {
            selected = false;
            hoverSprite.SetActive(selected);
        }
    }
}
