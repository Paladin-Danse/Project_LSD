using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossAttackState : DesertBossBaseState
{
    public DesertBossAttackState(DesertBossStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
    }

    public override void Enter()
    {        
        stateMachine.MovementSpeedModifier = 0;
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.BigAttackParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.SmallAttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.BigAttackParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.SmallAttackParameterHash);

    }

    public override void Update()
    {
        base.Update();

        //float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "@Attack");

        //if (0 < normalizedTime)
        //{
        //    if (!IsInAttackRange())
        //    {
        //        stateMachine.ChangeState(stateMachine.ChasingState);
        //        return;
        //    }
        //}

        //if (!IsInAttackRange())
        //{
        //    stateMachine.ChangeState(stateMachine.ChasingState);
        //    return;
        //}

        if (!IsInChaseRange() && !IsInAttackRange())
        {
            //stateMachine.ChangeState(stateMachine.IdlingState);
            return;
        }
        else if (IsInChaseRange() && IsInAttackRange())
        {
            //stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
        else if (!IsInAttackRange() && IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }
    }
}
