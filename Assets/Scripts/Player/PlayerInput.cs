using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions inputAction { get; private set; }
    public PlayerInputActions.PlayerActions playerActions { get; private set; }

    private void Awake()
    {
        inputAction = new PlayerInputActions();
        playerActions = inputAction.Player;
    }
    private void OnEnable()
    {
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Disable();
    }
}
