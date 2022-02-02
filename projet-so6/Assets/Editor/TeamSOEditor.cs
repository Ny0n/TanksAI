using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TeamSO)), CanEditMultipleObjects]
public class TeamSOEditor : Editor
{
    private TeamSO _var;

    private void OnEnable()
    {
        _var = target as TeamSO;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Notify Change"))
            _var.NotifyChange();
        
        base.OnInspectorGUI();
    }
}
