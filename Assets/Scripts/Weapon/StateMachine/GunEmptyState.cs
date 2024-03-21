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
        if (stateMachine.ShotCoroutine == null)
        {
            stateMachine.ShotCoroutine = DryFire();
            stateMachine.Gun.StartCoroutine(stateMachine.ShotCoroutine);
        }
    }

    IEnumerator DryFire()
    {
        stateMachine.Gun.PlayClip(stateMachine.Gun.dry_AudioClip, stateMachine.Gun.dry_Volume);
        yield return null;
        stateMachine.ShotCoroutine = null;
    }
}
