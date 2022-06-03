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

        public ActionType actionType;

        [ShowIf("actionType", ActionType.GainPower)]
        public int powerGained;

        public void Init(Location _location)
        {
            location = _location;
        }
    }
}
