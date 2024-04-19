using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossStateMachine : StateMachine<DesertBossBaseState>
{
    public DesertBoss Enemy { get; }

    public Health Target { get; set; }

    public DesertBossIdleState IdlingState { get; }
    public DesertBossChasingState ChasingState { get; }
    public DesertBossAttackState AttackState { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public DesertBossStateMachine(DesertBoss enemy)
    {
        Enemy = enemy;
        //Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        IdlingState = new DesertBossIdleState(this);
        ChasingState = new DesertBossChasingState(this);
        AttackState = new DesertBossAttackState(this);

        MovementSpeed = enemy.RData.GroundedData.BaseSpeed;
        RotationDamping = enemy.RData.GroundedData.BaseRotationDamping;
    }
}
