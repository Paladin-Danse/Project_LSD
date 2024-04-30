using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions inputAction { get; private set; }
    public PlayerInputActions.PlayerActions playerActions { get; private set; }
    public PlayerInputActions.WeaponActions weaponActions { get; private set; }
    public PlayerInputActions.PlayerUIActions playerUIActions { get; private set; }
    

    private void Awake()
    {
        inputAction = new PlayerInputActions();
        playerActions = inputAction.Player;
        playerUIActions = inputAction.PlayerUI;
        weaponActions = inputAction.Weapon;
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
            playerActions.Enable();
            weaponActions.Enable();

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            playerActions.Disable();
            weaponActions.Disable();

            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
