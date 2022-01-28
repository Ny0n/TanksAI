using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "SearchPathSystem/Dijkstra")]
public class SPS_Dijkstra : SearchPathSystem
{
    
    public override async Task<List<Vector3>> FindShortestPath(Vector3 startPos, Vector3 targetPos, GridVariable gridVariable)
    {
        GridVariable gridInstance = Instantiate(gridVariable);
        
        NodeGrid startNodeGrid = gridInstance.NodeFromWorldPosition(startPos);
        NodeGrid goalNodeGrid = gridInstance.NodeFromWorldPosition(targetPos);

        if (!goalNodeGrid.Walkable)
        {
            return null;
        }

        List<NodeGrid> openedNode = new List<NodeGrid>();
        List<NodeGrid> closedNode = new List<NodeGrid>();

        startNodeGrid.GCost = 0;
        openedNode.Add(startNodeGrid);
        return await Task.Run(() =>
        {
            while (openedNode.Count > 0)
            {
                //can use it in dijkstra because Hcost will always be 0 so it return the Gcost witch is used here
                NodeGrid currentNodeGrid = FindNodeLowestFCost(openedNode);
                
                closedNode.Add(currentNodeGrid);
                openedNode.Remove(currentNodeGrid);

                if (currentNodeGrid == goalNodeGrid)
                {
                    return RetracePath(startNodeGrid, goalNodeGrid);
                }

                foreach (var neighbour in gridInstance.GetNeighbour(currentNodeGrid))
                {
                    if (!neighbour.Walkable || closedNode.Contains(neighbour))
                        continue;

                    //can use the same system as AStar to return a cost if horizontal neigh or not
                    //No need in this project but we can add a special cost on node to add with DistBtwNode
                    int newNeighbourFCost = currentNodeGrid.GCost + DistBtwNode(currentNodeGrid, neighbour);

                    if (newNeighbourFCost < neighbour.GCost || !openedNode.Contains(neighbour))
                    {
                        neighbour.GCost = newNeighbourFCost;

                        neighbour.parentNodeGrid = currentNodeGrid;

                        if (!openedNode.Contains(neighbour))
                            openedNode.Add(neighbour);
                    }
                }
            }

            return null;
        });
    }
}
