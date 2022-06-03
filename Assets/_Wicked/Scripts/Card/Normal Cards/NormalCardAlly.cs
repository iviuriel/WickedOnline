using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked
{
    public class NormalCardAlly : Card
    {
        public int powerCost;
        public int strength;

        public override void Init(Character _character)
        {
            base.Init(_character);
        }

        public override void SetCardType()
        {
            cardType = CardType.Normal;
        }

        public override void SetFateCardType() {return;}

        public override void SetNormalCardType()
        {
            normalCardType = NormalCardType.Ally;
        }
    }
}
