using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunReadyState : GunBaseState
{
    public GunReadyState(GunStateMachine gunStateMachine ) : base(gunStateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        stateMachine.Gun.isFiring = false;
        stateMachine.Gun.animator.SetInteger(stateMachine.Gun.animationData.fireParameterHash, -1);
        if (stateMachine.Gun.isEmpty) stateMachine.ChangeState(stateMachine.EmptyState);
    }
    public override void Exit()
    {
        base.Exit();
        stateMachine.Gun.isFiring = false;
        stateMachine.Gun.animator.SetInteger(stateMachine.Gun.animationData.fireParameterHash, -1);
    }
    public override void Update()
    {
        base.Update();
        if(stateMachine.Gun.isAuto && stateMachine.Gun.isFiring && !stateMachine.Gun.isEmpty) AutoFire();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        PlayerStateMachine playerStateMachine = stateMachine.Gun.playerCharacter_.stateMachine;
        int playerState = (playerStateMachine.currentState == playerStateMachine.WalkState ? 1 :
                           playerStateMachine.currentState == playerStateMachine.RunState ? 3 : 0);
        if(!stateMachine.Gun.isFiring)
            stateMachine.Gun.animator.SetInteger(stateMachine.Gun.animationData.movementSpeedParameterHash, playerState);
        else
            stateMachine.Gun.animator.SetInteger(stateMachine.Gun.animationData.movementSpeedParameterHash, 0);
    }
    protected override void OnFire(InputAction.CallbackContext callbackContext)
    {
        base.OnFire(callbackContext);
        Weapon curWeapon = stateMachine.Gun;

        curWeapon.isFiring = true;

        if (!curWeapon.isAuto && !curWeapon.isEmpty && curWeapon.isShotable)
        {
            curWeapon.animator.SetInteger(curWeapon.animationData.fireParameterHash, 1);
            curWeapon.ShotCoroutinePlay(curWeapon.Shot());
            curWeapon.animator.SetInteger(curWeapon.animationData.fireParameterHash, -1);
        }
    }

    protected override void StopFire(InputAction.CallbackContext callbackContext)
    {
        base.StopFire(callbackContext);
        Weapon curWeapon = stateMachine.Gun;

        if (curWeapon.isFiring)
        {
            curWeapon.isFiring = false;
            curWeapon.animator.SetInteger(curWeapon.animationData.fireParameterHash, -1);
            curWeapon.animator.SetInteger(curWeapon.animationData.movementSpeedParameterHash, 0);
        }
    }

    protected void AutoFire()
    {
        Weapon curWeapon = stateMachine.Gun;

        if (curWeapon.isFiring && !curWeapon.isEmpty && curWeapon.isShotable)
        {
            curWeapon.animator.SetInteger(curWeapon.animationData.fireParameterHash, 2);
            curWeapon.ShotCoroutinePlay(curWeapon.Shot());
        }
    }
    protected override void OnReload(InputAction.CallbackContext callbackContext)
    {
        base.OnReload(callbackContext);
        Weapon curWeapon = stateMachine.Gun;
        //재장전 하기 전 남은 잔탄을 현재 가지고 있는 전체 탄에 더할 것.
        if (curWeapon.curMagazine != curWeapon.maxMagazine)
        {
            curWeapon.isFiring = false;
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
    }
}
