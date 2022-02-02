using UnityEditor;
using UnityEngine;

public abstract class ObservableListEditor<T> : Editor
{
    private ObservableListSO<T> _var;

    private void OnEnable()
    {
        _var = target as ObservableListSO<T>;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Notify Change"))
            _var.NotifyChange();
        
        base.OnInspectorGUI();
    }
}
