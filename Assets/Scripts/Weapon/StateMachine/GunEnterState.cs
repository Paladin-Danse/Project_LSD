using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnterState : GunBaseState
{
    public GunEnterState(GunStateMachine gunStateMachine) : base(gunStateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.gun.PlayClip(stateMachine.gun.cock_AudioClip, stateMachine.gun.cock_Volume);
        stateMachine.gun.isSwap = true;
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
        stateMachine.gun.isSwap = false;
    }
    public override void Update()
    {
        base.Update();
        
        if (stateMachine.gun.animator.GetCurrentAnimatorStateInfo(0).IsTag("Enter"))
        {
            stateMachine.gun.TakeInCoroutinePlay();
        }
    }
}
