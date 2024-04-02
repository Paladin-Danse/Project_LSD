using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GunStateMachine : StateMachine<GunBaseState>
{
    public Weapon gun { get; }

    public GunReadyState ReadyState { get; }
    public GunEmptyState EmptyState { get; }
    public GunReloadState ReloadState { get; }
    public GunEnterState EnterState { get; }
    public GunExitState ExitState { get; }
    public Vector2 playerMovementInput { get; private set; }
    public GunStateMachine(Weapon Gun)
    {
        this.gun = Gun;
        
        ReadyState = new GunReadyState(this);
        EmptyState = new GunEmptyState(this);
        ReloadState = new GunReloadState(this);
        EnterState = new GunEnterState(this);
        ExitState = new GunExitState(this);
    }
    public void DebugCurrentState()
    {
        Debug.Log(currentState);
    }
}
