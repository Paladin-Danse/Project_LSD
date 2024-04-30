using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        groundData = stateMachine.player.Data.groundData;
    }
    public virtual void Enter()
    {
        stateMachine.DebugCurrentState();
        if(stateMachine.player.input != null)
            AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        if (stateMachine.player.input != null)
            RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void Update()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        Move();
        Rotate();
    }
    protected void SetAnimation(int ParameterHash)
    {
        stateMachine.player.animator.SetTrigger(ParameterHash);
    }
    protected void SetAnimation(int ParameterHash, bool setBool)
    {
        stateMachine.player.animator.SetBool(ParameterHash, setBool);
    }
    protected void SetAnimation(int ParameterHash, float setFloat)
    {
        stateMachine.player.MoveLerpAnimation(ParameterHash, setFloat);
        //stateMachine.player.animator.SetFloat(ParameterHash, setFloat);
    }
    
    public virtual void AddInputActionsCallbacks()
    {
        //Movement
        PlayerInput input = stateMachine.player.input;
        input.playerActions.Move.canceled += OnMovementCanceled;
        input.playerActions.Jump.started += OnJump;
        input.playerActions.Run.started += OnRun;

        //Interact
        input.playerUIActions.Interact.started += stateMachine.player.playerInteract.OnInteractInput;

        //Equipment
        input.weaponActions.WeaponSwap.started += OnSwap;
    }

    public virtual void RemoveInputActionsCallbacks()
    {
        //Movement
        PlayerInput input = stateMachine.player.input;
        input.playerActions.Move.canceled -= OnMovementCanceled;
        input.playerActions.Jump.started -= OnJump;
        input.playerActions.Run.started -= OnRun;

        //Interact
        input.playerUIActions.Interact.started -= stateMachine.player.playerInteract.OnInteractInput;
        
        //Equipment
        input.weaponActions.WeaponSwap.started -= OnSwap;
    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext callbackContext)
    {

    }
    protected virtual void OnJump(InputAction.CallbackContext callbackContext)
    {

    }

    protected virtual void OnRun(InputAction.CallbackContext context)
    {

    }
    protected virtual void OnSwap(InputAction.CallbackContext context)
    {
        stateMachine.player.WeaponSwap();
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.player.input.playerActions.Move.ReadValue<Vector2>();
    }
    protected virtual void Move()
    {
        
    }
    private void Rotate()
    {
        stateMachine.player.Rotate();
    }

    
    
}
