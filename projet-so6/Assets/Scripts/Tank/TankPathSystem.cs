using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPathSystem : MonoBehaviour
{
    [SerializeField] private SearchPathSystem SearchPathSystem;
    [SerializeField] private GridVariable grid;
    
    [SerializeField] private AnimationCurve TurnRateCurve;
    [SerializeField] private AnimationCurve MoveRateCurve;

    private List<NodeGrid> MyPath;
    private Vector3 nextDestination;

    private TankMovement _tankMovement;

    private Vector3 targetPos;

    private RaycastHit m_HitInfo;

    // Start is called before the first frame update
    void Awake()
    {
        MyPath = new List<NodeGrid>();
        _tankMovement = GetComponent<TankMovement>();
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
                PathTank(m_HitInfo.point);
            }
        }

        //total mess juste to give me an idea
        if (MyPath.Count > 0)
        {
            nextDestination = MyPath[0].NodePosition;
            float angle = Vector3.SignedAngle(transform.forward, nextDestination - transform.position, Vector3.one);
            _tankMovement.TurnInputValue = TurnRateCurve.Evaluate(angle);
            _tankMovement.MovementInputValue = MoveRateCurve.Evaluate(angle);
            
            if (Vector3.SqrMagnitude(nextDestination - transform.position) < 0.1)
            {
                MyPath.RemoveAt(0);
            }
            
            Debug.DrawRay(transform.position, transform.forward, Color.red);
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
                
                Gizmos.DrawCube(Node.NodePosition, Vector3.one * grid.NodeRadius * 2);
            }
        }
        
        Gizmos.color = Color.green;
        NodeGrid targetNodeGrid = grid.NodeFromWorldPosition(targetPos);
        Gizmos.DrawCube(targetNodeGrid.NodePosition, new Vector3(grid.NodeRadius * 2, 4, grid.NodeRadius * 2));
    }
}
