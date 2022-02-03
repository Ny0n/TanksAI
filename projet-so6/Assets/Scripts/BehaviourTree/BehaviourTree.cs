using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        public Node rootNode;
        public Node.State treeState = Node.State.Running;
        public List<Node> nodes = new List<Node>();
        public Blackboard blackboard = new Blackboard();
        
        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running)
            {
                return rootNode.Update();
            }

            return treeState;
        }

        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            
            Undo.RecordObject(this, "Behaviour Tree (Create Node)");
            nodes.Add(node);

            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            
            Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (Create Node)");
            
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "Behaviour Tree (Delete Node)");
            nodes.Remove(node);
            
            AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);

            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (Add Child)");
                decorator.child = child;
                EditorUtility.SetDirty(decorator);
            }
            
            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (Add Child)");
                composite.children.Add(child);
                EditorUtility.SetDirty(composite);
            }

            RootNode root = parent as RootNode;
            if (root)
            {
                Undo.RecordObject(root, "Behaviour Tree (Add Child)");
                root.child = child;
                EditorUtility.SetDirty(root);
            }
        }
        public void RemoveChild(Node parent, Node child)
        {
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (Remove Child)");
                decorator.child = null;
                EditorUtility.SetDirty(decorator);

            }
            
            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (Remove Child)");
                composite.children.Remove(child);
                EditorUtility.SetDirty(composite);
            }
            
            RootNode root = parent as RootNode;
            if (root)
            {
                Undo.RecordObject(root, "Behaviour Tree (Remove Child)");
                root.child = null;
                EditorUtility.SetDirty(root);
            }
            
        }
        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();
            
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator && decorator.child != null)
            {
                children.Add(decorator.child);
            }
            
            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                return composite.children;
            }
            
            RootNode root = parent as RootNode;
            if (root && root.child != null)
            {
                children.Add(root.child);
            }
            

            return children;
        }
        
        public void Traverse(Node node, System.Action<Node> visitor)
        {
            if (node)
            {
                visitor.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n) => Traverse(n, visitor));
            }
        }
        
        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();
            tree.nodes = new List<Node>();
            Traverse(tree.rootNode, (n) =>
            {
                tree.nodes.Add(n);
            });
            return tree;
        }

        public void Bind()
        {
            Traverse(rootNode, node =>
            {
                node.blackboard = blackboard;
            });
        }
    }
    
}
