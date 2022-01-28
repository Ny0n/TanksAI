using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SearchPathSystem : ScriptableObject
{
    //used for Disjktra and AStar
    public List<NodeGrid> Path = new List<NodeGrid>();

    public abstract Task<List<NodeGrid>> FindShortestPath(Vector3 startPos, Vector3 targetPos, GridVariable gridVariable);
    
    
    public async Task<List<NodeGrid>> RetracePath(NodeGrid startNodeGrid, NodeGrid targetNodeGrid)
    {
        List<NodeGrid> NodePath = new List<NodeGrid>();
        
        if (targetNodeGrid != startNodeGrid)
        {
            NodePath.AddRange(await RetracePath(startNodeGrid, targetNodeGrid.parentNodeGrid));
        }
        NodePath.Add(targetNodeGrid);
        
        return NodePath;
    }
    
    
    //not optimized
    protected NodeGrid FindNodeLowestFCost(List<NodeGrid> NodesList)
    {
        NodeGrid nodeGridToReturn = NodesList[0];

        foreach (var node in NodesList)
        {
            if (nodeGridToReturn.FCost > node.FCost || node.FCost == nodeGridToReturn.FCost && node.HCost < nodeGridToReturn.HCost)
            {
                nodeGridToReturn = node;
            }
        }

        return nodeGridToReturn;
    }

    protected int DistBtwNode(NodeGrid start, NodeGrid goal)
    {
        int yAxis = Mathf.Abs(start.YIndex - goal.YIndex);
        int xAxis = Mathf.Abs(start.XIndex - goal.XIndex);

        if ( xAxis > yAxis)
            return 14 * yAxis + 10 * (xAxis - yAxis);
        return 14 * xAxis + 10 * (yAxis - xAxis);
    }
}
