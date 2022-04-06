using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TeamsListSO _teamsList;
    [SerializeField] private TeamVariableSO _controllingTeam;
    [SerializeField] private BoolVariableSO _isControllingTeamOnPoint;
    [SerializeField] private SettingsVariableSO _settings;
    [SerializeField] private FloatVariableSO _timer;

    [Serializable]
    private struct TeamUIElements
    {
        public Text title;
        public Text body;
    }

    [Header("UI elements")]
    [SerializeField] private Text _timerText;
    
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

    private void OnEnable()
    {
        _controllingTeam.ValueChanged += OnControllingTeamUpdated;
        _settings.ValueChanged += OnDataUpdated;
    }

    private void OnDisable()
    {
        _controllingTeam.ValueChanged -= OnControllingTeamUpdated;
        _settings.ValueChanged -= OnDataUpdated;
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
        UpdateTimerState();
    }

    // Update is called once per frame
    void Update()
    {
        if (_settings.Value.GameMode == SettingsSO.GameModeType.MostPointsWin)
            UpdateTimer();
        
        if (!_isControllingTeamOnPoint.Value)
            return;
        
        // we update only the team that is in control of the point
        UpdateTeam(_controllingIndex+1);
    }

    private void OnTeamsUpdated() // TODO link to teams modif
    {
        UpdateTeams();
    }

    private void UpdateTimerState()
    {
        if (_settings.Value.GameMode == SettingsSO.GameModeType.FirstToMaximumPointsWin)
            _timerText.text = string.Empty;
    }

    private void UpdateTimer()
    {
        if (_timer.Value < 30)
            _timerText.text = TimeSpan.FromSeconds(_timer.Value).ToString(@"mm\:ss\:ff");
        else
            _timerText.text = TimeSpan.FromSeconds(_timer.Value).ToString(@"mm\:ss");
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

        switch (_settings.Value.GameMode)
        {
            case SettingsSO.GameModeType.FirstToMaximumPointsWin:
                teamUI.body.text = String.Empty;
                teamUI.body.text = ((team.Points / _settings.Value.MaximumPoints) * 100).ToString("F2") + "%";
                teamUI.body.text += "\n";
                teamUI.body.text += "(" + team.Points.ToString("F1") + "/" + _settings.Value.MaximumPoints + ")";
                break;
            case SettingsSO.GameModeType.MostPointsWin:
                teamUI.body.text = String.Empty;
                teamUI.body.text += team.Points.ToString("F1");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
    }
}
