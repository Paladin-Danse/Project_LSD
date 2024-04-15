using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions inputAction { get; private set; }
    public PlayerInputActions.PlayerActions playerActions { get; private set; }
    public PlayerInputActions.PlayerUIActions playerUIActions { get; private set; }

    private void Awake()
    {
        inputAction = new PlayerInputActions();
        playerActions = inputAction.Player;
        playerUIActions = inputAction.PlayerUI;
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Disable();
    }

    public void SetCursorLock(bool setBool)
    {
        if (setBool)
        {
            playerActions.Jump.Enable();
            playerActions.Shoot.Enable();
            playerActions.Move.Enable();
            playerActions.Reload.Enable();
            playerActions.Aim.Enable();
            playerActions.Look.Enable();
            playerActions.WeaponSwap.Enable();

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            playerActions.Jump.Disable();
            playerActions.Shoot.Disable();
            playerActions.Move.Disable();
            playerActions.Reload.Disable();
            playerActions.Aim.Disable();
            playerActions.Look.Disable();
            playerActions.WeaponSwap.Disable();

            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
