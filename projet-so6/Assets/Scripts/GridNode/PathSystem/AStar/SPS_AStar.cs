using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "SearchPathSystem/AStar")]
public class SPS_AStar : SearchPathSystem
{

    public override async Task<List<Vector3>> FindShortestPath(Vector3 startPos, Vector3 targetPos, GridVariable gridVariable)
    {
        NodeGrid startNodeGrid = gridVariable.NodeFromWorldPosition(startPos);
        NodeGrid goalNodeGrid = gridVariable.NodeFromWorldPosition(targetPos);

        if (!goalNodeGrid.Walkable)
        {
            return new List<Vector3>();
        }

        //alternative instead of Instanciate a grid every time this function is called
        Dictionary<string, NodeGridWeighted> openedNodeDict = new Dictionary<string, NodeGridWeighted>();
        Dictionary<string, NodeGridWeighted> closedNodeDict = new Dictionary<string, NodeGridWeighted>();
        
        openedNodeDict.Add(startNodeGrid.NodeID, new NodeGridWeighted(startNodeGrid));
        
        return await Task.Run(() =>
        {
            while (openedNodeDict.Count > 0)
            {
                NodeGridWeighted currentNodeGrid = FindNodeLowestFCost(openedNodeDict);
                closedNodeDict.Add(currentNodeGrid.NodeID, currentNodeGrid);
                openedNodeDict.Remove(currentNodeGrid.NodeID);

                if (currentNodeGrid.NodeID.Equals(goalNodeGrid.NodeID))
                {
                    return RetracePath(closedNodeDict[startNodeGrid.NodeID], closedNodeDict[goalNodeGrid.NodeID]);
                }

                foreach (var neighbour in gridVariable.GetNeighbour(currentNodeGrid))
                {
                    if (!neighbour.Walkable || closedNodeDict.ContainsKey(neighbour.NodeID))
                        continue;
                    
                    NodeGridWeighted neighbourWeigh = new NodeGridWeighted(neighbour);
                    if (openedNodeDict.ContainsKey(neighbour.NodeID))
                    {
                        neighbourWeigh = openedNodeDict[neighbour.NodeID];
                    }
                    
                    int newNeighbourFCost = currentNodeGrid.GCost + DistBtwNode(currentNodeGrid, neighbour) + currentNodeGrid.MovementPenalty;

                    if (newNeighbourFCost < neighbourWeigh.GCost || !openedNodeDict.ContainsKey(neighbour.NodeID))
                    {
                        neighbourWeigh.GCost = newNeighbourFCost;
                        neighbourWeigh.HCost = DistBtwNode(neighbour, goalNodeGrid);

                        neighbourWeigh.ParentNodeGrid = currentNodeGrid;

                        if (!openedNodeDict.ContainsKey(neighbour.NodeID))
                        {
                            openedNodeDict.Add(neighbour.NodeID, neighbourWeigh);
                        }
                    }
                }
            }

            return new Task<List<Vector3>>(null);
        });
    }

}
