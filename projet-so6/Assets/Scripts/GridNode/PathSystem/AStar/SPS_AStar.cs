using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "SearchPathSystem/AStar")]
public class SPS_AStar : SearchPathSystem
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
        
        openedNode.Add(startNodeGrid);
        return await Task.Run(() =>
        {
            while (openedNode.Count > 0)
            {
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

                    int newNeighbourFCost = currentNodeGrid.GCost + DistBtwNode(currentNodeGrid, neighbour);

                    if (newNeighbourFCost < neighbour.GCost || !openedNode.Contains(neighbour))
                    {
                        neighbour.GCost = newNeighbourFCost;
                        neighbour.HCost = DistBtwNode(neighbour, goalNodeGrid);

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
