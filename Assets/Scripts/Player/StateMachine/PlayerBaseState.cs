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
        if(stateMachine.player.input != null)
            AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        if (stateMachine.player.input != null)
            RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void Update()
    {
        Move();
        Rotate();
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
        stateMachine.player.animator.SetFloat(ParameterHash, setFloat);
    }
    
    public virtual void AddInputActionsCallbacks()
    {
        //Movement
        PlayerInput input = stateMachine.player.input;
        input.playerActions.Move.canceled += OnMovementCanceled;
        input.playerActions.Jump.started += OnJump;
        input.playerActions.Run.started += OnRun;

        //Interact
        input.playerActions.Interact.started += stateMachine.player.dungeonInteract.OnInteractInput;

        //Inventory
        input.playerActions.Inventory.started += stateMachine.player.inventory.Toggle;
    }

    public virtual void RemoveInputActionsCallbacks()
    {
        //Movement
        PlayerInput input = stateMachine.player.input;
        input.playerActions.Move.canceled -= OnMovementCanceled;
        input.playerActions.Jump.started -= OnJump;
        input.playerActions.Run.started -= OnRun;

        //Interact
        input.playerActions.Interact.started -= stateMachine.player.dungeonInteract.OnInteractInput;
        //Inventory
        input.playerActions.Inventory.started -= stateMachine.player.inventory.Toggle;
    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext callbackContext)
    {

    }
    protected virtual void OnJump(InputAction.CallbackContext callbackContext)
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
        Vector2 rotateDirection = callbackContext.ReadValue<Vector2>();

        PlayerCharacter player = stateMachine.player;
        PlayerData SOData = player.Data;
        Transform camTransform = player.playerCamTransform;
        Rigidbody rigidbody = stateMachine.player.rigidbody_;
        

        player.camXRotate += rotateDirection.y * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime * -1;
        player.camXRotate = Mathf.Clamp(player.camXRotate, -SOData.UpdownMaxAngle, SOData.UpdownMaxAngle);
        stateMachine.playerYRotate += rotateDirection.x * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime;

        camTransform.localRotation = Quaternion.Euler(new Vector3(player.camXRotate - player.curWeapon.curRecoil, 0, 0));
        rigidbody.transform.rotation = Quaternion.Euler(new Vector3(0, stateMachine.playerYRotate, 0));
    }
    protected void Rotate()
    {
        PlayerCharacter player = stateMachine.player;
        Vector2 rotateDirection = player.input.playerActions.Look.ReadValue<Vector2>();

        PlayerData SOData = player.Data;
        Transform camTransform = player.playerCamTransform;
        Rigidbody rigidbody = player.rigidbody_;

        player.camXRotate += rotateDirection.y * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime * -1;
        player.camXRotate = Mathf.Clamp(player.camXRotate, -SOData.UpdownMaxAngle, SOData.UpdownMaxAngle);
        stateMachine.playerYRotate += rotateDirection.x * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime;

        camTransform.localRotation = Quaternion.Euler(new Vector3(player.camXRotate - player.curWeapon.curRecoil, 0, 0));
        rigidbody.transform.rotation = Quaternion.Euler(new Vector3(0, stateMachine.playerYRotate, 0));
    }
    protected void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.player.GetComponent<Rigidbody>().MovePosition(stateMachine.player.transform.position + (movementDirection * movementSpeed * Time.deltaTime));
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        
    }
    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.player.playerCamTransform.forward;
        Vector3 right = stateMachine.player.playerCamTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }
}
