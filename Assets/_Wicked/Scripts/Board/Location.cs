using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wicked
{
    public enum LocationState
    {
        Locked,
        Unlocked,
        Current
    }
    public class Location : MonoBehaviour
    {
        [HideInInspector]
        public Domain domain;

        public LocationState state;

        public string locationName;

        public List<Action> topActions = new List<Action>();
        public List<Action> bottomActions = new List<Action>();

        [Title("Containers")]
        public Transform normalCardTransform;
        public Transform fateCardTransform;
        public SelectionCard invisibleCard;

        [Space(10)]
        [HideInInspector]
        public List<Card> normalCardsPlayed = new List<Card>();
        [HideInInspector]
        public List<Card> fateCardsPlayed = new List<Card>();

        [BoxGroup("Colors")] public Color ableToSelectColor;
        [BoxGroup("Colors")] public Color unlockedColor;

        [FoldoutGroup("In-game parameters")] public float yDisplacement;
        [FoldoutGroup("In-game parameters")] public float zDisplacement;

        private BoxCollider2D boxCollider;
        private BoxCollider fateCollider;
        private BoxCollider normalCollider;
        private GameObject frameSelector;


        #region Inspector Buttons
        [Button(ButtonSizes.Medium)]
        public void UpdateName()
        {
            this.gameObject.name = locationName != null && locationName != "" 
                ? locationName : "Generic Location";
        }
        #endregion

        public void Init(Domain _domain)
        {
            domain = _domain;

            foreach(Action top in topActions)
            {
                top.Init(this);
            }

            foreach (Action bottom in bottomActions)
            {
                bottom.Init(this);
            }

            boxCollider = GetComponent<BoxCollider2D>();
            fateCollider = fateCardTransform.GetComponent<BoxCollider>();
            normalCollider = normalCardTransform.GetComponent<BoxCollider>();

            fateCollider.enabled = false;
            normalCollider.enabled = false;

            frameSelector = transform.GetChild(0).gameObject;

            Deactivate();
            invisibleCard.Deselect();
        }

        #region Turn Cycle

        public void TryActivate()
        {
            if (state == LocationState.Unlocked)
            {
                Activate();
            }
        }

        private void Activate()
        {
            boxCollider.enabled = true;
        }

        public void Deactivate()
        {
            boxCollider.enabled = false;
        }

        public void SelectLocation()
        {
            Debug.Log("Selected: "+name);
            domain.character.player.SelectLocation(this);
        }

        public void ActivateActions(bool resetActions = true)
        {
            foreach(Action top in topActions)
            {
                if(resetActions) top.ResetAction();
                top.TryActivate();
            }

            foreach (Action bottom in bottomActions)
            {
                if(resetActions) bottom.ResetAction();
                bottom.TryActivate();
            }
        }

        public void DeactivateActions()
        {
            foreach(Action top in topActions)
            {
                top.Deactivate();
            }

            foreach (Action bottom in bottomActions)
            {
                bottom.Deactivate();
            }
        }

        public void ActivateCardLocation(CardType type)
        {
            Vector3 invisiblePos = Vector3.zero;
            if (type == CardType.Normal)
            {
                normalCollider.enabled = true;
                invisiblePos = normalCardTransform.position;
            }
            else if (type == CardType.Fate)
            {
                fateCollider.enabled = true;
                invisiblePos = fateCardTransform.position;
            }

            invisibleCard.transform.position = invisiblePos + GetCardNextPositionByType(type);
        }

        public void DeactivateCardLocation()
        {
            normalCollider.enabled = false;
            fateCollider.enabled = false;
            invisibleCard.Deselect();
        }

        private Vector3 GetCardNextPositionByType(CardType type)
        {
            return type == CardType.Normal ?
                new Vector3(0f, -yDisplacement * (normalCardsPlayed.Count), zDisplacement * (normalCardsPlayed.Count)) :
                new Vector3(0f, yDisplacement * (fateCardsPlayed.Count), zDisplacement * (fateCardsPlayed.Count));
        }

        public void PlayCard(Card card)
        {
            if(card.cardType == CardType.Normal)
            {
                card.transform.SetParent(normalCardTransform);
                card.transform.position = normalCardTransform.position + GetCardNextPositionByType(card.cardType);
                normalCardsPlayed.Add(card);
            }
            else if (card.cardType == CardType.Fate)
            {
                card.transform.SetParent(fateCardTransform);
                card.transform.position = fateCardTransform.position + GetCardNextPositionByType(card.cardType);
                fateCardsPlayed.Add(card);
            }

            int layer = LayerMask.NameToLayer("Board");
            WickedUtils.SetLayerToGameObject(card.gameObject, layer);

            /// Message that card was played
        }

        #endregion

        #region Mouse Actions
        private void OnMouseUpAsButton()
        {
            SelectLocation();
        }

        private void OnMouseEnter()
        {
            frameSelector.SetActive(true);
        }

        private void OnMouseExit()
        {
            frameSelector.SetActive(false);
        }
        #endregion

    }
}
