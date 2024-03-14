using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        bool isGround = stateMachine.player.isGrounded;
        if (isGround)
        {
            float jumpForce = stateMachine.player.Data.airData.JumpForce *
                              stateMachine.player.Data.airData.JumpForceModifier;
            stateMachine.player.rigidbody_.velocity = new Vector3(0, jumpForce, 0);
            stateMachine.player.isGrounded = false;
        }
        base.Enter();

        //(stateMachine.player.AnimationData.JumpParameterHash);
    }
    public override void Exit()
    {
        base.Exit();

        //StopAnimation(stateMachine.player.AnimationData.JumpParameterHash);
    }
    public override void Update()
    {
        base.Update();
        
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (stateMachine.player.rigidbody_.velocity.y <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}
