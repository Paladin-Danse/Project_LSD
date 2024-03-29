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
        if (stateMachine.Gun.ShotCoroutine == null)
        {
            stateMachine.Gun.ShotCoroutinePlay(stateMachine.Gun.DryFire());
        }
    }

    

    protected override void OnReload(InputAction.CallbackContext callbackContext)
    {
        base.OnReload(callbackContext);
        if (stateMachine.Gun.curMagazine != stateMachine.Gun.maxMagazine)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
    }
}
