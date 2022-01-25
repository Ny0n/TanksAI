using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TeamsListSO _teamsList;
    [SerializeField] private TeamVariableSO _controllingTeam;
    [SerializeField] private BoolVariableSO _isSomeoneOnControlPoint;
    [SerializeField] private FloatVariableSO _winPoints;

    [Serializable]
    private struct TeamUIElements
    {
        public Text title;
        public Text body;
    }

    [Header("Team 1 elements")]
    [SerializeField] private TeamUIElements _team1UI;
    
    [Header("Team 2 elements")]
    [SerializeField] private TeamUIElements _team2UI;
    
    [Header("Team 3 elements")]
    [SerializeField] private TeamUIElements _team3UI;
    
    [Header("Team 4 elements")]
    [SerializeField] private TeamUIElements _team4UI;

    private List<TeamUIElements> _teamsUI;

    // Start is called before the first frame update
    void Start()
    {
        _teamsUI = new List<TeamUIElements>();
        _teamsUI.Add(_team1UI);
        _teamsUI.Add(_team2UI);
        _teamsUI.Add(_team3UI);
        _teamsUI.Add(_team4UI);

        UpdateTeams();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isSomeoneOnControlPoint.Value)
            return;
        
        // we update only the team that is in control of the point
        int index = _teamsList.Value.FindIndex(t => t.Equals(_controllingTeam.Value));
        UpdateTeam(index);
    }

    private void OnTeamsUpdated() // TODO link to teams modif
    {
        UpdateTeams();
    }

    private void UpdateTeams()
    {
        for (int i = 1; i <= 4; i++)
        {
            UpdateTeam(i);
        }
    }

    private void UpdateTeam(int index) // 1-4
    {
        TeamSO team = null;
        if (_teamsList.Value.Count <= index)
            team = _teamsList.Value[index-1];
        UpdateTitle(team, _teamsUI[index-1]);
        UpdateBody(team, _teamsUI[index-1]);
    }
    
    private void UpdateTitle(TeamSO team, TeamUIElements teamUI)
    {
        if (team == null)
        {
            teamUI.title.text = String.Empty;
            return;
        }
        
        teamUI.title.color = team.Color;
        teamUI.title.text = team.Name + " Team";
    }

    private void UpdateBody(TeamSO team, TeamUIElements teamUI)
    {
        if (team == null)
        {
            teamUI.body.text = String.Empty;
            return;
        }
        
        teamUI.body.text = String.Empty;
        teamUI.body.text = ((team.Points / _winPoints.Value) * 100).ToString();
        teamUI.body.text += "\n";
        teamUI.body.text += "(" + team.Points + "/" + _winPoints.Value + ")";
    }
}
