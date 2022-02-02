using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // [SerializeField] private TeamsListSO _teamsList;
    [SerializeField] private TeamListSO _teamsList;
    [SerializeField] private TeamVariableSO _controllingTeam;
    [SerializeField] private BoolVariableSO _isControllingTeamOnPoint;
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

    private int _controllingIndex;
    
    private void Awake()
    {
        _teamsList.Init();
    }

    private void OnEnable()
    {
        _teamsList.AddObserver(OnTeamsUpdated);
        _controllingTeam.ValueChanged += OnControllingTeamUpdated;
        _winPoints.ValueChanged += OnDataUpdated;
    }

    private void OnDisable()
    {
        _teamsList.RemoveObserver(OnTeamsUpdated);
        _controllingTeam.ValueChanged -= OnControllingTeamUpdated;
        _winPoints.ValueChanged -= OnDataUpdated;
    }

    
    [ContextMenu("Register")]
    public void Register()
    {
        // _teamsList._value.ValueChanged += OnTeamsUpdated;
    }
    
    [ContextMenu("Unregister")]
    public void Unregister()
    {
        // _teamsList._value.ValueChanged -= OnTeamsUpdated;
    }

    private void OnDataUpdated()
    {
        UpdateTeams();
    }

    private void OnControllingTeamUpdated()
    {
        if (_controllingTeam.Value != null)
            _controllingIndex = _teamsList.Value.FindIndex(t => t.Equals(_controllingTeam.Value));
        else
            _controllingIndex = 0;
    }

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
        if (!_isControllingTeamOnPoint.Value)
            return;
        
        // we update only the team that is in control of the point
        UpdateTeam(_controllingIndex+1);
    }

    private void OnTeamsUpdated()
    {
        Debug.Log("oi?!");
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
        if (index < 1 || index > 4)
            return;
        
        TeamSO team = null;
        if (_teamsList.Value.Count >= index)
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
        teamUI.body.text = ((team.Points / _winPoints.Value) * 100).ToString("F2") + "%";
        teamUI.body.text += "\n";
        teamUI.body.text += "(" + team.Points.ToString("F1") + "/" + _winPoints.Value + ")";
    }
}
