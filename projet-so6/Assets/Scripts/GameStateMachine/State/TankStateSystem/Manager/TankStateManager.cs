using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class TankStateManager : StateManager
{
    [SerializeField] private AnimationCurve turnRateCurve;
    public AnimationCurve TurnRateCurve => turnRateCurve;
    
    [SerializeField] private AnimationCurve moveRateCurve;
    public AnimationCurve MoveRateCurve => moveRateCurve;
    
    
    private Vector3 _nextDestination;
    public Vector3 NextDestination => _nextDestination;

    private TankMovement _tankMovement;
    public TankMovement TankMovement => _tankMovement;
    
    
    private TankShooting _tankShooting;
    public TankShooting TankShooting => _tankShooting;
    
    
    private TankPathSystem _tankPathSystem;
    public TankPathSystem TankPathSystem => _tankPathSystem;


    private RaycastHit _HitInfo;

    private void Awake()
    {
        _tankMovement = GetComponent<TankMovement>();
        _tankShooting = GetComponent<TankShooting>();
        _tankPathSystem = GetComponent<TankPathSystem>();
    }

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out _HitInfo))
            {
                _tankPathSystem.SearchTargetPath(_HitInfo.point);
            }
        }
    }
}
