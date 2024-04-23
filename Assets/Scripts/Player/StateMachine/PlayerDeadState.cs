using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.DebugCurrentState();
        RemoveInputActionsCallbacks();
        SetAnimation(stateMachine.player.AnimationData.DeathParameterHash);
    }
    public override void HandleInput()
    {
        
    }
    public override void Update()
    {
        
    }
}
