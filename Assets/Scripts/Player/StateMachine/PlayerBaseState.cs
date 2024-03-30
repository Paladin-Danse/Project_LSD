using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.ShaderGraph;
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
        Move();
        Rotate();
    }

    public virtual void PhysicsUpdate()
    {
        
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
        input.playerActions.Interact.started += stateMachine.player.dungeonInteract.OnInteractInput;

        //Inventory
        input.playerActions.Inventory.started += stateMachine.player.inventory.Toggle;
    }

    public virtual void RemoveInputActionsCallbacks()
    {
        //Movement
        PlayerInput input = stateMachine.player.input;
        input.playerActions.Move.canceled -= OnMovementCanceled;
        input.playerActions.Jump.started -= OnJump;
        input.playerActions.Run.started -= OnRun;

        //Interact
        input.playerActions.Interact.started -= stateMachine.player.dungeonInteract.OnInteractInput;
        //Inventory
        input.playerActions.Inventory.started -= stateMachine.player.inventory.Toggle;
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
