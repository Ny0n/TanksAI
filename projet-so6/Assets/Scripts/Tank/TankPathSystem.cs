using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPathSystem : MonoBehaviour
{
    [SerializeField] private SearchPathSystem SearchPathSystem;
    [SerializeField] private NodeGridVariable NodeGrid;

    private List<Node> MyPath;

    private Vector3 targetPos;

    private RaycastHit m_HitInfo;

    // Start is called before the first frame update
    void Start()
    {
        MyPath = new List<Node>();
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
    }

    async void PathTank(Vector3 target)
    {
        MyPath = await SearchPathSystem.FindShortestPath(transform.position, target, NodeGrid);
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
                
                Gizmos.DrawCube(Node.NodePosition, Vector3.one * NodeGrid.NodeRadius * 2);
            }
        }
        
        Gizmos.color = Color.green;
        Node targetNode = NodeGrid.NodeFromWorldPosition(targetPos);
        Gizmos.DrawCube(targetNode.NodePosition, new Vector3(NodeGrid.NodeRadius * 2, 4, NodeGrid.NodeRadius * 2));
    }
}
