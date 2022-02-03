using System;
using UnityEngine;

namespace BehaviourTree
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree originTree;
        public BehaviourTree runningTree;
        private BehaviourTree _previousTree;
        private bool _hasInitialized;

        private void Start()
        {
            if (!originTree) return;

            if (!_hasInitialized)
                Initialize();
        }

        private void OnEnable()
        {
            Update();
        }
        
        private void OnDisable()
        {
            _hasInitialized = false;
        }

        public void Initialize()
        {
            runningTree = originTree.Clone();
            
            // we repopulate the original gamemanager's dico
            foreach (var kvp in originTree.blackboard.Dico)
                runningTree.blackboard.SetValue(kvp.Key, kvp.Value);
            
            runningTree.Bind();
            _hasInitialized = true;
            _previousTree = runningTree;
        }

        private void Update()
        {
            if (!originTree) return;
            
            if (runningTree != _previousTree) 
                _hasInitialized = false;

            if (!_hasInitialized)
                Initialize();
            
            runningTree.Update();
        }
    }
}
