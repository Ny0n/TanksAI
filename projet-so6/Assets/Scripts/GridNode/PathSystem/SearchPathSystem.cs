using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SearchPathSystem : ScriptableObject
{
    //used for Disjktra and AStar
    public List<Node> Path = new List<Node>();

    public abstract Task<List<Node>> FindShortestPath(Vector3 startPos, Vector3 targetPos, NodeGridVariable nodeGridVariable);
    
    
    public async Task<List<Node>> RetracePath(Node startNode, Node targetNode)
    {
        List<Node> NodePath = new List<Node>();
        
        if (targetNode != startNode)
        {
            NodePath.AddRange(await RetracePath(startNode, targetNode.parentNode));
        }
        NodePath.Add(targetNode);
        
        return NodePath;
    }
    
    
    //not optimized
    protected Node FindNodeLowestFCost(List<Node> NodesList)
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

    protected int DistBtwNode(Node start, Node goal)
    {
        int yAxis = Mathf.Abs(start.YIndex - goal.YIndex);
        int xAxis = Mathf.Abs(start.XIndex - goal.XIndex);

        if ( xAxis > yAxis)
            return 14 * yAxis + 10 * (xAxis - yAxis);
        return 14 * xAxis + 10 * (yAxis - xAxis);
    }
}
