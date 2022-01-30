using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private BaseState[] firstStates;
    private BaseState[] currentStates;
    
    // Start is called before the first frame update
    protected void Start()
    {
        currentStates = new BaseState[firstStates.Length];
        for (int i = 0; i < firstStates.Length; i++)
        {
            ChangeState(firstStates[i], i);
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        for (int i = 0; i < currentStates.Length; i++)
        {
            currentStates[i].Tick(this, i);
        }
    }

    public void ChangeState(BaseState newState, int stateIndex)
    {
        if(currentStates[stateIndex] != null)
        {
            currentStates[stateIndex].OnStateExit(this);
        }
        currentStates[stateIndex] = newState;
        currentStates[stateIndex].OnStateEnter(this);
    }
}
