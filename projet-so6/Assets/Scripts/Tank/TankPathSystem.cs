using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPathSystem : MonoBehaviour
{
    [SerializeField] private SearchPathSystem SearchPathSystem;
    [SerializeField] private NodeGridVariable NodeGrid;

    private List<Node> MyPath;

    private Transform targetPos;


    // Start is called before the first frame update
    void Start()
    {
        MyPath = new List<Node>();
        targetPos = GameObject.FindGameObjectWithTag("Target").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            Debug.Log(NodeGrid.GridNew.Count);
            if (NodeGrid.GridNew != null)
                PathTank();
        }
    }

    async void PathTank()
    {
        MyPath = await SearchPathSystem.FindShortestPath(transform.position, targetPos.position);
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
    }
}
