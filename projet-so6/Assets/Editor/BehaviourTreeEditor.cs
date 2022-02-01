using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.IMGUI.Controls;
using UnityEditor.VersionControl;

namespace BehaviourTree
{
    public class BehaviourTreeEditor : EditorWindow
    {
        private BehaviourTreeView treeView;
        private InspectorView inspectorView;
        private IMGUIContainer blackboardView;

        private SerializedObject treeObject;
        private SerializedProperty blackboardProperty;
        
        [MenuItem("BehaviourTreeEditor/Editor ...")]
        public static void OpenWindow()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is BehaviourTree)
            {
                OpenWindow();
                return true;
            }

            return false;
        }
        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTreeEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviourTreeView>();
            inspectorView = root.Q<InspectorView>();
            blackboardView = root.Q<IMGUIContainer>();

            blackboardView.onGUIHandler = () =>
            {
                if (treeObject != null && treeObject.targetObject != null)
                {
                    treeObject.Update();
                    EditorGUILayout.PropertyField(blackboardProperty);
                    treeObject.ApplyModifiedProperties();
                }
            };

            treeView.OnNodeSelected = OnNodeSelectionChanged;
            
            OnSelectionChange();
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    OnSelectionChange();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }
        

        private void OnSelectionChange()
        {
            BehaviourTree tree = Selection.activeObject as BehaviourTree;
            if (!tree)
            {
                if (Selection.activeGameObject)
                {
                    BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                    if (runner)
                    {
                        tree = runner.tree;
                    }
                }
            }
            if (tree)
            { 
                treeView.PopulateView(tree);
            }

            if (tree != null)
            {
                treeObject = new SerializedObject(tree);
                blackboardProperty = treeObject.FindProperty("blackboard");
            }
        }

        void OnNodeSelectionChanged(NodeView node)
        {
            inspectorView.UpdateSelection(node);
        }
        
        private void OnInspectorUpdate()
        {
            treeView?.UpdateNodeState();
        }
    }
    
}
