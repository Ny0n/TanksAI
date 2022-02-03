using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    [HideInInspector] public TeamSO team;
    [HideInInspector] public GameObject tankInstance;         // A reference to the instance of the tank when it is created.
    [HideInInspector] public int playerNumber;

    private TankMovement m_Movement;                        // Reference to tank's movement script, used to disable and enable control.
    private TankShooting m_Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.

    private TankStateManager _stateManager;
    
    public TankManager(TeamSO team, int playerNumber)
    {
        this.team = team;
        this.playerNumber = playerNumber;
    }
    
    public void Setup()
    {
        // Get and Set references to the components.
        tankInstance.GetComponent<TankData>().Team = team;
        tankInstance.GetComponent<TankData>().Manager = this;
        m_Movement = tankInstance.GetComponent<TankMovement>();
        m_Shooting = tankInstance.GetComponent<TankShooting>();
        m_CanvasGameObject = tankInstance.GetComponentInChildren<Canvas>().gameObject;
        _stateManager = tankInstance.GetComponent<TankStateManager>();

        // Set the player numbers to be consistent across the scripts.
        m_Movement.m_PlayerNumber = playerNumber;
        m_Shooting.m_PlayerNumber = playerNumber;

        // Get all of the renderers of the tank
        MeshRenderer[] renderers = tankInstance.GetComponentsInChildren<MeshRenderer>();
        foreach (var m in renderers)
        {
            // and set their material color to the color specific to this tank
            m.material.color = team.Color;
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
        //_stateManager.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }

    // Used at the start of each round to put the tank into it's default state.
    public void Reset()
    {
        tankInstance.transform.position = team.SpawnPoint.position;
        tankInstance.transform.rotation = team.SpawnPoint.rotation;

        tankInstance.SetActive(false);
        tankInstance.SetActive(true);
    }
}
