using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttackState : RangedEnemyBaseState
{    
    //private bool alreadyAppliedDealing;

    public RangedEnemyAttackState(RangedEnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
    }

    public override void Enter()
    {        
        //alreadyAppliedDealing = false;

        stateMachine.MovementSpeedModifier = 0;
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.BaseAttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.BaseAttackParameterHash);

    }

    public override void Update()
    {
        base.Update();

        //float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "@Attack");

        //if (0 < normalizedTime)
        //{
        //    //if (!alreadyAppliedDealing && normalizedTime >= stateMachine.Enemy.RData.Dealing_Start_TransitionTime)
        //    //{
        //    //    stateMachine.Enemy.Projectile.SetDamage(stateMachine.Enemy.RData.Damage);
        //    //}

        //    if (!IsInAttackRange())
        //    {
        //        stateMachine.ChangeState(stateMachine.ChasingState);
        //        return;
        //    }
        //}
        //
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
