using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Observable/List/Team")]
public class TeamListSO : ObservableListSO<TeamSO>
{
    [SerializeField] private TeamSO _team1;
    [SerializeField] private TeamSO _team2;
    [SerializeField] private TeamSO _team3;
    
    public new void Init()
    {
        base.Init();
        _value.Add(_team1);
        _value.Add(_team2);
        _value.Add(_team3);
    }
}
