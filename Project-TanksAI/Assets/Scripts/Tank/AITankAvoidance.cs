using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITankAvoidance : MonoBehaviour
{
    [SerializeField] private Transform[] tankStartDetection;
    [SerializeField] private float sensorLenght;
    [SerializeField] private LayerMask ignoreLayer;
    private RaycastHit _hit;

    private bool _avoidObstacle;
    public bool AvoidObstacle => _avoidObstacle;

    private LayerMask _ignoreMask;

    public LayerMask IgnoreMask => _ignoreMask;

    private float _turnCost;
    public float TurnCost => _turnCost;
    
    
    private float _angleCost;
    public float AngleCost => _angleCost;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _avoidObstacle = false;
        
        foreach (var rayTransform in tankStartDetection)
        {
            if (Physics.Raycast(rayTransform.position, rayTransform.forward, out _hit, sensorLenght, ~ignoreLayer))
            {
                _ignoreMask = _hit.transform.gameObject.layer;
                _avoidObstacle = true;
                _angleCost = Vector3.SignedAngle(rayTransform.forward, -_hit.normal, Vector3.one);
                Debug.DrawLine(rayTransform.position, _hit.point, Color.red);
                Debug.DrawRay(_hit.point, _hit.normal, Color.blue);
            }
        }
    }
}
