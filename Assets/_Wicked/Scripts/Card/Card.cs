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

    public class Card : MonoBehaviour
    {
        [Title("Card Info")]
        public CardType cardType;
        public CardEffect effect;
        public string cardName;

        [Title("Object references")]
        public SpriteRenderer frontRenderer;
        public SpriteRenderer backRenderer;
        public GameObject hoverSprite;
        public GameObject selectedSprite;

        [ShowIfGroup("CheckNormalCard")]
        [TitleGroup("CheckNormalCard/Normal Card Info")]        
        public NormalCardType normalCardType;
        [TitleGroup("CheckNormalCard/Normal Card Info")]
        [HideIf("normalCardType", Value = NormalCardType.Condition)]
        public int powerCost;


        [ShowIfGroup("CheckFateCard")]
        [TitleGroup("CheckFateCard/Fate Card Info")]
        public FateCardType fateCardType;
        
        [ShowIfGroup("CheckAllyOrHero")]
        [TitleGroup("CheckAllyOrHero/Ally Card")]        
        public int strength;

        [TitleGroup("CheckAllyOrHero/Ally Card"), ReadOnly]
        public List<Card> itemsAttached;

        [ShowIfGroup("CheckItemType")]
        [TitleGroup("CheckItemType/Item Card")]        
        public bool mustBeAttachedToCard = false;
        [TitleGroup("CheckItemType/Item Card")]
        [ShowIf("mustBeAttachedToCard"), ReadOnly]
        public Card cardAttachedTo;

        [Title("Sprites")]
        [OnValueChanged("UpdateSprite")]
        public Sprite cardSprite;
        [OnValueChanged("UpdateBackSprite")]
        public Sprite backSprite;

        [HideInInspector]
        public Character character;


        [Title("In-Game Info")]
        [ReadOnly] public bool selected;
        [ReadOnly] public bool dragged;
        [ReadOnly] public Vector3 lastPosition;
        [ReadOnly] public BoxCollider2D boxCollider;
        [ReadOnly] public Location location;

        private PlayerManager player;
        private Location tempLocation;

        #region Odin Inspector

        private void UpdateSprite() { frontRenderer.sprite = cardSprite; }
        private void UpdateBackSprite() { backRenderer.sprite = backSprite;}

        [Button(ButtonSizes.Medium)]
        private void UpdateName()
        {
            this.gameObject.name = cardName != null && cardName != ""
                ? cardName : "Generic Card";
        }

        private bool CheckNormalCard() {return cardType == CardType.Normal;}
        private bool CheckFateCard() {return cardType == CardType.Fate;}

        private bool CheckAllyOrHero()
        {
            return (cardType == CardType.Normal && normalCardType == NormalCardType.Ally)
                || (cardType == CardType.Fate && fateCardType == FateCardType.Hero);
        }

        private bool CheckItemType()
        {
            return (cardType == CardType.Normal && normalCardType == NormalCardType.Item)
                || (cardType == CardType.Fate && fateCardType == FateCardType.Item);
        }

        #endregion}

        public void Init(Character _character)
        {
            character = _character;
            player = character.player;

            selected = false;
            dragged = false;
            boxCollider = GetComponent<BoxCollider2D>();
            DisableForSelection();
        }

        public void ShowCard()
        {
            this.transform.localEulerAngles = Vector3.zero;
        }
        
        public void HideCard()
        {
            this.transform.localEulerAngles = new Vector3 (0f, 180f, 0f);
        }

        public void DisableForSelection()
        {
            boxCollider.enabled = false;
            hoverSprite.SetActive(false);
            selectedSprite.SetActive(false);
        }

        public void EnableForSelection()
        {
            boxCollider.enabled = true;
        }

        public void ToggleSelected()
        {
            selected = !selected;
            selectedSprite.SetActive(selected);
        }

        #region Unity 

        private void FixedUpdate()
        {
            if (dragged)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos = new Vector3(pos.x, pos.y, -12f);
                transform.position = pos;

                RaycastHit hit;
                int layer = LayerMask.NameToLayer("Board");

                Debug.Log("Trying raycast");
                Debug.DrawRay(pos, Vector3.forward * 20, Color.red);

                if (Physics.Raycast(pos, Vector3.forward, out hit, 20f))
                {
                    Debug.Log("Found collider: " + hit.collider.gameObject);
                    Location newLoc = hit.collider.GetComponentInParent<Location>();
                    if(newLoc != null)
                    {
                        if (newLoc != tempLocation)
                        {
                            tempLocation?.invisibleCard.Deselect();
                            tempLocation = newLoc;
                            tempLocation.invisibleCard.Select();
                        }
                    }
                    else
                    {
                        tempLocation?.invisibleCard.Deselect();
                        tempLocation = null;
                    }
                }
            }
        }

        private void OnMouseEnter() { hoverSprite.SetActive(true); }
        private void OnMouseExit() { hoverSprite.SetActive(false); }
        private void OnMouseDown() {
            ToggleSelected();
            lastPosition = transform.position;

            player.ActivateCardLocationSelector(cardType);
            
        }

        private void OnMouseUp()
        {
            player.DectivateCardLocationSelector();
            if (dragged)
            {
                dragged = false;
                ToggleSelected();
                if(tempLocation != null && player.CanPlayCard(this))
                {
                    player.PlayCardAtLocation(this, tempLocation);
                }
                else
                {
                    transform.position = lastPosition;
                }
            }
        }

        private void OnMouseDrag() { dragged = true; }

        #endregion
    }
}
