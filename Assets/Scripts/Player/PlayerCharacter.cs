using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerCharacter : CharacterStatHandler
{
    public Player ownedPlayer { get; private set; }
    public PlayerInput input { get; private set; }

    public PlayerStateMachine stateMachine { get; private set; }
    [HideInInspector]
    [field: SerializeField] public PlayerData Data { get; private set; }
    public GameObject fpsBody { get; private set; }
    public GameObject fullBody { get; private set; }
    public Rigidbody rigidbody_ { get; private set; }
    public Animator animator { get; private set; }
    public PlayerAnimationData AnimationData { get; private set; }
    public Health health { get; private set; }
    [field: SerializeField] public LayerMask layerMask_GroundCheck;
    public bool isGrounded = true;
    public bool isJump = true;
    public float MovementSpeed { get; private set; }
    public float MovementSpeedModifier { get; set; }
    public float JumpCoolTime = 1.0f;
    private Vector3 jumpDirection;
    private float jumpSpeed;
    [field: Header("Camera")]
    public Camera FPCamera { get; private set; }
    public Transform playerCamTransform;
    public float camXRotate = 0f;

    [field: Header("Weapon")]
    public Transform firePos;
    public float fireRateDelay;
    [field: SerializeField]
    public Weapon curWeapon { get { return _curWeapon; } set { OnWeaponChanged?.Invoke(value); _curWeapon = value; } }
    Weapon _curWeapon;
    public AmmoType curWeapon_AmmoType
    {
        get
        {
            if(curWeapon)
            {
                return curWeapon.baseStat.e_useAmmo;
            }
            return AmmoType.None;
        }
    }
    public WeaponStatHandler weaponStatHandler;

    public Action<PlayerStateMachine> SetWeaponEvent;
    public Action<Weapon> OnWeaponChanged;
    [SerializeField]
    public Weapon primaryWeapon;
    [SerializeField]
    public Weapon secondaryWeapon;
    public bool isPrimary = true;

    public Dictionary<int, float> AnimHashFloats = new Dictionary<int, float>();
    //public Action<PlayerStateMachine> SetWeaponEvent;

    //Coroutine
    IEnumerator JumpCoolTimeCoroutine;
    IEnumerator SwapCoroutine = null;

    private void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine(this);
        rigidbody_ = GetComponent<Rigidbody>();
        AnimationData = new PlayerAnimationData();
        playerCamTransform = transform.Find("FPCamera");
        fpsBody = transform.Find("FPSBody").gameObject;
        fullBody = transform.Find("FullBody").gameObject;
        FPCamera = playerCamTransform.GetComponent<Camera>();
        health = GetComponent<Health>();

        if (!TryGetComponent(out weaponStatHandler)) Debug.Log("WeaponStatHandler : weaponStatHandler is not Found!");

        Animator[] anim_temp = transform.GetComponentsInChildren<Animator>();
        foreach (Animator anim in anim_temp)
        {
            if (anim.gameObject.activeSelf && !anim.CompareTag("Weapon"))
                animator = anim;
        }

        MovementSpeed = Data.groundData.BaseSpeed;
        MovementSpeedModifier = Data.groundData.WalkSpeedModifier;
    }

    private void Start()
    {
        base.Start();
        stateMachine.ChangeState(stateMachine.IdleState);
        health.OnDie += Death;
        AnimationData.Initialize();
    }

    public void OnPossessCharacter(Player player)
    {
        input = player._input;
        ownedPlayer = player;

        if (stateMachine.currentState == null)
            stateMachine.ChangeState(stateMachine.IdleState);

        if (primaryWeapon) primaryWeapon.Init(this);
        if (secondaryWeapon) secondaryWeapon.Init(this);
        

        if (curWeapon == null)
        {
            if (primaryWeapon != null)
            {
                EquipWeapon(primaryWeapon);
            }
            else if (secondaryWeapon != null)
            {
                EquipWeapon(secondaryWeapon);
            }
            else
            {
                EquipWeapon(null);
            }
        }
    }

    public void OnUnpossessCharacter() 
    {
        stateMachine.currentState.RemoveInputActionsCallbacks();
        input = null;
        stateMachine.ChangeState(null);
        curWeapon?.stateMachine.currentState.RemoveInputActionsCallbacks();
        curWeapon.input_ = null;
        ownedPlayer = null;
    }

    private void Update()
    {
        if(input != null)
            stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        float movementSpeed = GetMovementSpeed();
        rigidbody_.MovePosition(transform.position + (movementDirection * movementSpeed * currentStat.moveSpeed * Time.fixedDeltaTime));
    }
    public void JumpMove()
    {
        rigidbody_.MovePosition(transform.position + (jumpDirection * jumpSpeed * Time.fixedDeltaTime));
    }
    public void JumpMoveSetting()
    {
        jumpDirection = stateMachine.player.GetMovementDirection();
        jumpSpeed = stateMachine.player.GetMovementSpeed();
    }

    public void Rotate()
    {
        Vector2 rotateDirection = input.playerActions.Look.ReadValue<Vector2>();
        float recoil = curWeapon ? curWeapon.curRecoil : 0f;

        camXRotate += rotateDirection.y * (Data.LookRotateSpeed * Data.LookRotateModifier) * Time.deltaTime * -1;
        camXRotate = Mathf.Clamp(camXRotate, -Data.UpdownMaxAngle, Data.UpdownMaxAngle);
        stateMachine.playerYRotate += rotateDirection.x * (Data.LookRotateSpeed * Data.LookRotateModifier) * Time.deltaTime;

        playerCamTransform.localRotation = Quaternion.Euler(new Vector3(camXRotate - recoil, 0, 0));
        rigidbody_.transform.rotation = Quaternion.Euler(new Vector3(0, stateMachine.playerYRotate, 0));
    }
    public void WeaponSwap()
    {
        Weapon swappingWeapon = isPrimary ? secondaryWeapon : primaryWeapon;
        bool curWeaponSwapping = curWeapon?.isSwap == true;
        if(swappingWeapon != null && SwapCoroutine == null && !curWeaponSwapping)
        {
            SwapCoroutine = Swapping();
            StartCoroutine(SwapCoroutine);
        }
    }
    public void Jump()
    {
        float jumpForce = Data.airData.JumpForce * Data.airData.JumpForceModifier;
        rigidbody_.velocity = new Vector3(0, jumpForce, 0);
        isGrounded = false;
        isJump = false;
    }

    public bool Falling()
    {
        Vector3 origin = transform.position;
        RaycastHit hit;
        LayerMask layerMask = layerMask_GroundCheck;
        float RayDistance = Data.airData.GroundCheckRay_Distance;

        Debug.DrawRay(origin, Vector3.down, Color.red, RayDistance);
        if (Physics.Raycast(new Ray(origin, Vector3.down), out hit, RayDistance, layerMask) && !isGrounded)
        {
            isGrounded = true;
            if (JumpCoolTimeCoroutine == null)
            {
                JumpCoolTimeCoroutine = OnJumpCoolTime();
                StartCoroutine(JumpCoolTimeCoroutine);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Death()
    {
        input.playerUIActions.Inventory.started -= Player.Instance.ToggleInventory;

        Debug.Log("PlayerDie");

        fpsBody.SetActive(false);
        fullBody.SetActive(true);

        animator = fullBody.GetComponent<Animator>();

        stateMachine.ChangeState(stateMachine.DeadState);

        ownedPlayer.OnControllUI();
        ownedPlayer.UnPossess();
        curWeapon.gameObject.SetActive(false);

        UIController.Instance.Clear();
        UIController.Instance.Push("DungeonFailedUI");
    }
    public void MoveLerpAnimation(int ParameterHash, float setFloat)
    {
        if (!AnimHashFloats.ContainsKey(ParameterHash)) AnimHashFloats.Add(ParameterHash, 0);
        AnimHashFloats[ParameterHash] = math.lerp(AnimHashFloats[ParameterHash], setFloat, 0.1f);
        stateMachine.player.animator.SetFloat(ParameterHash, AnimHashFloats[ParameterHash]);
    }
    public bool InventoryWeaponEquip(WeaponStat _weaponStat)
    {
        Weapon weapon;
        switch (_weaponStat.e_useAmmo)
        {
            case AmmoType.Rifle:
                playerCamTransform.Find("Assault Rifle [FP]").TryGetComponent<Weapon>(out weapon);
                break;
            case AmmoType.Pistol:
                playerCamTransform.Find("Pistol [FP]").TryGetComponent<Weapon>(out weapon);
                break;
            default:
                weapon = null;
                break;
        }

        if (weapon == null) return false;

        if (primaryWeapon == null)
        {
            primaryWeapon = weapon;
            primaryWeapon.Init(this);
        }
        else if (secondaryWeapon == null)
        {
            secondaryWeapon = weapon;
            secondaryWeapon.Init(this);
        }
        else return false;

        if (curWeapon == null)
        {
            EquipWeapon(weapon);
        }
        return true;
    }
    public void InventoryWeaponUnequip(bool _isPrimary)
    {
        if (_isPrimary)
        {
            if(curWeapon == primaryWeapon)
            {
                UnequipWeapon(primaryWeapon);
                //EquipWeapon(emptyWeapon);
                curWeapon = null;
            }
            primaryWeapon = null;
        }
        else
        {
            if(curWeapon == secondaryWeapon)
            {
                UnequipWeapon(secondaryWeapon);
                //EquipWeapon(emptyWeapon);
                curWeapon = null;
            }
            secondaryWeapon = null;
        }
    }
    public void EquipWeapon(Weapon weapon)
    {
        weapon.gameObject.SetActive(true);
        weaponStatHandler.EquipWeapon(weapon);
        curWeapon = weapon;
        curWeapon.CurrentWeaponEquip();
    }

    public void UnequipWeapon(Weapon weapon)
    {
        if (weapon.ReloadCoroutine != null)
        {
            weapon.StopAction(ref weapon.ReloadCoroutine);
        }

        weaponStatHandler?.UnequipWeapon();
        //curWeapon.stateMachine.currentState.RemoveInputActionsCallbacks();
        weapon.CurrentWeaponUnEquip();
        weapon.input_ = null;
    }
    public float GetMovementSpeed()
    {
        float moveSpeed = MovementSpeed * MovementSpeedModifier;
        return moveSpeed;
    }

    public Vector3 GetMovementDirection()
    {
        Vector3 forward = playerCamTransform.forward;
        Vector3 right = playerCamTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }
    public IEnumerator OnJumpCoolTime()
    {
        yield return new WaitForSeconds(JumpCoolTime);
        isJump = true;
        JumpCoolTimeCoroutine = null;
    }
    public IEnumerator Swapping()
    {
        if (curWeapon)
        {
            UnequipWeapon(curWeapon);
            while (curWeapon.gameObject.activeSelf)
            {
                yield return null;
            }
        }
        if (isPrimary) EquipWeapon(secondaryWeapon);
        else EquipWeapon(primaryWeapon);
        
        yield return YieldCacher.WaitForSeconds(curWeapon.animator.GetCurrentAnimatorStateInfo(0).length);
        isPrimary = !isPrimary;
        SwapCoroutine = null;
    }
}
