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
        if(stateMachine.player.curWeapon != null) 
        {
            stateMachine.player.curWeapon?.CancelReload();
            stateMachine.player.curWeapon.isShotable = false;
        }
        stateMachine.player.input.weaponActions.Disable();
        base.Enter();
    }
    public override void Exit()
    {
        if (stateMachine.player.curWeapon != null)
        {
            stateMachine.player.curWeapon.isShotable = true;
        }
        stateMachine.player.input.weaponActions.Enable();
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
