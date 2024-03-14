using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerGroundedState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = stateMachine.player.Data.groundData.WalkSpeedModifier;
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        SetAnimation(stateMachine.player.AnimationData.DirectionParameterHash, stateMachine.MovementInput.x / 2);
        SetAnimation(stateMachine.player.AnimationData.SpeedParameterHash, stateMachine.MovementInput.y / 2);
    }
}
