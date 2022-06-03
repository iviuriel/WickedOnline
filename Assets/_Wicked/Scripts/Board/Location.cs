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

        [Space(10)]
        [HideInInspector]
        public List<Card> normalCardsPlayed = new List<Card>();
        [HideInInspector]
        public List<Card> fateCardsPlayed = new List<Card>();

        [BoxGroup("Colors")] public Color ableToSelectColor;
        [BoxGroup("Colors")] public Color unlockedColor;

        private BoxCollider2D collider;

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

            collider = GetComponent<BoxCollider2D>();
            Deactivate();
        }

        #region Turn Cycle

        public void TryActivate()
        {
            if (state == LocationState.Unlocked)
            {
                Activate();
                Debug.Log("Activated");
            }
        }

        private void Activate()
        {
            collider.enabled = true;
        }

        public void Deactivate()
        {
            collider.enabled = false;
        }

        public void SelectLocation()
        {
            Debug.Log("Selected: "+name);
            domain.character.player.SelectLocation(this);
        }

        public void ActivateActions()
        {
            return;
        }

        #endregion

        #region Mouse Actions
        private void OnMouseUpAsButton()
        {
            SelectLocation();
        }
        #endregion

    }
}
