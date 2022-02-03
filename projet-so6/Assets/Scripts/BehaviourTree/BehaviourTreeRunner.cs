using System;
using UnityEngine;

namespace BehaviourTree
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;
        private BehaviourTree _previousTree;
        private bool _hasInitialized;

        private void Start()
        {
            if (!tree) return;

            if (!_hasInitialized)
                Initialize();
        }

        public void Initialize()
        {
            tree = tree.Clone();
            tree.Bind();
            _hasInitialized = true;
            _previousTree = tree;
        }

        private void Update()
        {
            if (!tree) return;
            
            if (tree != _previousTree) 
                _hasInitialized = false;

            if (!_hasInitialized)
                Initialize();
            
            tree.Update();
        }
    }
}
