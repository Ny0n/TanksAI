using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
     public bool Walkable;
     public Vector3 NodePosition;
     public List<(int X, int Y)> NeighbourIndex;

     public Node(bool walkable, Vector3 nodePosition)
     {
          Walkable = walkable;
          NodePosition = nodePosition;
          NeighbourIndex = new List<(int X, int Y)>();
     }
}
