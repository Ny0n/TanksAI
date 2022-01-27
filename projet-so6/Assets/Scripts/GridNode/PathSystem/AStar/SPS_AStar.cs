using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "SearchPathSystem/AStar")]
public class SPS_AStar : SearchPathSystem
{

    public override async Task<List<Node>> FindShortestPath(Vector3 startPos, Vector3 targetPos, NodeGridVariable nodeGridVariable)
    {
        NodeGridVariable gridInstance = Instantiate(nodeGridVariable);
        
        Node startNode = gridInstance.NodeFromWorldPosition(startPos);
        Node goalNode = gridInstance.NodeFromWorldPosition(targetPos);

        if (!goalNode.Walkable)
        {
            return null;
        }

        List<Node> openedNode = new List<Node>();
        List<Node> closedNode = new List<Node>();
        
        openedNode.Add(startNode);
        return await Task.Run(() =>
        {
            while (openedNode.Count > 0)
            {
                Node currentNode = FindNodeLowestFCost(openedNode);
                closedNode.Add(currentNode);
                openedNode.Remove(currentNode);

                if (currentNode == goalNode)
                {
                    return RetracePath(startNode, goalNode);
                }

                foreach (var neighbour in gridInstance.GetNeighbour(currentNode))
                {
                    if (!neighbour.Walkable || closedNode.Contains(neighbour))
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

            return null;
        });
    }

}
