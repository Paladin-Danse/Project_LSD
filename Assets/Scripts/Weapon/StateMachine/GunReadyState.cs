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
        if (stateMachine.isEmpty) stateMachine.ChangeState(stateMachine.EmptyState);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if(stateMachine.Gun.isAuto && stateMachine.Gun.isFiring && !stateMachine.isEmpty) AutoFire();
    }

    protected override void OnFire(InputAction.CallbackContext callbackContext)
    {
        base.OnFire(callbackContext);
        Weapon curWeapon = stateMachine.Gun;

        curWeapon.isFiring = true;

        if (!curWeapon.isAuto && stateMachine.ShotCoroutine == null && !stateMachine.isEmpty)
        {
            stateMachine.ShotCoroutine = Shot();
            curWeapon.StartCoroutine(stateMachine.ShotCoroutine);
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

        if (stateMachine.ShotCoroutine == null && curWeapon.isFiring && !stateMachine.isEmpty)
        {
            stateMachine.ShotCoroutine = Shot();
            curWeapon.StartCoroutine(stateMachine.ShotCoroutine);
        }
    }
    protected override void OnReload(InputAction.CallbackContext callbackContext)
    {
        base.OnReload(callbackContext);

    }
    protected IEnumerator Shot()
    {
        stateMachine.curMagazine--;
        //stateMachine.Gun.StartCoroutine(OnRecoil());
        if (stateMachine.isEmpty) stateMachine.ChangeState(stateMachine.EmptyState);
        ProjectilePooling(stateMachine.Gun.ammoProjectile);
        yield return stateMachine.weaponAttackDelay;
        stateMachine.ShotCoroutine = null;
    }
    
}
