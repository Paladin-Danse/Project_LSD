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
        if (stateMachine.gun.ReloadCoroutine == null && stateMachine.gun.CheckInventoryAmmo())
        {
            SetAnimation(stateMachine.gun.animationData.movementSpeedParameterHash, 0);
            stateMachine.gun.ReloadCoroutine = stateMachine.gun.Reload();
            stateMachine.gun.StartCoroutine(stateMachine.gun.ReloadCoroutine);
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
