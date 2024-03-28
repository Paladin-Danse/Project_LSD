using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundedState
{
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.player.MovementSpeedModifier = groundData.RunSpeedModifier;
        stateMachine.player.curWeapon.isShotable = false;
        base.Enter();
    }
    public override void Exit()
    {
        stateMachine.player.curWeapon.isShotable = true;
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (stateMachine.MovementInput.y <= 0.1f)
            stateMachine.ChangeState(stateMachine.WalkState);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        SetAnimation(stateMachine.player.AnimationData.DirectionParameterHash, stateMachine.MovementInput.x);
        SetAnimation(stateMachine.player.AnimationData.SpeedParameterHash, stateMachine.MovementInput.y);
    }
}
