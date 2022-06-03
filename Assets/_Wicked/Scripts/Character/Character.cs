using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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

        [Title("Card Sprites")]
        [PreviewField] public Sprite cardBack;
        [PreviewField] public Sprite fateCardBack;

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
        }

        public void MoveAvatar(Location location)
        {
            Vector3 locPos = location.transform.position;
            mover.transform.position = new Vector3(locPos.x, locPos.y, mover.transform.position.z);
        }
    }
}
