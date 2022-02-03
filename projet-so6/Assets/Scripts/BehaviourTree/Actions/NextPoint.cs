using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;

public class NextPoint : ActionNode
{
    private int _current;
    private Vector3ListSO _waypoints;
    [SerializeField] private string waypointsString;
    
    protected override void OnStart()
    {
        
        Debug.Log("NextPoint");
        _current = blackboard.GetValue<int>("currentPoint");
        _waypoints = blackboard.MapWaypoints;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (_current == _waypoints.Value.Count - 1)
        {
            _current = 0;
            blackboard.SetValue("currentPoint", _current);
            return State.Success;
        }

        _current++;
        blackboard.SetValue("currentPoint", _current);
        return State.Success;
    }
}
