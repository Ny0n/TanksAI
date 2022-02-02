using UnityEditor;

[CustomEditor(typeof(TeamListSO)), CanEditMultipleObjects]
public class TeamListEditor : ObservableListEditor<TeamSO> { }
