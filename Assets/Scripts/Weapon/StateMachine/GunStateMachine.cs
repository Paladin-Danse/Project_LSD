using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GunStateMachine : StateMachine<GunBaseState>
{
    public Weapon Gun { get; }

    public GunReadyState ReadyState { get; }
    public GunEmptyState EmptyState { get; }
    public GunReloadState ReloadState { get; }
    
    public GunStateMachine(Weapon Gun)
    {
        this.Gun = Gun;
        
        ReadyState = new GunReadyState(this);
        EmptyState = new GunEmptyState(this);
        ReloadState = new GunReloadState(this);
    }
    public void DebugCurrentState()
    {
        Debug.Log(currentState);
    }
}
