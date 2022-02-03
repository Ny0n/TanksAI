using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SettingsVariableSO _settings;
    [SerializeField] private BoolVariableSO _gameEnded;
    [SerializeField] private FloatVariableSO _timer;
    [SerializeField] private TeamsListSO _teamsList;
    
    [Header("General Settings")]
    [SerializeField] private float _tankRespawnTime = 5f;
    [SerializeField] private float _startDelay = 3f;        // The delay between the start of RoundStarting and RoundPlaying phases.
    [SerializeField] private float _endDelay = 3f;          // The delay between the end of RoundPlaying and RoundEnding phases.
    
    [Space]
    [SerializeField] private CameraControl _cameraControl;  // Reference to the CameraControl script for control during different phases.
    [SerializeField] private Text _messageText;             // Reference to the overlay Text to display winning text, etc.

    private WaitForSeconds _startWait;  // Used to have a delay whilst the round starts.
    private WaitForSeconds _endWait;    // Used to have a delay whilst the round or game ends.
    
    [SerializeField] private GameObject _playerTankPrefab;
    [SerializeField] private GameObject _aiTankPrefab;
    
    private TeamSO _winningTeam;
    private List<TankManager> _tanks;

    private bool _isTimerActive;
    private List<TeamSO> _drawTeams; // just in case

    const float k_MaxDepenetrationVelocity = float.PositiveInfinity;

    private void Start()
    {
        _tanks = new List<TankManager>();
        _drawTeams = new List<TeamSO>();
        _gameEnded.Value = false;
        
        // This line fixes a change to the physics engine.
        Physics.defaultMaxDepenetrationVelocity = k_MaxDepenetrationVelocity;
        
        // Create the delays so they only have to be made once.
        _startWait = new WaitForSeconds(_startDelay);
        _endWait = new WaitForSeconds(_endDelay);

        CreateAllTanks();
        SetCameraTargets();

        // Once the tanks have been created and the camera is using them as targets, start the game.
        StartCoroutine(GameLoop());

        ResetTimer();
        StartTimer();
    }

    private void Update()
    {
        if (!_gameEnded.Value)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(0); // restart the game
    }

    private void CreateAllTanks()
    {
        // For all the teams & all of the players inside
        int j = 0;
        foreach (var team in _teamsList.Value)
        {
            for (int i = 0; i < team.NbPlayers; i++)
            {
                CreateTank(team, i, i + 1 + j);
            }

            j += team.NbPlayers;
        }
    }

    private void CreateTank(TeamSO team, int teamTankNumber, int playerNumber)
    {
        // we create and spawn the tanks
        Debug.Log(playerNumber);
        TankManager tm = new TankManager(team, playerNumber);

        if (team.IsControlledByAI)
        {
            tm.tankInstance = Instantiate(_aiTankPrefab, team.SpawnPoint.position, team.SpawnPoint.rotation) as GameObject;
            BehaviourTreeRunner btRunner = tm.tankInstance.GetComponent<BehaviourTreeRunner>();
            if (teamTankNumber <= team.BTList.Count - 1)
            {
                btRunner.tree = team.BTList[teamTankNumber]; // set the designated tree to the tank
                btRunner.Initialize(); // clone it for the tank
                btRunner.tree.blackboard.Values.Add("tankManager", tm);
            }
        }
        else
        {
            tm.tankInstance = Instantiate(_playerTankPrefab, team.SpawnPoint.position, team.SpawnPoint.rotation) as GameObject;
        }
        tm.Setup();
                
        _tanks.Add(tm);
        team.Tanks.Add(tm);
    }

    public void OnTankDeath(GenericEventSO evt) // SO event
    {
        TankDeathEventSO e = (TankDeathEventSO) evt;
        StartCoroutine(RespawnTankCoroutine(e.Manager));
    }

    private IEnumerator RespawnTankCoroutine(TankManager tm)
    {
        yield return new WaitForSeconds(_tankRespawnTime);
        tm.Reset();
    }

    private void SetCameraTargets()
    {
        // Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[_tanks.Count];

        // For each of these transforms...
        for (int i = 0; i < targets.Length; i++)
        {
            // ... set it to the appropriate tank transform.
            targets[i] = _tanks[i].tankInstance.transform;
        }

        // These are the targets the camera should follow.
        _cameraControl.m_Targets = targets;
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameStarting());
        yield return StartCoroutine(GamePlaying());
        yield return StartCoroutine(GameEnding());
    }

    private IEnumerator GameStarting()
    {
        // As soon as the round starts reset the tanks and make sure they can't move.
        ResetAllTanks();
        DisableTankControl();

        // Snap the camera's zoom and position to something appropriate for the reset tanks.
        _cameraControl.SetStartPositionAndSize();

        _messageText.text = "GAME START";

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return _startWait;
    }

    private IEnumerator GamePlaying()
    {
        // As soon as the game starts we let the players/IA control the tanks.
        EnableTankControl();

        // Clear the text from the screen.
        _messageText.text = string.Empty;

        while (true)
        {
            switch (_settings.Value.GameMode)
            {
                case SettingsSO.GameModeType.FirstToMaximumPointsWin:
                    if (CheckForWinner())
                        yield break;
                    break;
                case SettingsSO.GameModeType.MostPointsWin:
                    if (_isTimerActive)
                    {
                        _timer.SetValueWithoutNotify(_timer.Value - Time.deltaTime);
                        if (_timer.Value <= 0)
                        {
                            _timer.Value = 0;
                            yield break;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            yield return null;
        }
    }

    private bool CheckForWinner()
    {
        // game mode 1
        return _teamsList.Value.FirstOrDefault(team => team.Points >= _settings.Value.MaximumPoints);
    }

    private IEnumerator GameEnding()
    {
        // Stop tanks from moving.
        DisableTankControl();
        
        // set the flag for game ended
        _gameEnded.Value = true;

        if (_teamsList.Value.Count <= 0)
        {
            Debug.LogWarning("There are no teams!");
            yield break;
        }

        // we find the winning team
        _winningTeam = null;
        switch (_settings.Value.GameMode)
        {
            case SettingsSO.GameModeType.FirstToMaximumPointsWin:
                _winningTeam = _teamsList.Value.FirstOrDefault(team => team.Points >= _settings.Value.MaximumPoints);
                break;
            case SettingsSO.GameModeType.MostPointsWin:
                (TeamSO team, float points) teamData;
                teamData.team = null;
                teamData.points = -1;
                
                foreach (var team in _teamsList.Value)
                {
                    if (team.Points > teamData.points)
                    {
                        _drawTeams.Clear();
                        _drawTeams.Add(team);
                        teamData.team = team;
                        teamData.points = team.Points;
                    } else if (Mathf.Approximately(team.Points, teamData.points))
                    {
                        _drawTeams.Add(team);
                    }
                }
    
                if (_drawTeams.Count <= 1)
                    _winningTeam = teamData.team;
                
                // float max = (from team in _teamsList.Value select team.Points).Max();
                // foreach (var team in _teamsList.Value)
                // {
                //     if (Mathf.Approximately(team.Points, max))
                //     {
                //         _winningTeam = team;
                //         break;
                //     }
                // }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // display the end message
        string message = EndMessage();
        _messageText.text = message;

        // wait and end the game
        yield return _endWait;
    }

    private string EndMessage()
    {
        string message;
        if (_winningTeam == null)
        {
            message = "Draw!";
            message += "\n";
            message += GetColoredName(_drawTeams[0]);
            for (int i = 1; i < _drawTeams.Count; i++)
            {
                message += "/";
                message += GetColoredName(_drawTeams[i]);
            }
        }
        else
        {
            message = GetColoredName(_winningTeam) + " wins the game!";
        }
         
        return message;
    }

    private string GetColoredName(TeamSO team)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(team.Color) + ">" + team.Name + "</color>";
    }

    #region Timer

    private void ResetTimer()
    {
        _timer.Value = _settings.Value.PlayTime;
    }

    private void StartTimer()
    {
        _isTimerActive = true;
    }
    
    private void StopTimer()
    {
        _isTimerActive = false;
    }

    #endregion
    
    private void ResetAllTanks()
    {
        foreach (var tank in _tanks)
        {
            tank.Reset();
        }
    }

    private void EnableTankControl()
    {
        foreach (var tank in _tanks)
        {
            tank.EnableControl();
        }
    }

    private void DisableTankControl()
    {
        foreach (var tank in _tanks)
        {
            tank.DisableControl();
        }
    }
}
