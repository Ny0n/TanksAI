using System;
using UnityEngine;

namespace BehaviourTree
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        private BehaviourTree tree;

        private void Start()
        {
            tree = ScriptableObject.CreateInstance<BehaviourTree>();
            
            var log = ScriptableObject.CreateInstance<DebugLogNode>();
            log.message = "test";

            tree.rootNode = log;
        }

        private void Update()
        {
            tree.Update();
        }
    }
}
