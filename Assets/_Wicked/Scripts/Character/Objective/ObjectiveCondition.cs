using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked
{
    public enum ObjectiveConditionTypeName
    {
        DefeatHero,
        UnlockLocation,
    }

    public abstract class ObjectiveCondition: MonoBehaviour
    {
        [HideInInspector]
        public Objective objective;
        [ReadOnly]
        public ObjectiveConditionTypeName conditionType;

        public virtual void Init(Objective _objective)
        {
            objective = _objective;

            SetConditionType();
            AddListeners();
        }

        public virtual void DeInit()
        {
            RemoveListeners();
        }

        public abstract void AddListeners();
        public abstract void RemoveListeners();
        public abstract void SetConditionType();

        public void OnDestroy()
        {
            DeInit();
        }
    }
}
