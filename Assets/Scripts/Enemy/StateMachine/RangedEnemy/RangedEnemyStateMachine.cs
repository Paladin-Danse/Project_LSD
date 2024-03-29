using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyStateMachine : StateMachine<RangedEnemyBaseState>
{
    public RangedEnemy Enemy { get; }

    public Health Target { get; private set; }

    public RangedEnemyIdleState IdlingState { get; }
    public RangedEnemyChasingState ChasingState { get; }
    public RangedEnemyAttackState AttackState { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public RangedEnemyStateMachine(RangedEnemy enemy)
    {
        Enemy = enemy;
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        IdlingState = new RangedEnemyIdleState(this);
        ChasingState = new RangedEnemyChasingState(this);
        AttackState = new RangedEnemyAttackState(this);

        MovementSpeed = enemy.RData.GroundedData.BaseSpeed;
        RotationDamping = enemy.RData.GroundedData.BaseRotationDamping;
    }
}
