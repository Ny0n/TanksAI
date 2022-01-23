using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Node
{
     public bool Walkable;
     public Vector3 NodePosition;
     public List<(int X, int Y)> NeighbourIndex;

     public (int X, int Y) nodeIndex;
     
     
     //for AStar
     public int GCost { get; set; }
     public int HCost { get; set; }
     public int FCost => GCost + HCost;

     public Node parentNode;

     public Node(bool walkable, Vector3 nodePosition, int X, int Y)
     {
          Walkable = walkable;
          NodePosition = nodePosition;
          nodeIndex.X = X;
          nodeIndex.Y = Y;
          NeighbourIndex = new List<(int X, int Y)>();
     }

     protected Node()
     {
          
     }
}
