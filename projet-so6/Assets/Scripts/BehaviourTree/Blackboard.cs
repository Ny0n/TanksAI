using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [System.Serializable]
    public class Blackboard
    {
        public Dictionary<string, object> Values;
        public TeamsListSO TeamsList;

        public Blackboard()
        {
            Values = new Dictionary<string, object>()
            {
                {"targetPos", new Vector3()}
            };
        }
    }
}
