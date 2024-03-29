using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerInput _input { get; private set; }
    
    public Inventory inventory;
    public PlayerUI playerUI;

    public PlayerCharacter playerCharacter;

    private void Awake()
    {
        _input = transform.AddComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        Possess(playerCharacter);
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {

    }

    public void Possess(PlayerCharacter playerCharacter)
    {
        this.playerCharacter = playerCharacter;
        OnControllCharacter();
        playerCharacter.playerUIEventInvoke();
    }

    public void UnPossessed(PlayerCharacter playerCharacter) 
    {
        this.playerCharacter = null;
        playerCharacter.playerUIEventInvoke();
    }

    public void OnControllCharacter() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCharacter.input = _input;
        playerCharacter.OnPossessCharacter();
    }

    public void OnControllUI() 
    {
        Cursor.lockState = CursorLockMode.None;
        playerCharacter.input = null;
        playerCharacter.OnUnpossessCharacter();
    }
}
