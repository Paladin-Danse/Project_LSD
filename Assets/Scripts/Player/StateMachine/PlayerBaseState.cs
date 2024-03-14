using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    
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
    protected void SetAnimation(int ParameterHash)
    {
        stateMachine.player.animator.SetTrigger(ParameterHash);
    }
    protected void SetAnimation(int ParameterHash, bool setBool)
    {
        stateMachine.player.animator.SetBool(ParameterHash, setBool);
    }
    protected void SetAnimation(int ParameterHash, float setFloat)
    {
        //stateMachine.player.animator.SetFloat(ParameterHash, math.lerp());
    }
    
    protected virtual void AddInputActionsCallbacks()
    {
        //Movement
        PlayerInput input = stateMachine.player.input_;
        input.playerActions.Move.canceled += OnMovementCanceled;
        input.playerActions.Look.started += Rotate;
        input.playerActions.Jump.started += OnJump;
        input.playerActions.Run.started += OnRun;

        //Interact
        input.playerActions.Interact.started += stateMachine.player.dungeonInteract.OnInteractInput;
    }

    
    protected virtual void RemoveInputActionsCallbacks()
    {
        //Movement
        PlayerInput input = stateMachine.player.input_;
        input.playerActions.Move.canceled -= OnMovementCanceled;
        input.playerActions.Look.started -= Rotate;
        input.playerActions.Jump.started -= OnJump;
        input.playerActions.Run.started -= OnRun;

        //Interact
        input.playerActions.Interact.started -= stateMachine.player.dungeonInteract.OnInteractInput;
    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext callbackContext)
    {

    }
    protected virtual void OnJump(InputAction.CallbackContext callbackContext)
    {

    }
    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.player.input_.playerActions.Move.ReadValue<Vector2>();
    }
    private void Move()
    {
        Player player = stateMachine.player;
        
        Vector3 movementDirection = GetMovementDirection();
        Move(movementDirection);
    }
    protected void Rotate(InputAction.CallbackContext callbackContext)
    {
        Vector2 rotateDirection = callbackContext.ReadValue<Vector2>();

        PlayerData SOData = stateMachine.player.Data;
        Transform camTransform = stateMachine.PlayerCamTransform;
        Rigidbody rigidbody = stateMachine.player.rigidbody_;
        
        
        stateMachine.camXRotate += rotateDirection.y * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime * -1;
        stateMachine.camXRotate = Mathf.Clamp(stateMachine.camXRotate, -SOData.UpdownMaxAngle, SOData.UpdownMaxAngle);

        stateMachine.playerYRotate += rotateDirection.x * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime;

        camTransform.localRotation = Quaternion.Euler(new Vector3(stateMachine.camXRotate, 0, 0));
        rigidbody.transform.rotation = Quaternion.Euler(new Vector3(0, stateMachine.playerYRotate, 0));
    }

    protected void Move(Vector3 movementDirection)
    {
        Player player = stateMachine.player;
        float movementSpeed = GetMovementSpeed();
        player.GetComponent<Rigidbody>().MovePosition(player.transform.position + (movementDirection * movementSpeed * Time.deltaTime));
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
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
