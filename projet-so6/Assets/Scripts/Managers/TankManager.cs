using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    private TeamSO _team;
    private int _playerNumber;
    
    [HideInInspector] public string m_ColoredPlayerText;    // A string that represents the player with their number colored to match their tank.
    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.

    private TankMovement m_Movement;                        // Reference to tank's movement script, used to disable and enable control.
    private TankShooting m_Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.

    private TankStateManager _stateManager;
    public TankManager(TeamSO team, int playerNumber)
    {
        _team = team;
        _playerNumber = playerNumber;
    }
    
    public void Setup()
    {
        // Get and Set references to the components.
        m_Instance.GetComponent<TankData>().Team = _team;
        m_Instance.GetComponent<TankData>().Manager = this;
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        _stateManager = m_Instance.GetComponent<TankStateManager>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        // Set the player numbers to be consistent across the scripts.
        m_Movement.m_PlayerNumber = _playerNumber;
        m_Shooting.m_PlayerNumber = _playerNumber;

        // Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(_team.Color) + ">PLAYER " + _playerNumber + "</color>";

        // Get all of the renderers of the tank
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();
        foreach (var m in renderers)
        {
            // and set their material color to the color specific to this tank
            m.material.color = _team.Color;
        }
    }

    // Used during the phases of the game where the player shouldn't be able to control their tank.
    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;
        _stateManager.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }

    // Used during the phases of the game where the player should be able to control their tank.
    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;
        _stateManager.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }

    // Used at the start of each round to put the tank into it's default state.
    public void Reset()
    {
        m_Instance.transform.position = _team.SpawnPoint.position;
        m_Instance.transform.rotation = _team.SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
