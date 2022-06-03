using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;


#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Wicked
{
    [Serializable]
    public class Objective: MonoBehaviour
    {
        [HideInInspector]
        public Character character;

        [ReadOnly]
        public string description;

        [OnCollectionChanged("CollectionChanged")]
        [SerializeReference]
        public List<ObjectiveCondition> conditions = new List<ObjectiveCondition>();

        public bool checkObjectiveWhenTurnBegins = false;

        #region Odin Inspector

        #if UNITY_EDITOR
        private void CollectionChanged(CollectionChangeInfo info, object value)
        {
            UpdateDescription();
        }
        #endif

        [Button("Update Description")]
        public void UpdateDescription()
        {
            StringBuilder sb = new StringBuilder();

            if(conditions.Count == 0)
            {
                description = "No objectives.";
                return;
            }

            for(int i = 0; i < conditions.Count; i++)
            {
                sb.Append(conditions[i].ToString());
                if(i == conditions.Count - 2)
                {
                    sb.Append(" and ");
                }
                else
                {
                    sb.Append(", ");
                }
            }

            description = sb.ToString();
        }

        #endregion

        public void Init(Character _character)
        {
            character = _character;
        }
    }
}
