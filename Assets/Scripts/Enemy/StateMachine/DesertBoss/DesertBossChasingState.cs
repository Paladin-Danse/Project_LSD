using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossChasingState : DesertBossBaseState
{
    public DesertBossChasingState(DesertBossStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 1;
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.GroundParameterHash);
        
        StartAnimation(stateMachine.Enemy.AnimationData.WalkParameterHash);
        //StartAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.GroundParameterHash);

        StopAnimation(stateMachine.Enemy.AnimationData.WalkParameterHash);
        //StopAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();
        Debug.Log(IsInAttackRange()+"/"+IsInChaseRange());
        if (!IsInChaseRange() && !IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
            return;
        }
        else if (IsInChaseRange() && IsInAttackRange())
        {            
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
        else if (!IsInAttackRange() && IsInChaseRange())
        {
            //stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }

        //if (IsInChaseRange())
        //{
        //    stateMachine.ChangeState(stateMachine.ChasingState);
        //    return;
        //}

        //float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "@Attack");

        //if (0 < normalizedTime)
        //{
        //    if (!IsInAttackRange() && IsInChaseRange())
        //    {
        //        stateMachine.ChangeState(stateMachine.ChasingState);
        //        return;
        //    }
        //}
    }
}
