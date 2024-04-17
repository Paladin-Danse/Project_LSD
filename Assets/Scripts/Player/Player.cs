using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
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
                    GameObject obj = new GameObject(typeof(Player).Name);
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
    public PlayerInteract playerInteract;

    public event Action OnPossessed;
    public event Action OnUnPossessed;

    private void Awake()
    {
        _input = transform.AddComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();
        playerInteract = transform.AddComponent<PlayerInteract>();
    }

    private void Start()
    {
        // PlayerData �ε�
        playerInteract.RegisterPlayer(this);
        _input.playerUIActions.Inventory.started += Toggle;
        Possess(playerCharacter);
    }

    public void Possess(PlayerCharacter playerCharacter)
    {
        this.playerCharacter = playerCharacter;
        playerCharacter.OnPossessCharacter(this);
        // playerCharacter.OnPossessCharacter���� Inventory ������ �޾ƿͼ� ���� ���� �� ��
        OnControllCharacter();

        // temp codes
        if(UIController.Instance.Push<PlayerUI>("HUDCanvas", out playerUI)) 
        {
            playerUI.BindPlayerCharacter(playerCharacter);
        }
    }

    public void UnPossess() 
    {
        playerUI.ReleasePlayerCharacter();
        playerCharacter.OnUnpossessCharacter();
        this.playerCharacter = null;
    }

    public void OnControllCharacter() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        _input.playerActions.Enable();
        _input.weaponActions.Enable();
    }

    public void OnControllUI() 
    {
        Cursor.lockState = CursorLockMode.None;
        _input.playerActions.Disable();
        _input.weaponActions.Disable();
    }

    public void LoadData() 
    {
        // load Inventory data
    }

    public void SaveData() 
    {
        // save Inventory data
    }

    public void Toggle(InputAction.CallbackContext callbackContext)
    {
        if(UIController.Instance.Peek(out GameObject gameObject)) 
        {
            if(gameObject.TryGetComponent(out InventoryUI inventoryUI)) 
            {
                UIController.Instance.Pop();
                Player.Instance.OnControllCharacter();
            }
        }
        else 
        {
            UIController.Instance.Push("InventoryCanvas");
            // todo : sync inventoryUI with inventory
            // inventoryUI.Init(inventory);
            Player.Instance.OnControllUI();
        }
    }
}
