using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public LayerMask UnwalkableLayer;

    [SerializeField] private NodeGridVariable NodeGridVariable;
    
    public float _nodeDiameter;

    // Start is called before the first frame update
    void Start()
    {
        _nodeDiameter = NodeGridVariable.NodeRadius * 2;
        NodeGridVariable.NodeNumberX = Mathf.RoundToInt(NodeGridVariable.GridSize.x / _nodeDiameter);
        NodeGridVariable.NodeNumberY = Mathf.RoundToInt(NodeGridVariable.GridSize.y / _nodeDiameter);
        StartCoroutine(CreateGrid());
    }
    
    IEnumerator CreateGrid()
    {
        NodeGridVariable.GridNew = new List<SerializeList>(NodeGridVariable.NodeNumberX);

        Vector3 worldBottomLeft = transform.position - Vector3.right * NodeGridVariable.GridSize.x / 2 - Vector3.forward * NodeGridVariable.GridSize.y / 2;
        
        for (int i = 0; i < NodeGridVariable.NodeNumberX; i++)
        {
            for (int j = 0; j < NodeGridVariable.NodeNumberY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * _nodeDiameter + NodeGridVariable.NodeRadius) 
                                                     + Vector3.forward * (j * _nodeDiameter + NodeGridVariable.NodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, NodeGridVariable.NodeRadius, UnwalkableLayer));

                Debug.Log(NodeGridVariable.GridNew.Count);
                if (NodeGridVariable.GridNew.Count <= i)
                {
                    Debug.Log("add list");
                    NodeGridVariable.GridNew.Add(new SerializeList(NodeGridVariable.NodeNumberY));
                }
                NodeGridVariable.GridNew[i].NodeList.Add(new Node(walkable, worldPoint, i, j));
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(NodeGridVariable.GridSize.x,1,NodeGridVariable.GridSize.y));

        if (NodeGridVariable.GridNew != null)
        {
            foreach (var nodeList in NodeGridVariable.GridNew)
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
