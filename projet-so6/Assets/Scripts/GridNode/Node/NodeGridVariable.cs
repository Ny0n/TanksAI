using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SerializeList
{
    public List<Node> NodeList;
    public Node this[int Y] => NodeList[Y];

    public SerializeList(int Y)
    {
        NodeList = new List<Node>(Y);
    }
}


[CreateAssetMenu (menuName = "Variable/NodeGrid")]
public class NodeGridVariable : ScriptableObject
{
    public List<SerializeList> GridNew;

    public Vector2 GridSize;

    public float NodeRadius;
    public int NodeNumberX;
    public int NodeNumberY;

    public List<Node> GetNeighbour(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int neighX = node.XIndex + i;
                int neighY = node.YIndex + j;
                
                if(i == 0 & j == 0)
                    continue;

                if((neighX >= 0 && neighX < NodeNumberX) && (neighY >= 0 && neighY < NodeNumberY))
                {
                    neighbours.Add(GridNew[neighX][neighY]);
                }
            }
        }
        return neighbours;
    }
    
    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + GridSize.x/2) / GridSize.x;
        float percentY = (worldPosition.z + GridSize.y/2) / GridSize.y;
        
        
        //this or use Mathf.Clamp01(percentX/Y) and always return a node
        if (percentX > 1 || percentX < 0 || percentY > 1 || percentY < 0)
            return null;

        int XgridIndex = Mathf.RoundToInt(percentX * (NodeNumberX - 1));
        int YgridIndex = Mathf.RoundToInt(percentY * (NodeNumberY - 1));
        Debug.Log(NodeNumberX);
        return GridNew[XgridIndex][YgridIndex];
    }
}
