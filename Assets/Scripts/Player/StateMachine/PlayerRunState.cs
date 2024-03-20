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
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        SetAnimation(stateMachine.player.AnimationData.DirectionParameterHash, stateMachine.MovementInput.x);
        SetAnimation(stateMachine.player.AnimationData.SpeedParameterHash, stateMachine.MovementInput.y);
    }
}
