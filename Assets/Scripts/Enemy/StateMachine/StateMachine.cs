using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StateMachine : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnStateChanged))]
    public BaseState activeState;

    public void Initialise()
    {
        ChangeState(GetComponent<PatrolState>());

    }

    void Start()
    {
        
    }

    void Update()
    {
        if (activeState != null)
        {
            activeState.Perform();
        }
    }

    
    public void ChangeState(BaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }

        activeState = newState;
        if (activeState != null)
        {
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }

    void OnStateChanged(BaseState oldValue, BaseState newValue)
    {
        if(isClient) activeState = newValue;
    }
}