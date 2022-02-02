using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FloatVariableSO _winPoints;
    
    public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
    public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
    public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
    public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.

    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
    
    public GameObject tankPrefab;

    [SerializeField] private TeamListSO _teamsList;
    [SerializeField] private TeamVariableSO _winningTeam;
    private List<TankManager> _tanks;

    const float k_MaxDepenetrationVelocity = float.PositiveInfinity;
    
    private void Start()
    {
        ObservableCollection<TeamSO> test = new ObservableCollection<TeamSO>();
        test.CollectionChanged += (sender, args) =>
        {
            Debug.Log("Changed");
        };
        
        Debug.Log(_teamsList.Value[0].Name);
        test.Add(_teamsList.Value[0]);
            
        // This line fixes a change to the physics engine.
        Physics.defaultMaxDepenetrationVelocity = k_MaxDepenetrationVelocity;
        
        // Create the delays so they only have to be made once.
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();
        SetCameraTargets();

        // Once the tanks have been created and the camera is using them as targets, start the game.
        StartCoroutine(GameLoop());
    }
    
    [ContextMenu("Change")]
    public void Change()
    {
        _teamsList.Value[0].Reset();
    }

    private void SpawnAllTanks()
    {
        _tanks = new List<TankManager>();
        int j = 0;
        // For all the teams & all of the players inside
        foreach (var team in _teamsList.Value)
        {
            for (int i = 1; i <= team.NbPlayers; i++)
            {
                // we create and spawn the tanks
                i = i + j;
                TankManager tm = new TankManager(team, i);
                tm.m_Instance = Instantiate(tankPrefab, team.SpawnPoint.position, team.SpawnPoint.rotation) as GameObject;
                tm.Setup();
                
                _tanks.Add(tm);
                team.Tanks.Add(tm);
            }

            j += team.NbPlayers;
        }
    }

    private void SetCameraTargets()
    {
        // Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[_tanks.Count];

        // For each of these transforms...
        for (int i = 0; i < targets.Length; i++)
        {
            // ... set it to the appropriate tank transform.
            targets[i] = _tanks[i].m_Instance.transform;
        }

        // These are the targets the camera should follow.
        m_CameraControl.m_Targets = targets;
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameStarting());
        yield return StartCoroutine(GamePlaying());
        yield return StartCoroutine(GameEnding());

        // SceneManager.LoadScene(0);
    }

    private IEnumerator GameStarting()
    {
        // As soon as the round starts reset the tanks and make sure they can't move.
        ResetAllTanks();
        DisableTankControl();

        // Snap the camera's zoom and position to something appropriate for the reset tanks.
        m_CameraControl.SetStartPositionAndSize();

        m_MessageText.text = "GAME START";

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }

    private IEnumerator GamePlaying()
    {
        // As soon as the game starts we let the players/IA control the tanks.
        EnableTankControl();

        // Clear the text from the screen.
        m_MessageText.text = string.Empty;

        while (!CheckForWinner())
        {
            // ... return on the next frame.
            yield return null;
        }
    }

    private bool CheckForWinner()
    {
        return false;
        // return _teamsList.Value.FirstOrDefault(team => team.Points >= _winPoints.Value);
    }

    private IEnumerator GameEnding()
    {
        // Stop tanks from moving.
        DisableTankControl();

        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessage();
        m_MessageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_EndWait;
    }

    private string EndMessage()
    {
        string message = string.Empty;

        message = "<color=#" + ColorUtility.ToHtmlStringRGB(_winningTeam.Value.Color) + ">" +  "Team " + _winningTeam.Value.Name + "</color>" + " wins the game!";

        return message;
    }

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
