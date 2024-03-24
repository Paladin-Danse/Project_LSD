using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerInput input_ { get; private set; }
    [field: SerializeField] public PlayerData Data { get; private set; }
    public Rigidbody rigidbody_ { get; private set; }
    [SerializeField] public Animator animator { get; private set; }
    public PlayerAnimationData AnimationData {get; private set; }
    public DungeonInteract dungeonInteract;
    [field: SerializeField] public LayerMask layerMask_GroundCheck;
    public bool isGrounded = true;
    //Inventory
    public Inventory inventory;

    //Weapon
    public Transform firePos;
    public float fireRateDelay;
    [SerializeField] public Weapon curWeapon;
    //public Action<PlayerStateMachine> SetWeaponEvent;

    //UI
    public PlayerUI playerUI;
    

    private void Awake()
    {
        stateMachine = new PlayerStateMachine(this);
        rigidbody_ = GetComponent<Rigidbody>();
        input_ = GetComponent<PlayerInput>();
        dungeonInteract = GetComponent<DungeonInteract>();
        AnimationData = new PlayerAnimationData();
        //Inventory
        inventory = GetComponent<Inventory>();

        //UI
        if (!TryGetComponent<PlayerUI>(out playerUI)) Debug.Log("Player : PlayerUI is not Found!");
        else
        {
            playerUI.InitSetting();
            stateMachine.playerUIEvent += playerUI.UITextUpdate;
        }

        Animator[] anim_temp = transform.GetComponentsInChildren<Animator>();
        foreach(Animator anim in anim_temp)
        {
            if (anim.gameObject.activeSelf && !anim.CompareTag("Weapon"))
                animator = anim;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
        AnimationData.Initialize();
        curWeapon.CurrentWeaponEquip();
        curWeapon.GetStateMachine(stateMachine);

        playerUIEventInvoke();
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public void ObjectListClear()
    {
        foreach (AmmoProjectile ammoProjectile in curWeapon.weaponProjectile_List)
            Destroy(ammoProjectile.gameObject);
        curWeapon.weaponProjectile_List.Clear();
    }

    public void playerUIEventInvoke()
    {
        if (playerUI) stateMachine.playerUIEvent(this);
    }
}
