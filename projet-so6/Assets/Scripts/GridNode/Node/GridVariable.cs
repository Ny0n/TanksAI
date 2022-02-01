using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SerializeList
{
    public List<NodeGrid> NodeList;
    public NodeGrid this[int Y] => NodeList[Y];

    public SerializeList(int Y)
    {
        NodeList = new List<NodeGrid>(Y);
    }
}


[CreateAssetMenu (menuName = "Variable/NodeGrid")]
public class GridVariable : ScriptableObject
{
    public List<SerializeList> GridNew;
    
    
    public NodeGrid this[int x, int y]
    {
        get => GridNew[x][y];
    }

    public Vector2 GridSize;

    public float NodeRadius;
    
    
    [HideInInspector] public int NodeNumberX;
    [HideInInspector] public int NodeNumberY;
    

    public List<NodeGrid> GetNeighbour(NodeGrid nodeGrid)
    {
        List<NodeGrid> neighbours = new List<NodeGrid>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int neighX = nodeGrid.XIndex + i;
                int neighY = nodeGrid.YIndex + j;
                
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
    
    public NodeGrid NodeFromWorldPosition(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + GridSize.x/2) / GridSize.x;
        float percentY = (worldPosition.z + GridSize.y/2) / GridSize.y;
        
        
        //this or use Mathf.Clamp01(percentX/Y) and always return a node
        if (percentX > 1 || percentX < 0 || percentY > 1 || percentY < 0)
            return null;

        int XgridIndex = Mathf.RoundToInt(percentX * (NodeNumberX - 1));
        int YgridIndex = Mathf.RoundToInt(percentY * (NodeNumberY - 1));
        return GridNew[XgridIndex][YgridIndex];
    }
}
