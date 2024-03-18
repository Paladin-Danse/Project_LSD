using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunReadyState : GunBaseState
{
    public GunReadyState(GunStateMachine gunStateMachine ) : base(gunStateMachine)
    {

    }
    public override void Update()
    {
        base.Update();
        if(stateMachine.Gun.isAuto && stateMachine.Gun.isFiring) AutoFire();
    }

    protected override void OnFire(InputAction.CallbackContext callbackContext)
    {
        base.OnFire(callbackContext);
        Weapon curWeapon = stateMachine.Gun;

        curWeapon.isFiring = true;

        if (!curWeapon.isAuto && stateMachine.ShotCoroutine == null)
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

        if (stateMachine.ShotCoroutine == null && curWeapon.isFiring)
        {
            stateMachine.ShotCoroutine = Shot();
            curWeapon.StartCoroutine(stateMachine.ShotCoroutine);
        }
    }
    // Start is called before the first frame update
    protected IEnumerator Shot()
    {
        ProjectilePooling(stateMachine.Gun.ammoProjectile);
        yield return stateMachine.weaponAttackDelay;
        stateMachine.ShotCoroutine = null;
    }

}
