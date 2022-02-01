using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SearchPathSystem : ScriptableObject
{
    //used for Disjktra and AStar
    public List<NodeGrid> Path = new List<NodeGrid>();

    public abstract Task<List<Vector3>> FindShortestPath(Vector3 startPos, Vector3 targetPos, GridVariable gridVariable);
    
    
    public async Task<List<Vector3>> RetracePath(NodeGridWeighted startNodeGrid, NodeGridWeighted targetNodeGrid)
    {
        List<Vector3> NodePath = new List<Vector3>();
        NodeGridWeighted currentNode = targetNodeGrid;

        return await Task.Run((() => {int destination = 0;

            while (currentNode != startNodeGrid)
            {
                int newDestination = Mathf.Abs(currentNode.GCost - currentNode.ParentNodeGrid.GCost);
                if (destination != newDestination)
                {
                    NodePath.Add(currentNode.NodePosition);
                    destination = newDestination;
                }
                currentNode = currentNode.ParentNodeGrid;
            }
            NodePath.Reverse();
        
            return NodePath; 
        }));
        
    }
    
    //not optimized
    protected NodeGridWeighted FindNodeLowestFCost(Dictionary<string, NodeGridWeighted> NodesList)
    {
        NodeGridWeighted nodeGridToReturn = NodesList.First().Value;

        foreach (var nodeKeyValue in NodesList)
        {
            NodeGridWeighted node = nodeKeyValue.Value;
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
