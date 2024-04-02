using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunEmptyState : GunBaseState
{
    public GunEmptyState(GunStateMachine gunStateMachine) : base(gunStateMachine)
    {
    }

    protected override void OnFire(InputAction.CallbackContext callbackContext)
    {
        base.OnFire(callbackContext);
        if (stateMachine.gun.ShotCoroutine == null)
        {
            stateMachine.gun.ShotCoroutinePlay(stateMachine.gun.DryFire());
        }
    }

    

    protected override void OnReload(InputAction.CallbackContext callbackContext)
    {
        base.OnReload(callbackContext);
        if (stateMachine.gun.curMagazine != stateMachine.gun.maxMagazine)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
    }
}
