using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public Transform playerPos;
    
    public LayerMask UnwalkableLayer;
    public Vector2 GridSize;
    public float NodeRadius;
    public Node[,] grid;
    
    public float _nodeDiameter;
    private int _nodeNumberX, _nodeNumberY;

    // Start is called before the first frame update
    void Start()
    {
        _nodeDiameter = NodeRadius * 2;
        _nodeNumberX = Mathf.RoundToInt(GridSize.x / _nodeDiameter);
        _nodeNumberY = Mathf.RoundToInt(GridSize.y / _nodeDiameter);
        StartCoroutine(CreateGrid());
    }
    
    IEnumerator CreateGrid()
    {
        grid = new Node[_nodeNumberX, _nodeNumberY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * GridSize.x / 2 - Vector3.forward * GridSize.y / 2;
        
        for (int i = 0; i < _nodeNumberX; i++)
        {
            for (int j = 0; j < _nodeNumberY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * _nodeDiameter + NodeRadius) 
                                                     + Vector3.forward * (j * _nodeDiameter + NodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableLayer));

                grid[i, j] = new Node(walkable, worldPoint, i, j);
            }
            yield return null;
        }
    }
    
    public List<Node> GetNeighbour(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int neighX = node.nodeIndex.X + i;
                int neighY = node.nodeIndex.Y + j;
                
                if(i == 0 & j == 0)
                    continue;

                if((neighX >= 0 && neighX < _nodeNumberX) && (neighY >= 0 && neighY < _nodeNumberY))
                {
                    neighbours.Add(grid[neighX, neighY]);
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

        int XgridIndex = Mathf.RoundToInt(percentX * (_nodeNumberX - 1));
        int YgridIndex = Mathf.RoundToInt(percentY * (_nodeNumberY - 1));
        return grid[XgridIndex, YgridIndex];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(GridSize.x,1,GridSize.y));

        if (grid != null)
        {
            foreach (var node in grid)
            {
                if (node == null)
                    continue;
                
                if (!node.Walkable)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(node.NodePosition, new Vector3(_nodeDiameter, 0.5f, _nodeDiameter));
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(node.NodePosition, new Vector3(_nodeDiameter, 0.5f, _nodeDiameter));
                }
            }
        }
    }
}
