using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class ControlPointManager : MonoBehaviour
{
    /*
     * desc du prof pour comment ça marche:
     *
     * - au début: quand une équipe arrive, il faut charger sa barre à 100% pour capturer -> capture = helipad est coloré
     * - **il faut toujours au moins un tank de l'equipe qui a capturé le point pour que la barre monte**
     * - si il y a un tank de chaque équipe, rien ne se passe / ça met en pause la capture
     * - si un tank de l'autre équipe vient pour capturer, ça commence direct sa capture à 0% (ça décharge pas l'autre, donc *pas* comme overwatch)
     * 
     * */
    
    [Header("Misc Data")]
    [SerializeField] private SettingsVariableSO _settings;
    [SerializeField] private BoolVariableSO _gameEnded;
    [SerializeField] private TeamsListSO _teamsList;
    
    [Header("Control Point Data")]
    [SerializeField] private CooldownSO _captureDrop;
    [SerializeField] private TeamVariableSO _controllingTeam; // IA data #1
    [SerializeField] private BoolVariableSO _isControllingTeamOnPoint;
    
    [Header("Capture settings")]
    [SerializeField] private float _captureSpeed = 30;
    [SerializeField] private float _captureSpeedBonusPerTank = 10;
    [SerializeField] private float _captureDropAutoSpeed = 10;
    [SerializeField] private float _captureDropEnemySpeed = 30;
    [SerializeField] private float _captureDropEnemySpeedBonusPerTank = 10;
    [SerializeField] private bool _dropCaptureInstant;
    
    [Header("Points settings")]
    [SerializeField] private float _pointsWinningSpeed = 4;
    [SerializeField] private float _pointsWinningSpeedBonusPerTank = 2;
    
    [Header("UI/World Data")]
    [SerializeField] private GameObject _helipad;
    [SerializeField] private Image _captureImage;

    private List<Color> _savedColors;

    private List<TeamSO> _teamsOnPoint; // IA data #3
    private List<GameObject> _tanksOnPoint; // IA data #4

    private (TeamSO team, float progress) _capture; // IA data #2
    
    private void OnEnable()
    {
        _controllingTeam.ValueChanged += OnControllingTeamUpdated;
        _gameEnded.ValueChanged += OnGameEndedUpdated;
    }

    private void OnDisable()
    {
        _controllingTeam.ValueChanged -= OnControllingTeamUpdated;
        _gameEnded.ValueChanged -= OnGameEndedUpdated;
    }

    private void OnGameEndedUpdated()
    {
        if (_gameEnded.Value)
        {
            if (_settings.Value.GameMode == SettingsSO.GameModeType.FirstToMaximumPointsWin)
            {
                // clamp to max points on game end
                foreach (var team in _teamsList.Value)
                    team.Points = Mathf.Min(team.Points, _settings.Value.MaximumPoints);
            }
        }
    }

    private void OnControllingTeamUpdated()
    {
        if (_controllingTeam.Value != null)
        {
            // change helipad color to reflect the team that captured it
            MeshRenderer[] renderers = _helipad.GetComponentsInChildren<MeshRenderer>();
            foreach (var m in renderers)
                m.material.color = _controllingTeam.Value.Color;
            
            // // reset the capture bar
            // _captureImage.fillAmount = 0;
            // _captureImage.fillClockwise = false;
            // _captureImage.color = Color.white;
        }
        else
        {
            // reset the helipad color
            MeshRenderer[] renderers = _helipad.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].material.color = _savedColors[i];
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _teamsOnPoint = new List<TeamSO>();
        _tanksOnPoint = new List<GameObject>();
        _savedColors = new List<Color>();
        MeshRenderer[] renderers = _helipad.GetComponentsInChildren<MeshRenderer>();
        foreach (var m in renderers)
            _savedColors.Add(m.material.color);
        
        _controllingTeam.Value = default;
        _isControllingTeamOnPoint.Value = default;
        _capture = default;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameEnded.Value)
            return;
        
        if (_teamsOnPoint.Contains(_capture.team)) // as long as the capturing team is on the point, we don't drop the capture
            _captureDrop.StartCooldown();
            
        if (_captureDrop.IsCooldownDone() && _capture.progress > 0)
        {
            if (!_dropCaptureInstant)
            {
                // by default, when the team that was capturing left the point, after a specified cooldown, we start resetting the progress to 0
                SetCaptureProgress(Mathf.Max(_capture.progress - _captureDropAutoSpeed * Time.deltaTime, 0));
            }
            else
            {
                SetCaptureProgress(0);
            }
        }
        
        // if there is only one team on the point
        if (_teamsOnPoint.Count != 1)
            return;
        
        // multiple cases here

        TeamSO team = _teamsOnPoint[0];
        if (_controllingTeam.Value == team) // if it's the controlling team, we give it points
        {
            AddCapture(team); // we also keep its capture progress for as long as the team's on the point
            AddPoints(_controllingTeam.Value);
        }
        else // if there are no controlling team or it's an other one, we start the capture
        {
            AddCapture(team);
        }
    }

    private void AddPoints(TeamSO team)
    {
        // considering that if we're here, there is only one team on the point
        float speedBonus = _pointsWinningSpeedBonusPerTank * (_tanksOnPoint.Count - 1);
        team.Points += (_pointsWinningSpeed + speedBonus) * Time.deltaTime;
    }

    private void AddCapture(TeamSO team)
    {
        // again, if we're here, that means that there's only one team on the point
        if (team == _capture.team) // if it's the team that is already capturing
        {
            float speedBonus = _captureSpeedBonusPerTank * (_tanksOnPoint.Count - 1);
            SetCaptureProgress(Mathf.Min(_capture.progress + (_captureSpeed + speedBonus) * Time.deltaTime, 100));
        }
        else // if it's not the same team
        {
            if (_captureDrop.IsCooldownDone() && _capture.progress > 0)
            {
                // when an other team comes to capture the point, it drops faster
                
                // first, due to the way that i did the code (and due to time limits it's not worth changing)
                // we also have to cancel the auto drop
                SetCaptureProgress(Mathf.Min(_capture.progress + _captureDropAutoSpeed * Time.deltaTime, 100));
                
                // then we drop bc there's enemies on the point
                float speedBonus = _captureDropEnemySpeedBonusPerTank * (_tanksOnPoint.Count - 1);
                SetCaptureProgress(Mathf.Max(_capture.progress - (_captureDropEnemySpeed + speedBonus) * Time.deltaTime, 0));
            }
            
            if (_capture.progress <= 0) // switch team capture
            {
                _capture.team = team;
                _captureImage.fillClockwise = !_captureImage.fillClockwise;
                _captureImage.color = team.Color;
            }
        }
    }

    private void SetCaptureProgress(float value)
    {
        _captureImage.fillAmount = value / 100;
        _capture.progress = value;

        if (_capture.progress <= 0)
        {
            ReleasePoint();
        }
        else if (_capture.progress >= 100)
        {
            if (_controllingTeam.Value != _capture.team) // if the team doesn't already own the point
                CapturePoint(_capture.team);
        }
    }

    private void CapturePoint(TeamSO team)
    {
        // the func to make a team control the point
        _controllingTeam.Value = team;
        // ResetCapture();
        CheckControllingTeamForUI();
    }
    
    private void ReleasePoint()
    {
        // the func to make a team lose the control of the point
        _controllingTeam.Value = null;
        // ResetCapture();
        CheckControllingTeamForUI();
    }

    private void ResetCapture()
    {
        _capture = default;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Tank"))
            return;
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        if (!_tanksOnPoint.Contains(other.gameObject))
        {
            _tanksOnPoint.Add(other.gameObject);
            UpdateTeamsOnPoint();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Tank"))
            return;
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
        
        if (_tanksOnPoint.Contains(other.gameObject))
        {
            _tanksOnPoint.Remove(other.gameObject);
            UpdateTeamsOnPoint();
        }
        
    }

    private void UpdateTeamsOnPoint()
    {
        _teamsOnPoint.Clear();
        foreach (var tank in _tanksOnPoint)
        {
            TeamSO team = tank.GetComponent<TankData>().Team;
            if (!_teamsOnPoint.Contains(team))
                _teamsOnPoint.Add(team);
        }

        CheckControllingTeamForUI();
    }

    private void CheckControllingTeamForUI()
    {
        // we update _isControllingTeamOnPoint for the UI
        if (_teamsOnPoint.Contains(_controllingTeam.Value))
        {
            if (!_isControllingTeamOnPoint.Value)
                _isControllingTeamOnPoint.Value = true;
        }
        else
        {
            if (_isControllingTeamOnPoint.Value)
                _isControllingTeamOnPoint.Value = false;
        }
    }
}
