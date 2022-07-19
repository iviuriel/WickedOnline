using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wicked
{
    public enum ActionType
    {
        GainPower,
        DiscardCards,
        Vanquish,
        PlayCard,
        Fate,
        MoveItemOrAlly,
        MoveHero
    }

    public class Action: MonoBehaviour
    {
        public Location location;

        [OnValueChanged("UpdateAction")]
        public ActionType actionType;

        [OnValueChanged("UpdateAction")]
        [MinValue(1)]
        [MaxValue(3)]
        [ShowIf("actionType", ActionType.GainPower)]
        public int powerGained;

        private CircleCollider2D circleCollider;
        private GameObject frameSelector;

        private bool actionUsed = false;

        #region Odin Inspector

        private void UpdateAction()
        {
            /// Sprite 
            SpriteRenderer spriteRend = GetComponent<SpriteRenderer>();
            
            string charName = WickedUtils.GetCharacterString(GetComponentInParent<Character>().character);
            string actionName = WickedUtils.GetActionString(actionType);
            

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Sprites/Actions/action_{0}_{1}", charName, actionName);

            if(actionType == ActionType.GainPower) sb.AppendFormat("_{0}", powerGained);

            Sprite sprite = Resources.Load<Sprite>(sb.ToString());
            spriteRend.sprite = sprite;

            /// Name
            gameObject.name = actionName;
            if(actionType == ActionType.GainPower) gameObject.name += "_"+powerGained;

        }

        #endregion

        #region Logic
        public void Init(Location _location)
        {
            location = _location;

            circleCollider = GetComponent<CircleCollider2D>();
            frameSelector = transform.GetChild(0).gameObject;
            frameSelector.SetActive(false);

            actionUsed = false;

            Deactivate();
        }

        public void TryActivate()
        {
            if(!HasBeenUsed())
            {
                Activate();
            }
        }      

        private void Activate()
        {
            circleCollider.enabled = true;
        }

        public void Deactivate()
        {
            circleCollider.enabled = false;
        }  

        public void ResetAction()
        {
            actionUsed = false;
        }

        public bool HasBeenUsed()
        {
            return actionUsed;
        }

        public void SetToUsed()
        {
            actionUsed = true;
        }

        private void PerformAction()
        {
            PlayerManager player = location?.domain?.character?.player;

            if(player == null) return;

            player.SetLastAction(this);

            switch(actionType)
            {
                case ActionType.GainPower:
                    player.GainPower(powerGained);
                    break;
                case ActionType.DiscardCards:
                    player.SelectCardsToDiscard();
                    break;
                case ActionType.PlayCard:
                    player.SelectCardToPlay();
                    break;
                case ActionType.Vanquish:
                    player.StartVanquish();
                    break;
                case ActionType.MoveItemOrAlly:
                    player.SelectCardToMove(CardType.Normal);
                    break;
                case ActionType.MoveHero:
                    player.SelectCardToMove(CardType.Fate);
                    break;
                case ActionType.Fate:
                    player.PerformFate();
                    break;
                default:
                    Debug.LogError("Error performing action.");
                    break;
            }
        }
        #endregion

        #region Mouse Actions
        private void OnMouseUpAsButton()
        {
            PerformAction();
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
