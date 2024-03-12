using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    DungeonInteract dungeonInteract = new DungeonInteract();

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        groundData = stateMachine.player.Data.groundData;
    }
    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void Update()
    {
        Move();
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.player.input;
        input.playerActions.Move.canceled += OnMovementCanceled;
        input.playerActions.Look.started += Rotate;
        input.playerActions.Interact.started += dungeonInteract.OnInteractInput;
    }
    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.player.input;
        input.playerActions.Move.canceled -= OnMovementCanceled;
        input.playerActions.Look.started -= Rotate;
        input.playerActions.Interact.started -= dungeonInteract.OnInteractInput;
    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext callbackContext)
    {

    }
    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.player.input.playerActions.Move.ReadValue<Vector2>();
    }
    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        Move(movementDirection);
    }
    protected void Rotate(InputAction.CallbackContext callbackContext)
    {
        Vector2 rotateDirection = stateMachine.player.input.playerActions.Look.ReadValue<Vector2>();
        
        Debug.Log(rotateDirection);
        PlayerData SOData = stateMachine.player.Data;
        Transform camTransform = stateMachine.PlayerCamTransform;
        Rigidbody rigidbody = stateMachine.player.rigidbody;

        camTransform.rotation *= Quaternion.Euler(new Vector3(camTransform.forward.x + rotateDirection.y, 0, 0) * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime * -1);
        rigidbody.rotation *= Quaternion.Euler(new Vector3(0, camTransform.forward.y + rotateDirection.x, 0) * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime);
    }


    protected void Move(Vector3 movementDirection)
    {
        Player player = stateMachine.player;
        float movementSpeed = GetMovementSpeed();
        //stateMachine.player.Controller.Move(
        //    ((movementDirection * movementSpeed) +
        //    stateMachine.player.ForceReceiver.Movement) *
        //    Time.deltaTime
        //    );
        player.rigidbody.MovePosition(player.transform.position + (movementDirection * movementSpeed * Time.deltaTime));
    }

    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.PlayerCamTransform.forward;
        Vector3 right = stateMachine.PlayerCamTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

}
