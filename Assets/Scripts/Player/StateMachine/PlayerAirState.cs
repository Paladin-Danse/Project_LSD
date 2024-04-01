using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    
    public PlayerAirState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    //"velocity.y < 0 ? falling : jump" hmm.....
    protected override void Move()
    {
        base.Move();
        stateMachine.player.JumpMove();
    }
}
