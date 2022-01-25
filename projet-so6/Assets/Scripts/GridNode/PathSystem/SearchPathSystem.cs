using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SearchPathSystem : ScriptableObject
{
    //used for Disjktra and AStar
    public List<Node> Path = new List<Node>();
    [SerializeField] protected NodeGridVariable NodeGridVariable;
    
    //todo make it async or coroutine (think async is the solution)
    public virtual async Task<List<Node>> FindShortestPath( Vector3 startPos, Vector3 targetPos)
    {
        return null;
    }
}
