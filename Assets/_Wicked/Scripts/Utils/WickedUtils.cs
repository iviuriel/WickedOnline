using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked
{
    public static class WickedUtils
    {
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

