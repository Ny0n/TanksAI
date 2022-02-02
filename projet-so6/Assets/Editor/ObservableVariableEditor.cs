using UnityEditor;
using UnityEngine;

public abstract class ObservableVariableEditor<T> : Editor
{
    private ObservableVariableSO<T> _var;

    private void OnEnable()
    {
        _var = target as ObservableVariableSO<T>;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Notify Change"))
            _var.NotifyChange();
        
        base.OnInspectorGUI();
    }
}
