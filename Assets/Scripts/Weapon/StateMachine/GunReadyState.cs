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
        stateMachine.Gun.isFiring = false;
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
        //재장전 하기 전 남은 잔탄을 현재 가지고 있는 전체 탄에 더할 것.
        if(stateMachine.curMagazine != stateMachine.maxMagazine)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
    }
    protected IEnumerator Shot()
    {
        stateMachine.curMagazine--;
        stateMachine.Gun.PlayClip(stateMachine.Gun.shot_AudioClip, stateMachine.Gun.shot_Volume);
        stateMachine.playerStateMachine_.player.playerUIEventInvoke();

        //Projectile Create
        ProjectilePooling(stateMachine.Gun.ammoProjectile);
        //Recoil
        if (stateMachine.RecoilCoroutine != null) stateMachine.Gun.StopCoroutine(stateMachine.RecoilCoroutine);
        stateMachine.RecoilCoroutine = OnRecoil();
        stateMachine.Gun.StartCoroutine(stateMachine.RecoilCoroutine);
        //Empty Check
        if (stateMachine.isEmpty) stateMachine.ChangeState(stateMachine.EmptyState);
        //shoot CoolTime
        yield return stateMachine.weaponAttackDelay;
        stateMachine.ShotCoroutine = null;
    }
}
