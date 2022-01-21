using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPathAStar : MonoBehaviour
{
    private NodeGrid _nodeGrid;

    private void Awake()
    {
        _nodeGrid = GetComponent<NodeGrid>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchPath(Vector3 startPos, Vector3 goalPos)
    {
        Node startNode = _nodeGrid.NodeFromWorldPosition(startPos);
        Node goalNode = _nodeGrid.NodeFromWorldPosition(goalPos);

        List<Node> openedNode = new List<Node>();
        List<Node> closedNode = new List<Node>();
        
        openedNode.Add(startNode);

        while (openedNode.Count > 0)
        {
            Node currentNode = FindNodeLowestFCost(openedNode);
            closedNode.Add(currentNode);
            openedNode.Remove(currentNode);

            if (currentNode == goalNode)
                return;

            foreach (var nodeIndex in currentNode.NeighbourIndex)
            {
                Node neighbour = _nodeGrid.grid[nodeIndex.X, nodeIndex.Y];
                if(!neighbour.Walkable || closedNode.Contains(neighbour))
                {
                    continue;
                }


                int newNeighbourFCost = currentNode.GCost + DistBtwNode(currentNode, neighbour);
                
                if (newNeighbourFCost < neighbour.GCost || !openedNode.Contains(neighbour))
                {
                    neighbour.GCost = newNeighbourFCost;
                    neighbour.HCost = DistBtwNode(neighbour, goalNode);
                    
                    neighbour.parentNode = currentNode;

                    if (!openedNode.Contains(neighbour))
                        openedNode.Add(neighbour);
                }
            }
        }
    }

    public List<Node> RetracePath(Node startNode, Node targetNode)
    {
        return null;
    }

    //not optimized
    public Node FindNodeLowestFCost(List<Node> NodesList)
    {
        Node nodeToReturn = NodesList[0];

        foreach (var node in NodesList)
        {
            if (nodeToReturn.FCost > node.FCost || node.FCost == nodeToReturn.FCost && node.HCost < nodeToReturn.HCost)
            {
                nodeToReturn = node;
            }
        }

        return nodeToReturn;
    }

    public int DistBtwNode(Node start, Node goal)
    {
        int yAxis =Mathf.RoundToInt(Mathf.Abs(start.NodePosition.z - goal.NodePosition.z));
        int xAxis =Mathf.RoundToInt(Mathf.Abs(start.NodePosition.x - goal.NodePosition.x));

        if (xAxis > yAxis)
            return 14 * yAxis + 10 * (xAxis - yAxis);
        return 14 * xAxis + 10 * (yAxis - xAxis);
    }
}
