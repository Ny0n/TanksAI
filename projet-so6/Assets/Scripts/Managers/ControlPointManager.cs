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
    
    [SerializeField] private FloatVariableSO _winPoints;
    [SerializeField] private CooldownSO _captureDrop;
    [SerializeField] private TeamVariableSO _controllingTeam; // IA data #1
    [SerializeField] private BoolVariableSO _isControllingTeamOnPoint;
    
    [SerializeField] private float _captureSpeed = 20;
    [SerializeField] private float _captureDropSpeed = 30;
    [SerializeField] private float _controlPointWinningSpeed = 4;
    [SerializeField] private bool _dropCaptureInstant;
    
    [SerializeField] private GameObject _helipad;
    [SerializeField] private Image _captureImage;
    
    private List<TeamSO> _teamsOnPoint; // IA data #3
    private List<GameObject> _tanksOnPoint; // IA data #4

    private (TeamSO team, float progress) _capture; // IA data #2
    
    private void OnEnable()
    {
        _controllingTeam.ValueChanged += OnControllingTeamUpdated;
    }

    private void OnDisable()
    {
        _controllingTeam.ValueChanged -= OnControllingTeamUpdated;
    }

    private void OnControllingTeamUpdated()
    {
        if (_controllingTeam.Value != null)
        {
            // change helipad color to reflect the team that captured it
            MeshRenderer[] renderers = _helipad.GetComponentsInChildren<MeshRenderer>();
            foreach (var m in renderers)
                m.material.color = _controllingTeam.Value.Color;
            
            // reset the capture bar
            _captureImage.fillAmount = 0;
            _captureImage.fillClockwise = false;
            _captureImage.color = Color.white;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _controllingTeam.Value = default;
        _isControllingTeamOnPoint.Value = default;

        _teamsOnPoint = new List<TeamSO>();
        _tanksOnPoint = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_teamsOnPoint.Contains(_capture.team)) // as long as the capturing team is on the point, we don't drop the capture
            _captureDrop.StartCooldown();
            
        if (_captureDrop.IsCooldownDone() && _capture.progress > 0)
        {
            if (!_dropCaptureInstant)
            {
                // by default, when the team that was capturing left the point, after a specified cooldown, we start resetting the progress to 0
                SetCaptureProgress(Mathf.Max(_capture.progress - _captureDropSpeed * Time.deltaTime, 0));
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
            _controllingTeam.Value.Points = Mathf.Min(_controllingTeam.Value.Points + _controlPointWinningSpeed * Time.deltaTime, _winPoints.Value);
        }
        else // if there are no controlling team or it's an other one, we start the capture
        {
            AddCapture(team);
        }
    }

    private void AddCapture(TeamSO team)
    {
        if (team == _capture.team)
        {
            SetCaptureProgress(Mathf.Min(_capture.progress + _captureSpeed * Time.deltaTime, 100));
        }
        else // if it's not the same team
        {
            if (_capture.progress <= 0)
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
            // ResetCapture();
        }
        else if (_capture.progress >= 100)
        {
            CapturePoint(_capture.team);
        }
    }

    private void CapturePoint(TeamSO team)
    {
        // the func to make a team control the point
        _controllingTeam.Value = team;
        ResetCapture();
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
            TeamSO team = tank.GetComponent<TankTeam>().Team;
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
