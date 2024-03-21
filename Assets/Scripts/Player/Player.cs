using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    // �κ��丮
    public Inventory inventory;

    //�ӽú���
    public Transform firePos;
    public float fireRateDelay;
    [SerializeField] public Weapon curWeapon;
    public Action<PlayerStateMachine> SetWeaponEvent;


    private void Awake()
    {
        stateMachine = new PlayerStateMachine(this);
        rigidbody_ = GetComponent<Rigidbody>();
        input_ = GetComponent<PlayerInput>();
        dungeonInteract = GetComponent<DungeonInteract>();
        AnimationData = new PlayerAnimationData();
        // �κ��丮
        inventory = GetComponent<Inventory>();

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
        curWeapon.WeaponSet();
        curWeapon.CurrentWeaponEquip();
        SetWeaponEvent += curWeapon.GetStateMachine;
        SetWeaponEvent.Invoke(stateMachine);
        //DungeonManager.instance.LoadSceneEvent += ObjectListClear;
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
        curWeapon.weaponProjectile_List.Clear();
    }
}
