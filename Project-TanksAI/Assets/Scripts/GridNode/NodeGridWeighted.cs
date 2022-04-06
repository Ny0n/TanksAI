using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGridWeighted : NodeGrid
{
    //for AStar
    public int GCost { get; set; } //Dijkstra will use this
    public int HCost { get; set; }
    public int FCost => GCost + HCost;

    public NodeGridWeighted ParentNodeGrid;

    
    public NodeGridWeighted(NodeGrid cloneNode)
    {
        NodePosition = cloneNode.NodePosition;
        XIndex = cloneNode.XIndex;
        YIndex = cloneNode.YIndex;
        NodeID = cloneNode.NodeID;
        MovementPenalty = cloneNode.MovementPenalty;
    }
}
