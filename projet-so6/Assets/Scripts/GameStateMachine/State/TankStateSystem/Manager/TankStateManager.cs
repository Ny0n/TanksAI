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
    [SerializeField] private bool Player;
    [SerializeField] private AnimationCurve turnRateCurve;
    public AnimationCurve TurnRateCurve => turnRateCurve;
    
    [SerializeField] private AnimationCurve moveRateCurve;
    public AnimationCurve MoveRateCurve => moveRateCurve;

    [HideInInspector] public bool AIShot;
    
    private Vector3 _nextDestination;
    public Vector3 NextDestination => _nextDestination;

    private TankMovement _tankMovement;
    public TankMovement TankMovement => _tankMovement;
    
    
    private TankShooting _tankShooting;
    public TankShooting TankShooting => _tankShooting;
    
    
    private TankPathSystem _tankPathSystem;
    public TankPathSystem TankPathSystem => _tankPathSystem;
    
    
    private AITankAvoidance _aiTankAvoidance;
    public AITankAvoidance AITankAvoidance => _aiTankAvoidance;


    private RaycastHit _HitInfo;

    private void Awake()
    {
        _tankMovement = GetComponent<TankMovement>();
        _tankShooting = GetComponent<TankShooting>();
        if(!Player)
        {
            _tankPathSystem = GetComponent<TankPathSystem>();
            _aiTankAvoidance = GetComponent<AITankAvoidance>();
        }
    }

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        if(Player)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out _HitInfo))
            {
                _tankPathSystem.SearchTargetPath(_HitInfo.point);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            AIShot = true;
        }
    }
}
