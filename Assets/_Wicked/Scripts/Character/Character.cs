using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace Wicked
{

    public enum CharacterName
    {
        CaptainHook,
        Jafar,
        Maleficent,
        PrinceJohn,
        QueenOfHearts,
        Ursula
    }

    public class Character : MonoBehaviour
    {
        [Title("Character Info")]
        public CharacterName character;
        [PreviewField] public Sprite characterPortrait;
        [InlineEditor] public Objective objective;
        [InlineEditor] public Domain domain;
        public GameObject mover;
        [HideInInspector] public GameObject domainGameObject;

        [Title("Cards")]
        [InlineEditor] public Deck normalDeck;
        [InlineEditor] public Deck fateDeck;
        [InlineEditor] public Deck normalDiscardDeck;
        [InlineEditor] public Deck fateDiscardDeck;

        [Title("Card Sprites")]
        [PreviewField] public Sprite cardBack;
        [PreviewField] public Sprite fateCardBack;

        [Title("UI")]
        public TextMeshProUGUI powerText;
        public GameObject discardUI;
        public GameObject vanquishUI;

        ///Private variables
        private Location curLocation;

        [HideInInspector]
        public PlayerManager player;

        public void Init(PlayerManager _player)
        {
            player = _player;

            objective.Init(this);
            domain.Init(this);
            domainGameObject = domain.gameObject;

            normalDeck.Init(this);
            fateDeck.Init(this);
            normalDiscardDeck.Init(this);
            fateDiscardDeck.Init(this);

            HideDiscardUI();
        }

        public void MoveAvatar(Location location)
        {
            Vector3 locPos = location.transform.position;
            mover.transform.position = new Vector3(locPos.x, locPos.y, mover.transform.position.z);
        }

        public List<Card> DrawCards(int amount)
        {
            return normalDeck.DrawCards(amount);
        }


        #region UI

        public void UpdatePowerUI(int power)
        {
            powerText.text = power.ToString();
        }

        public void ShowDiscardUI() { discardUI.SetActive(true); }
        public void HideDiscardUI() { discardUI.SetActive(false); }

        public void ShowVanquishUI() { vanquishUI.SetActive(true); }
        public void HideVanquishUI() { vanquishUI.SetActive(false); }

        public void UpdateVanquishUI(int powerDefender, int powerAttacker)
        {
            return;
        }

        public void AlertUI(string text)
        {
            return;
        }
        #endregion

        #region Overrides

        public override string ToString()
        {
            return WickedUtils.GetCharacterString(character);
        }
        #endregion

    }
}
