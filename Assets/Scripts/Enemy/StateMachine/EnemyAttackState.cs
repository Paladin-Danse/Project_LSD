using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{

    private bool alreadyAppliedForce;
    private bool alreadyAppliedDealing;

    public EnemyAttackState(EnemyStateMachine ememyStateMachine) : base(ememyStateMachine)
    {
    }    

    public override void Enter()
    {        
        alreadyAppliedDealing = false;

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

        float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "@Attack");        
        
        if (0 < normalizedTime && normalizedTime < 1f)
        {            
            if (!alreadyAppliedDealing && normalizedTime >= stateMachine.Enemy.Data.Dealing_Start_TransitionTime)
            {                
                stateMachine.Enemy.Weapon.SetAttack(stateMachine.Enemy.Data.Damage);
                stateMachine.Enemy.Weapon.gameObject.SetActive(true);                            
            }            

        }
        else
        {
            stateMachine.Enemy.Weapon.gameObject.SetActive(false);            
            if (IsInChaseRange())
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                return;
            }
        }

        if (!IsInAttackRange())
        {
            stateMachine.Enemy.Weapon.gameObject.SetActive(false);
            stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }

    }    
}
