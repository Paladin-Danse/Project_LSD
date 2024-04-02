using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunExitState : GunBaseState
{
    public GunExitState(GunStateMachine gunStateMachine) : base(gunStateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.gun.PlayClip(stateMachine.gun.takeOut_AudioClip, stateMachine.gun.takeOut_Volume);
        base.Enter();
        SetAnimation(stateMachine.gun.animationData.takeoutParameterHash);
    }
    public override void Update()
    {
        base.Update();
        if (stateMachine.gun.animator.GetCurrentAnimatorStateInfo(0).IsTag("Exit"))
        {
            stateMachine.gun.TakeOutCoroutinePlay();
        }
    }
}
