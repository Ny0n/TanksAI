using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TankPathSystem : MonoBehaviour
{
    [SerializeField] private SearchPathSystem searchPathSystem;
    [SerializeField] private GridVariable grid;
    
    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    private List<Vector3> _myPath;
    public List<Vector3> MyPath  => _myPath;
    
    private Vector3 _targetPos;


    // Start is called before the first frame update
    void Awake()
    {
        _myPath = new List<Vector3>();
    }

    private void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    
    //todo find better solution than removing the path system to switch on navmesh
    //Use this methode to move the tank toward a direction
    public void SearchTargetPath(Vector3 targetPosition)
    {
        _targetPos = targetPosition;
        if (searchPathSystem != null)
        {
            PathTank();
        }
        else
        {
            PathTankAgent();
        }
    }
    
    async void PathTank()
    {
        _myPath = await searchPathSystem.FindShortestPath(transform.position, _targetPos, grid);
    }
    
    void PathTankAgent()
    {
        NavMeshPath pathMesh = new NavMeshPath();
        agent.nextPosition = transform.position; //reset the agent next pos 
        agent.CalculatePath(_targetPos, pathMesh);
        
        _myPath = pathMesh.corners.ToList();
    }
    private void OnDrawGizmos()
    {
        if (_myPath != null)
        {
            foreach (var Node in _myPath)
            {
                if (Node == null)
                    return;
                
                Gizmos.color = Color.cyan;
                
                Gizmos.DrawCube(Node, Vector3.one * grid.NodeRadius * 2);
            }
        }
    }
}
