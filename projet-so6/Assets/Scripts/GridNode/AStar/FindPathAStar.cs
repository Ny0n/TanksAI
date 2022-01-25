using System.Collections.Generic;
using UnityEngine;

public class FindPathAStar : MonoBehaviour
{
    private NodeGrid _nodeGrid;

    public Transform startPos;
    public Transform targetPos;

    public List<Node> Path = new List<Node>();

    private void Awake()
    {
        _nodeGrid = GetComponent<NodeGrid>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Search");
            SearchPath(startPos.position, targetPos.position);
            Debug.Log(Path.Count);
        }
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
            // Debug.Log("Do while");
            Node currentNode = FindNodeLowestFCost(openedNode);
            closedNode.Add(currentNode);
            openedNode.Remove(currentNode);

            if (currentNode == goalNode)
            {
                Path = RetracePath(startNode, goalNode);
                return;
            }

            foreach (var neighbour in _nodeGrid.GetNeighbour(currentNode))
            {
                if(!neighbour.Walkable || closedNode.Contains(neighbour))
                    continue;
                
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

        Debug.LogWarning("Path not found");
    }

    public List<Node> RetracePath(Node startNode, Node targetNode)
    {
        List<Node> NodePath = new List<Node>();
        if (targetNode != startNode)
        {
            NodePath.AddRange(RetracePath(startNode, targetNode.parentNode));
        }
        NodePath.Add(targetNode);
        
        return NodePath;
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
        int yAxis =Mathf.Abs(start.nodeIndex.Y - goal.nodeIndex.Y);
        int xAxis =Mathf.Abs(start.nodeIndex.X - goal.nodeIndex.X);

        if ( xAxis > yAxis)
            return 14 * yAxis + 10 * (xAxis - yAxis);
        return 14 * xAxis + 10 * (yAxis - xAxis);
    }

    private void OnDrawGizmos()
    {
        if(_nodeGrid != null)
        {
            if (_nodeGrid.grid != null)
            {
                Gizmos.color = Color.yellow;
                Node targetN = _nodeGrid.NodeFromWorldPosition(targetPos.position);
                if(targetN != null)
                    Gizmos.DrawCube(targetN.NodePosition, new Vector3(_nodeGrid._nodeDiameter, 2, _nodeGrid._nodeDiameter));

                Gizmos.color = Color.green;
                Node startN = _nodeGrid.NodeFromWorldPosition(startPos.position);
                
                if(startN != null)
                    Gizmos.DrawCube(startN.NodePosition, new Vector3(_nodeGrid._nodeDiameter, 2, _nodeGrid._nodeDiameter));
            } 
        }
        
        if (Path.Count > 0)
        {
            Gizmos.color = Color.blue;
            foreach (var node in Path)
            {
                Gizmos.DrawCube(node.NodePosition, new Vector3(_nodeGrid._nodeDiameter, 2,_nodeGrid._nodeDiameter));
            }
        }
    }
}
