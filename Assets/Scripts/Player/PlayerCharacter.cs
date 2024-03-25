using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }
    [HideInInspector]
    public PlayerInput input;
    [field: SerializeField] public PlayerData Data { get; private set; }
    public Rigidbody rigidbody_ { get; private set; }
    public Animator animator { get; private set; }
    public PlayerAnimationData AnimationData { get; private set; }
    public DungeonInteract dungeonInteract;
    [field: SerializeField] public LayerMask layerMask_GroundCheck;
    public bool isGrounded = true;
    public Inventory inventory;

    //Weapon
    public Transform firePos;
    public float fireRateDelay;

    [SerializeField] public Weapon curWeapon;
    private WeaponStatHandler weaponStatHandler;

    public Action<PlayerStateMachine> SetWeaponEvent;

    [SerializeField]
    private Weapon primaryWeapon;
    [SerializeField]
    private Weapon secondaryWeapon;

    //public Action<PlayerStateMachine> SetWeaponEvent;

    //UI
    public PlayerUI playerUI;


    private void Awake()
    {
        stateMachine = new PlayerStateMachine(this);
        rigidbody_ = GetComponent<Rigidbody>();
        dungeonInteract = GetComponent<DungeonInteract>();
        AnimationData = new PlayerAnimationData();
        inventory = GetComponent<Inventory>();

        if (!TryGetComponent(out weaponStatHandler)) Debug.Log("WeaponStatHandler : weaponStatHandler is not Found!");
        //UI
        if (!TryGetComponent<PlayerUI>(out playerUI)) Debug.Log("Player : PlayerUI is not Found!");
        else
        {
            playerUI.InitSetting();
            stateMachine.playerUIEvent += playerUI.UITextUpdate;
        }

        Animator[] anim_temp = transform.GetComponentsInChildren<Animator>();
        foreach (Animator anim in anim_temp)
        {
            if (anim.gameObject.activeSelf && !anim.CompareTag("Weapon"))
                animator = anim;
        }
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        AnimationData.Initialize();
    }

    public void OnPossessCharacter() 
    {
        if (stateMachine.currentState == null)
            stateMachine.ChangeState(stateMachine.IdleState);
        stateMachine.currentState.AddInputActionsCallbacks();

        if (curWeapon == null)
        {
            if (primaryWeapon != null)
                EquipWeapon(primaryWeapon);
            else if (secondaryWeapon != null)
                EquipWeapon(secondaryWeapon);
            else
            {
                // todo : 무기 없을 경우에 주먹?
            }
        }
    }

    public void OnUnpossessCharacter() 
    {
        stateMachine.currentState.RemoveInputActionsCallbacks();
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

    public void EquipWeapon(Weapon weapon)
    {
        weaponStatHandler.EquipWeapon(weapon);
        curWeapon = weapon;
        curWeapon.input_ = this.input;
        curWeapon.WeaponSet();
        curWeapon.CurrentWeaponEquip();
        curWeapon.GetStateMachine(stateMachine);
        curWeapon.stateMachine.currentState.AddInputActionsCallbacks();
    }

    public void UnequipWeapon(Weapon weapon)
    {
        weaponStatHandler.UnequipWeapon();
        curWeapon.stateMachine.currentState.RemoveInputActionsCallbacks();
        curWeapon.input_ = null;
        curWeapon = null;
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
