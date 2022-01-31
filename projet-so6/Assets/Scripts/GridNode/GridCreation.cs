using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreation : MonoBehaviour
{
    public LayerMask UnwalkableLayer;

    [SerializeField] private GridVariable gridVariable;
    
    public float _nodeDiameter;

    // Start is called before the first frame update
    void Start()
    {
        _nodeDiameter = gridVariable.NodeRadius * 2;
        gridVariable.NodeNumberX = Mathf.RoundToInt(gridVariable.GridSize.x / _nodeDiameter);
        gridVariable.NodeNumberY = Mathf.RoundToInt(gridVariable.GridSize.y / _nodeDiameter);
        if (gridVariable.GridNew.Count == 0)
        {

            StartCoroutine(CreateGrid());
        }
    }
    
    IEnumerator CreateGrid()
    {
        gridVariable.GridNew = new List<SerializeList>(gridVariable.NodeNumberX);

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridVariable.GridSize.x / 2 - Vector3.forward * gridVariable.GridSize.y / 2;
        
        for (int i = 0; i < gridVariable.NodeNumberX; i++)
        {
            for (int j = 0; j < gridVariable.NodeNumberY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * _nodeDiameter + gridVariable.NodeRadius) 
                                                     + Vector3.forward * (j * _nodeDiameter + gridVariable.NodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, gridVariable.NodeRadius, UnwalkableLayer));

                Debug.Log(gridVariable.GridNew.Count);
                if (gridVariable.GridNew.Count <= i)
                {
                    gridVariable.GridNew.Add(new SerializeList(gridVariable.NodeNumberY));
                }
                gridVariable.GridNew[i].NodeList.Add(new NodeGrid(walkable, worldPoint, i, j));
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridVariable.GridSize.x,1,gridVariable.GridSize.y));

        if (gridVariable.GridNew != null)
        {
            foreach (var nodeList in gridVariable.GridNew)
            {
                foreach(var node in nodeList.NodeList){
                    if (node == null)
                        continue;

                    if (!node.Walkable)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(node.NodePosition, new Vector3(_nodeDiameter, 0.5f, _nodeDiameter));
                    }
                }
            }
        }
    }
}
