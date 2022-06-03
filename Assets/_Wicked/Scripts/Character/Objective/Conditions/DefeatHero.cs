using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked
{
    public class DefeatHero : ObjectiveCondition
    {
        public override void Init(Objective _objective)
        {
            base.Init(_objective);
        }

        public override void DeInit()
        {
            base.DeInit();
        }

        public override void SetConditionType()
        {
            conditionType = ObjectiveConditionTypeName.DefeatHero;
        }

        public override void AddListeners()
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveListeners()
        {
            throw new System.NotImplementedException();
        }
    }
}
