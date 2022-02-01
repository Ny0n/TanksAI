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
     
     //using for smooth weith
     [HideInInspector] public int MovementPenalty;
     
     public NodeGrid(bool walkable, Vector3 nodePosition, int movementPenalty, int X, int Y)
     {
          Walkable = walkable;
          NodePosition = nodePosition;
          XIndex = X;
          YIndex = Y;
          MovementPenalty = movementPenalty;
          NodeID = $"{X}{Y}";
     }

     public NodeGrid()
     {
     }
}
