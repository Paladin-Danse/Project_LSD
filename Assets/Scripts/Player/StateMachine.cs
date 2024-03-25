using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<T> where T : IState
{
    public T currentState { get; protected set; }

    public void ChangeState(T newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }
    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}
