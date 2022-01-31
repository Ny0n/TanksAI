using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreation : MonoBehaviour
{
    public LayerMask UnwalkableLayer;

    [SerializeField] private GridVariable gridVariable;

    public int BlurPenaltyValue;
    
    private float _nodeDiameter;

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
        Debug.Log("Start create grid");
        gridVariable.GridNew = new List<SerializeList>(gridVariable.NodeNumberX);

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridVariable.GridSize.x / 2 - Vector3.forward * gridVariable.GridSize.y / 2;
        
        for (int i = 0; i < gridVariable.NodeNumberX; i++)
        {
            for (int j = 0; j < gridVariable.NodeNumberY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * _nodeDiameter + gridVariable.NodeRadius) 
                                                     + Vector3.forward * (j * _nodeDiameter + gridVariable.NodeRadius);
                int movementPenalty = 0;
                
                bool walkable = !(Physics.CheckSphere(worldPoint, gridVariable.NodeRadius, UnwalkableLayer));

                if (!walkable)
                {
                    movementPenalty = 10;
                }
                if (gridVariable.GridNew.Count <= i)
                {
                    gridVariable.GridNew.Add(new SerializeList(gridVariable.NodeNumberY));
                }
                
                
                gridVariable.GridNew[i].NodeList.Add(new NodeGrid(walkable, worldPoint, movementPenalty, i, j));
            }
            yield return null;
        }

        Debug.Log("End create grid");
        StartCoroutine(BlurPenaltyMap(BlurPenaltyValue));
    }
    
    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;
    
    //it's just a copy past
    //wanted to see the effect of the tuto, don't take it into consideration for the notation
    IEnumerator BlurPenaltyMap(int blurSize)
    {
        Debug.Log("Start blur");
        int gridSizeX = gridVariable.NodeNumberX;
        int gridSizeY = gridVariable.NodeNumberY;
        
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX,gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX,gridSizeY];

        for (int y = 0; y < gridSizeY; y++) {
            for (int x = -kernelExtents; x <= kernelExtents; x++) {
                int sampleX = Mathf.Clamp (x, 0, kernelExtents);
                penaltiesHorizontalPass [0, y] += gridVariable[sampleX, y].MovementPenalty;
            }
            
            for (int x = 1; x < gridSizeX; x++) {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX-1);

                penaltiesHorizontalPass [x, y] = penaltiesHorizontalPass [x - 1, y] - gridVariable[removeIndex, y].MovementPenalty + gridVariable[addIndex, y].MovementPenalty;
            }

            yield return null;
        }
			
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = -kernelExtents; y <= kernelExtents; y++) {
                int sampleY = Mathf.Clamp (y, 0, kernelExtents);
                penaltiesVerticalPass [x, 0] += penaltiesHorizontalPass [x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, 0] / (kernelSize * kernelSize));
            gridVariable[x, 0].MovementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++) {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY-1);

                penaltiesVerticalPass [x, y] = penaltiesVerticalPass [x, y-1] - penaltiesHorizontalPass [x,removeIndex] + penaltiesHorizontalPass [x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, y] / (kernelSize * kernelSize));
                gridVariable[x, y].MovementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax) {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin) {
                    penaltyMin = blurredPenalty;
                }
            }
            yield return null;
        }

        Debug.Log("End blur");
    }

    public bool bDrawGizmo;
    private void OnDrawGizmos()
    {
        if (!bDrawGizmo)
            return;
        
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
                    else
                    {
                        Gizmos.color = Color.Lerp (Color.white, Color.black, Mathf.InverseLerp (0, 10, node.MovementPenalty));
                        Gizmos.DrawCube(node.NodePosition, new Vector3(_nodeDiameter, 0.5f, _nodeDiameter));
                    }
                }
            }
        }
    }
}
