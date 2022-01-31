using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class NodeGrid
{
     public bool Walkable;
     public Vector3 NodePosition;

     public int XIndex;
     public int YIndex;

     [HideInInspector] public string NodeID;
     
     public NodeGrid(bool walkable, Vector3 nodePosition, int X, int Y)
     {
          Walkable = walkable;
          NodePosition = nodePosition;
          XIndex = X;
          YIndex = Y;
          NodeID = $"{X}{Y}";
     }

     public NodeGrid()
     {
     }
}
