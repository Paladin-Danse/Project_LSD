using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine<PlayerBaseState>
{
    public PlayerCharacter player { get; }

    public float playerYRotate = 0f;
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerRunState RunState { get; }
    public PlayerFallState FallState { get; }
    public PlayerJumpState JumpState { get; }
    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float MovementSpeedModifier { get; set; }
    public float JumpForce { get; set; }
    public bool IsAttacking { get; set; }
    public int ComboIndex { get; set; }

    public Action<PlayerCharacter> playerUIEvent;
    public PlayerStateMachine(PlayerCharacter player)
    {
        this.player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        FallState = new PlayerFallState(this);
        JumpState = new PlayerJumpState(this);

        MovementSpeed = this.player.Data.groundData.BaseSpeed;
        MovementSpeedModifier = this.player.Data.groundData.WalkSpeedModifier;

    }
}
