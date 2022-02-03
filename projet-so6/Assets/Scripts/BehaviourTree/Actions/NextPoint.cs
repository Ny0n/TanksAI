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
        _current = blackboard.GetValue<int>("currentPoint");
        _waypoints = blackboard.GetValue<Vector3ListSO>(waypointsString);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (_current == _waypoints.Value.Count - 1)
        {
            _current = 0;
            return State.Success;
        }

        _current++;
        return State.Success;
    }
}
