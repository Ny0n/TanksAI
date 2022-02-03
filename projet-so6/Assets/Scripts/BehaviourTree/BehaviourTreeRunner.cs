using System;
using UnityEngine;

namespace BehaviourTree
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;

        private void Start()
        {
            if (!tree) return;

            Initialize();
        }

        public void Initialize()
        {
            tree = tree.Clone();
            tree.Bind();
        }

        private void Update()
        {
            tree.Update();
        }
    }
}
