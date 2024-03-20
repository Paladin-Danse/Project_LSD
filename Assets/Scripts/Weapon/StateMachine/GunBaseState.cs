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
        if (stateMachine.addRecoil > 0) RecoverRecoil();
    }
    public virtual void PhysicsUpdate()
    {
    }
    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Gun.input_;
        
        input.playerActions.Shoot.started += OnFire;
        input.playerActions.Shoot.canceled += StopFire;
    }

    
    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Gun.input_;
        
        input.playerActions.Shoot.started -= OnFire;
        input.playerActions.Shoot.canceled -= StopFire;
    }

    protected virtual void OnFire(InputAction.CallbackContext callbackContext)
    {

    }
    protected virtual void StopFire(InputAction.CallbackContext callbackContext)
    {

    }

    protected void ProjectilePooling(AmmoProjectile projectile)
    {
        if (stateMachine.Gun.weaponProjectile_List.Exists(x => x.gameObject.activeSelf == false))
        {
            AmmoProjectile findProjectile = stateMachine.Gun.weaponProjectile_List.Find(x => x.gameObject.activeSelf == false);
            findProjectile.transform.position = stateMachine.Gun.firePos.position;
            findProjectile.transform.rotation = Quaternion.LookRotation(-stateMachine.Gun.firePos.forward);
            findProjectile.OnInit(stateMachine.Gun);
        }
        else
        {
            AmmoProjectile newProjectile = stateMachine.Gun.CreateObject(stateMachine.Gun.weaponProjectile_List, projectile);
            newProjectile.transform.position = stateMachine.Gun.firePos.position;
            newProjectile.OnInit(stateMachine.Gun);
        }
    }
    protected virtual void OnReload(InputAction.CallbackContext callbackContext)
    {
        
    }
    public void RecoverRecoil()
    {
        
    }
    protected IEnumerator OnRecoil()
    {
        stateMachine.OnRecoil();
        PlayerStateMachine playerStateMachine = stateMachine.playerStateMachine_;
        float OriginRotate = playerStateMachine.camXRotate;
        float lerpRotate = playerStateMachine.camXRotate;

        while (playerStateMachine.camXRotate - stateMachine.addRecoil != OriginRotate)
        {
            lerpRotate = math.lerp(lerpRotate, playerStateMachine.camXRotate - stateMachine.addRecoil, 0.5f);
            playerStateMachine.playerCamTransform.localRotation = Quaternion.Euler(new Vector3(lerpRotate, 0, 0));
            yield return null;
        }
        while(stateMachine.addRecoil > 0)
        {
            stateMachine.addRecoil = math.lerp(stateMachine.addRecoil, 0, 0.2f);
            playerStateMachine.playerCamTransform.localRotation = Quaternion.Euler(OriginRotate - stateMachine.addRecoil, 0, 0);
            yield return null;
        }
    }
}
