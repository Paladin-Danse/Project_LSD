using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunEmptyState : GunBaseState
{
    public GunEmptyState(GunStateMachine gunStateMachine) : base(gunStateMachine)
    {
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        PlayerStateMachine playerStateMachine = stateMachine.gun.playerCharacter_.stateMachine;
        int playerState = (playerStateMachine.currentState == playerStateMachine.WalkState ? 1 :
                           playerStateMachine.currentState == playerStateMachine.RunState ? 3 : 0);
        if (!stateMachine.gun.isFiring)
            SetAnimation(stateMachine.gun.animationData.movementSpeedParameterHash, playerState);
        else
            SetAnimation(stateMachine.gun.animationData.movementSpeedParameterHash, 0);
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
