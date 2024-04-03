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
        //stateMachine.DebugCurrentState();
        if(stateMachine.gun.input_ != null)
            AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        if (stateMachine.gun.input_ != null)
            RemoveInputActionsCallbacks();
    }
    
    protected void SetAnimation(int ParameterHash)
    {
        stateMachine.gun.animator.SetTrigger(ParameterHash);
    }
    protected void SetAnimation(int ParameterHash, bool setBool)
    {
        stateMachine.gun.animator.SetBool(ParameterHash, setBool);
    }
    protected void SetAnimation(int ParameterHash, int setInt)
    {
        stateMachine.gun.animator.SetInteger(ParameterHash, setInt);
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
    public virtual void AddInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.gun.input_;
        
        input.playerActions.Shoot.started += OnFire;
        input.playerActions.Shoot.canceled += StopFire;
        input.playerActions.Reload.started += OnReload;
    }

    
    public virtual void RemoveInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.gun.input_;
        
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
    protected virtual void OnReload(InputAction.CallbackContext callbackContext)
    {
        
    }
}
