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

    // 인벤토리
    public Inventory inventory;

    //임시변수
    public Transform firePos;
    public float fireRateDelay;
    public AmmoProjectile ammoProjectile;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine(this);
        rigidbody_ = GetComponent<Rigidbody>();
        input_ = GetComponent<PlayerInput>();
        dungeonInteract = GetComponent<DungeonInteract>();
        AnimationData = new PlayerAnimationData();
        // 인벤토리
        inventory = GetComponent<Inventory>();

        Animator[] anim_temp = transform.GetComponentsInChildren<Animator>();
        foreach(Animator anim in anim_temp)
        {
            if (anim.gameObject.activeSelf)
                animator = anim;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
        AnimationData.Initialize();
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

    //public AmmoProjectile CreateObject(List<AmmoProjectile> pooling_List,AmmoProjectile obj)
    //{
    //    AmmoProjectile newProjectile = Instantiate(obj, firePos.position, Quaternion.LookRotation(-firePos.forward)).GetComponent<AmmoProjectile>();
    //    pooling_List.Add(newProjectile);
    //    return newProjectile;
    //}
}
