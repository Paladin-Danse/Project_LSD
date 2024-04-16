using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        PlayerInput input = stateMachine.player.input;
        //Inventory
        input.playerUIActions.Inventory.started += Player.Instance.inventory.Toggle;
    }
    public override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        PlayerInput input = stateMachine.player.input;
        //Inventory
        input.playerUIActions.Inventory.started -= Player.Instance.inventory.Toggle;

    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput == Vector2.zero)
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMovementCanceled(context);
    }
    protected override void Move()
    {
        base.Move();
        stateMachine.player.Move();
    }
    protected virtual void OnMove()
    {
        stateMachine.ChangeState(stateMachine.WalkState);
    }

    protected override void OnRun(InputAction.CallbackContext context)
    {
        base.OnRun(context);
        stateMachine.ChangeState(stateMachine.RunState);
    }

    protected override void OnJump(InputAction.CallbackContext callbackContext)
    {
        if(stateMachine.player.isJump)
        {
            stateMachine.ChangeState(stateMachine.JumpState);
        }   
    }
}
