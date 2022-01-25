using UnityEditor;
using UnityEngine;

public abstract class GenericEventEditor : Editor
{
    private GenericEventSO _event;

    private void OnEnable()
    {
        _event = target as GenericEventSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Raise"))
        {
            _event.Raise();
        }
    }
}
