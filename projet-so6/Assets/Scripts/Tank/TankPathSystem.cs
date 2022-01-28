using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TankPathSystem : MonoBehaviour
{
    [SerializeField] private SearchPathSystem SearchPathSystem;
    [SerializeField] private GridVariable grid;
    
    [SerializeField] private AnimationCurve TurnRateCurve;
    [SerializeField] private AnimationCurve MoveRateCurve;
    
    
    [SerializeField] private NavMeshAgent Agent;

    private List<Vector3> MyPath;
    private Vector3 nextDestination;

    private TankMovement _tankMovement;

    private Vector3 targetPos;

    private RaycastHit m_HitInfo;

    // Start is called before the first frame update
    void Awake()
    {
        MyPath = new List<Vector3>();
        _tankMovement = GetComponent<TankMovement>();
    }

    private void Start()
    {
        Agent.updatePosition = false;
        Agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
            {
                targetPos = m_HitInfo.point;
                
                //todo find better solution than removing the path system to switch on navmesh
                if(SearchPathSystem != null)
                {
                    PathTank(targetPos);
                }
                else
                {
                    NavMeshPath pathMesh = new NavMeshPath();

                    Agent.nextPosition = transform.position; //reset the agent next pos 
                    Agent.CalculatePath(targetPos, pathMesh);

                    MyPath = pathMesh.corners.ToList();
                }
            }
        }

        //total mess juste to give me an idea
        if (MyPath.Count > 0)
        {
            nextDestination = MyPath[0];
            float angle = Vector3.SignedAngle(transform.forward, nextDestination - transform.position, Vector3.one);
            _tankMovement.TurnInputValue = TurnRateCurve.Evaluate(angle);
            _tankMovement.MovementInputValue = MoveRateCurve.Evaluate(angle);
            
            if (Vector3.SqrMagnitude(nextDestination - transform.position) < 0.1)
            {
                MyPath.RemoveAt(0);
            }
            
            Debug.DrawRay(transform.position, nextDestination - transform.position, Color.blue);
        }
        else
        {
            _tankMovement.TurnInputValue = 0;
            _tankMovement.MovementInputValue = 0;
        }
        
    }

    async void PathTank(Vector3 target)
    {
        MyPath = await SearchPathSystem.FindShortestPath(transform.position, target, grid);
    }

    private void OnDrawGizmos()
    {
        if (MyPath != null)
        {
            foreach (var Node in MyPath)
            {
                if (Node == null)
                    return;
                
                Gizmos.color = Color.cyan;
                
                Gizmos.DrawCube(Node, Vector3.one * grid.NodeRadius * 2);
            }
        }
    }
}
