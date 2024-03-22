using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GunBaseState : IState
{
    protected GunStateMachine stateMachine;
    public GunBaseState(GunStateMachine gunStateMachine)
    {
        stateMachine = gunStateMachine;
    }

    public virtual void Enter()
    {
        stateMachine.DebugCurrentState();
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
    }
    public virtual void Update()
    {
    }
    public virtual void PhysicsUpdate()
    {
    }
    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Gun.input_;
        
        input.playerActions.Shoot.started += OnFire;
        input.playerActions.Shoot.canceled += StopFire;
        input.playerActions.Reload.started += OnReload;
    }

    
    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Gun.input_;
        
        input.playerActions.Shoot.started -= OnFire;
        input.playerActions.Shoot.canceled -= StopFire;
        input.playerActions.Reload.started -= OnReload;
    }

    protected virtual void OnFire(InputAction.CallbackContext callbackContext)
    {

    }
    protected virtual void StopFire(InputAction.CallbackContext callbackContext)
    {

    }

    protected void ProjectilePooling(AmmoProjectile projectile)
    {
        AmmoProjectile newProjectile;
        if (stateMachine.Gun.weaponProjectile_List.Exists(x => x.gameObject.activeSelf == false))
            newProjectile = stateMachine.Gun.weaponProjectile_List.Find(x => x.gameObject.activeSelf == false);
        else
            newProjectile = stateMachine.Gun.CreateObject(stateMachine.Gun.weaponProjectile_List, projectile);

        newProjectile.transform.position = stateMachine.Gun.firePos.position;
        newProjectile.transform.rotation = Quaternion.LookRotation(stateMachine.Gun.RandomSpread());
        newProjectile.OnInit(stateMachine.Gun);
    }
    protected virtual void OnReload(InputAction.CallbackContext callbackContext)
    {
        
    }
    protected IEnumerator OnRecoil()
    {
        stateMachine.targetRecoil = math.min(stateMachine.curRecoil + stateMachine.Gun.currentStat.recoil, stateMachine.maxRecoil);

        PlayerStateMachine playerStateMachine = stateMachine.playerStateMachine_;
        Transform camTransform = playerStateMachine.playerCamTransform;
        int cnt = 0;
        while (stateMachine.curRecoil < stateMachine.targetRecoil - 0.1f)
        {
            stateMachine.curRecoil = math.lerp(stateMachine.curRecoil, stateMachine.targetRecoil, 0.4f);
            
            cnt++;
            if (cnt > 100) break;
            
            yield return stateMachine.whileRestTimeSeconds;
        }
        cnt = 0;
        while(stateMachine.curRecoil > 0.1f)
        {
            if(stateMachine.Gun.isFiring) stateMachine.curRecoil = math.lerp(stateMachine.curRecoil, 0, 0.1f);
            else stateMachine.curRecoil = math.lerp(stateMachine.curRecoil, 0, 0.4f);
            cnt++;
            if(cnt > 100) break;

            yield return stateMachine.whileRestTimeSeconds;
        }
        stateMachine.RecoilCoroutine = null;
        Debug.Log("Coroutine end");
        yield break;
    }
}
