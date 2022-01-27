using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "SearchPathSystem/Dijkstra")]
public class SPS_Dijkstra : SearchPathSystem
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

        startNode.GCost = 0;
        openedNode.Add(startNode);
        return await Task.Run(() =>
        {
            while (openedNode.Count > 0)
            {
                //can use it in dijkstra because Hcost will always be 0 so it return the Gcost witch is used here
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

                    //can use the same system as AStar to return a cost if horizontal neigh or not
                    //No need in this project but we can add a special cost on node to add with DistBtwNode
                    int newNeighbourFCost = currentNode.GCost + DistBtwNode(currentNode, neighbour);

                    if (newNeighbourFCost < neighbour.GCost || !openedNode.Contains(neighbour))
                    {
                        neighbour.GCost = newNeighbourFCost;

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
