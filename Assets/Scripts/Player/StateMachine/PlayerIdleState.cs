using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        SetAnimation(stateMachine.player.AnimationData.DirectionParameterHash, 0f);
        SetAnimation(stateMachine.player.AnimationData.SpeedParameterHash, 0f);
    }
    public override void Exit() 
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if(stateMachine.MovementInput != Vector2.zero)
        {
            OnMove();
            return;
        }
    }
}
