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
        if (stateMachine.Gun.isEmpty) stateMachine.ChangeState(stateMachine.EmptyState);
        stateMachine.Gun.isFiring = false;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if(stateMachine.Gun.isAuto && stateMachine.Gun.isFiring && !stateMachine.Gun.isEmpty) AutoFire();
    }

    protected override void OnFire(InputAction.CallbackContext callbackContext)
    {
        base.OnFire(callbackContext);
        Weapon curWeapon = stateMachine.Gun;

        curWeapon.isFiring = true;

        if (!curWeapon.isAuto && !curWeapon.isEmpty && curWeapon.isShotable)
        {
            curWeapon.ShotCoroutinePlay(curWeapon.Shot());
        }
    }

    protected override void StopFire(InputAction.CallbackContext callbackContext)
    {
        base.StopFire(callbackContext);
        Weapon curWeapon = stateMachine.Gun;

        if (curWeapon.isFiring) curWeapon.isFiring = false;
    }

    protected void AutoFire()
    {
        Weapon curWeapon = stateMachine.Gun;

        if (curWeapon.isFiring && !curWeapon.isEmpty && curWeapon.isShotable)
        {
            curWeapon.ShotCoroutinePlay(curWeapon.Shot());
        }
    }
    protected override void OnReload(InputAction.CallbackContext callbackContext)
    {
        base.OnReload(callbackContext);
        Weapon curWeapon = stateMachine.Gun;
        //������ �ϱ� �� ���� ��ź�� ���� ������ �ִ� ��ü ź�� ���� ��.
        if (curWeapon.curMagazine != curWeapon.maxMagazine)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
    }
}
