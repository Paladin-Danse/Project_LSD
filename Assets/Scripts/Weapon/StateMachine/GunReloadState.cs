using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunReloadState : GunBaseState
{
    
    public GunReloadState(GunStateMachine gunStateMachine) : base(gunStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (stateMachine.Gun.ReloadCoroutine == null)
        {
            stateMachine.Gun.ReloadCoroutine = stateMachine.Gun.Reload();
            stateMachine.Gun.StartCoroutine(stateMachine.Gun.ReloadCoroutine);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.ReadyState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
