using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "SearchPathSystem/AStar")]
public class FindPathSystemAStar : SearchPathSystem
{

    public override async Task<List<Node>> FindShortestPath(Vector3 startPos, Vector3 targetPos)
    {
        NodeGridVariable GridInstance = Instantiate(NodeGridVariable);
        
        Node startNode = GridInstance.NodeFromWorldPosition(startPos);
        Node goalNode = GridInstance.NodeFromWorldPosition(targetPos);

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
                // Debug.Log("Do while");
                Node currentNode = FindNodeLowestFCost(openedNode);
                closedNode.Add(currentNode);
                openedNode.Remove(currentNode);

                if (currentNode == goalNode)
                {
                    return RetracePath(startNode, goalNode);
                }

                foreach (var neighbour in GridInstance.GetNeighbour(currentNode))
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
        int yAxis =Mathf.Abs(start.YIndex - goal.YIndex);
        int xAxis =Mathf.Abs(start.XIndex - goal.XIndex);

        if ( xAxis > yAxis)
            return 14 * yAxis + 10 * (xAxis - yAxis);
        return 14 * xAxis + 10 * (yAxis - xAxis);
    }
}
