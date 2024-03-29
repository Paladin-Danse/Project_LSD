using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBaseState : IState
{
    protected RangedEnemyStateMachine stateMachine;

    protected readonly EnemyGroundData groundData;
    public RangedEnemyBaseState(RangedEnemyStateMachine ememyStateMachine)
    {
        stateMachine = ememyStateMachine;
        groundData = stateMachine.Enemy.RData.GroundedData;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void Update()
    {
        Move();
    }

    public virtual void PhysicsUpdate()
    {

    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, false);
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);
        Move(movementDirection);
    }


    private Vector3 GetMovementDirection()
    {
        return (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).normalized;
    }

    private void Move(Vector3 direction)
    {
        RangedEnemy enemy = stateMachine.Enemy;
        float movementSpeed = GetMovementSpeed();
        //Debug.Log(direction + "/" + movementSpeed);
        //Debug.Log(stateMachine.Enemy.Animator.GetBool("@Attack"));
        enemy.GetComponent<Rigidbody>().MovePosition(enemy.transform.position + ((direction * movementSpeed) * Time.deltaTime));
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            stateMachine.Enemy.transform.rotation = Quaternion.Slerp(stateMachine.Enemy.transform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    protected float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;

        return movementSpeed;
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        if (!animator.IsInTransition(0) && animator.GetBool(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }


    protected bool IsInChaseRange()
    {
        if (stateMachine.Target.IsDead) { return false; }

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.Enemy.RData.PlayerChasingRange * stateMachine.Enemy.RData.PlayerChasingRange;
    }

    protected bool IsInAttackRange()
    {
        if (stateMachine.Target.IsDead) { return false; }

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.Enemy.RData.AttackRange * stateMachine.Enemy.RData.AttackRange;
    }
}
