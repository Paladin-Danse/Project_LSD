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
        
        if (stateMachine.player.isGrounded && stateMachine.player.isJump)
        {
            SetAnimation(stateMachine.player.AnimationData.JumpParameterHash);
            SetAnimation(stateMachine.player.AnimationData.GroundParameterHash, false);

            stateMachine.player.JumpMoveSetting();
            stateMachine.player.Jump();
        }
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
        if (stateMachine.player.rigidbody_.velocity.y < 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}
