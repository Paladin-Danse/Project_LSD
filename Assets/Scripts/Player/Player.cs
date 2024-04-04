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
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(Player).GetType().Name);
                    instance = obj.AddComponent<Player>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    public PlayerInput _input { get; private set; }
    
    public Inventory inventory;
    public PlayerUI playerUI;
    public PlayerCharacter playerCharacter;

    public event Action OnPossessed;
    public event Action OnUnPossessed;

    private void Awake()
    {
        _input = transform.AddComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        Possess(playerCharacter);
    }

    public void Possess(PlayerCharacter playerCharacter)
    {
        this.playerCharacter = playerCharacter;
        OnPossessed.Invoke();
        OnControllCharacter();
        playerCharacter.playerUIEventInvoke();
    }

    public void UnPossess(PlayerCharacter playerCharacter) 
    {
        this.playerCharacter = null;
        OnUnPossessed.Invoke();
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
