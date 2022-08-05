using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked
{
    public static class WickedUtils
    {
        public static class AlertStrings
        {
            public static string CannotAddAnotherHero = "Some hero was already selected to be confronted";
            public static string HeroMissing = "No HERO was selected to vanquish";
            public static string AttackersMissing = "You need to select at least one ALLY to vanquish";
            public static string NotEnoughStrength = "Your allies have not enough strength to defeat that hero";
            public static string WrongLocationVanquish = "One or more of the attackers can vanquish because they are "+
                "at the same location than the defender";
        }

        public static string GetCharacterString(CharacterName name)
        {
            switch(name)
            {
                case CharacterName.CaptainHook:
                    return "CaptainHook";
                case CharacterName.Ursula:
                    return "Ursula";
                case CharacterName.PrinceJohn:
                    return "PrinceJohn";
                case CharacterName.Jafar:
                    return "Jafar";
                case CharacterName.Maleficent:
                    return "Maleficent";
                case CharacterName.QueenOfHearts:
                    return "QueenOfHearts";
                default:
                    return "";
            }
        }

        public static string GetActionString(ActionType action)
        {
            switch(action)
            {
                case ActionType.GainPower:
                    return "GainPower";
                case ActionType.DiscardCards:
                    return "DiscardCards";
                case ActionType.PlayCard:
                    return "PlayCard";
                case ActionType.Vanquish:
                    return "Vanquish";
                case ActionType.MoveItemOrAlly:
                    return "MoveItemOrAlly";
                case ActionType.MoveHero:
                    return "MoveHero";
                case ActionType.Fate:
                    return "Fate";
                default:
                    return "";
            }
        }

        public static void SetLayerToGameObject(GameObject go, int layer)
        {
            go.layer = layer;
        }
    }
}

