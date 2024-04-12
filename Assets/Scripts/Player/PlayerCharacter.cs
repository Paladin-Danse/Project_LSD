using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : CharacterStatHandler
{
    public PlayerStateMachine stateMachine { get; private set; }
    [HideInInspector]
    public PlayerInput input;
    [field: SerializeField] public PlayerData Data { get; private set; }
    public GameObject fpsBody { get; private set; }
    public GameObject fullBody { get; private set; }
    public Rigidbody rigidbody_ { get; private set; }
    public Animator animator { get; private set; }
    public PlayerAnimationData AnimationData { get; private set; }
    public Health health { get; private set; }
    public DungeonInteract dungeonInteract;
    [field: SerializeField] public LayerMask layerMask_GroundCheck;
    public bool isGrounded = true;
    public bool isJump = true;
    //public Inventory inventory;
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
    [SerializeField] public Weapon curWeapon;
    private WeaponStatHandler weaponStatHandler;
    public Action<PlayerStateMachine> SetWeaponEvent;
    
    public Dictionary<int, float> AnimHashFloats = new Dictionary<int, float>();
    //public Action<PlayerStateMachine> SetWeaponEvent;

    //UI
    public PlayerUI playerUI;

    //Coroutine
    IEnumerator JumpCoolTimeCoroutine;
    IEnumerator SwapCoroutine = null;

    //차후 삭제 변수
    [SerializeField]
    public Weapon primaryWeapon;
    [SerializeField]
    public Weapon secondaryWeapon;

    private void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine(this);
        rigidbody_ = GetComponent<Rigidbody>();
        dungeonInteract = GetComponent<DungeonInteract>();
        AnimationData = new PlayerAnimationData();
        //inventory = GetComponent<Inventory>();
        playerCamTransform = transform.Find("FPCamera");
        fpsBody = transform.Find("FPSBody").gameObject;
        fullBody = transform.Find("FullBody").gameObject;
        FPCamera = playerCamTransform.GetComponent<Camera>();
        health = GetComponent<Health>();

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

        MovementSpeed = Data.groundData.BaseSpeed;
        MovementSpeedModifier = Data.groundData.WalkSpeedModifier;
    }

    private void Start()
    {
        base.Start();
        //Instantiate(inventory.inventoryWindow).SetActive(false);
        stateMachine.ChangeState(stateMachine.IdleState);
        AnimationData.Initialize();
    }

    public void OnPossessCharacter()
    {
        if (stateMachine.currentState == null)
            stateMachine.ChangeState(stateMachine.IdleState);
        stateMachine.currentState.AddInputActionsCallbacks();

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
                // todo : 무기 없을 경우에 주먹?
            }
        }
    }

    public void OnUnpossessCharacter() 
    {
        stateMachine.currentState.RemoveInputActionsCallbacks();
        curWeapon.input_ = null;
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
    //InputAction Event
    public void Rotate(InputAction.CallbackContext callbackContext)
    {
        Vector2 rotateDirection = callbackContext.ReadValue<Vector2>();
        float recoil = curWeapon ? curWeapon.curRecoil : 0f;

        camXRotate += rotateDirection.y * (Data.LookRotateSpeed * Data.LookRotateModifier) * Time.deltaTime * -1;
        camXRotate = Mathf.Clamp(camXRotate, -Data.UpdownMaxAngle, Data.UpdownMaxAngle);
        stateMachine.playerYRotate += rotateDirection.x * (Data.LookRotateSpeed * Data.LookRotateModifier) * Time.deltaTime;

        playerCamTransform.localRotation = Quaternion.Euler(new Vector3(camXRotate - recoil, 0, 0));
        rigidbody_.transform.rotation = Quaternion.Euler(new Vector3(0, stateMachine.playerYRotate, 0));
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
        if(primaryWeapon != null && secondaryWeapon != null && SwapCoroutine == null && !curWeapon.isSwap)
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
        input = null;
        curWeapon.input_ = null;
        curWeapon.gameObject.SetActive(false);
        curWeapon = null;

        fpsBody.SetActive(false);
        fullBody.SetActive(true);

        animator = fullBody.GetComponent<Animator>();
    }
    public void MoveLerpAnimation(int ParameterHash, float setFloat)
    {
        if (!AnimHashFloats.ContainsKey(ParameterHash)) AnimHashFloats.Add(ParameterHash, 0);
        AnimHashFloats[ParameterHash] = math.lerp(AnimHashFloats[ParameterHash], setFloat, 0.1f);
        stateMachine.player.animator.SetFloat(ParameterHash, AnimHashFloats[ParameterHash]);
    }
    public void InventoryWeaponEquip(Weapon weapon)
    {
        if(primaryWeapon == null)
        {
            primaryWeapon = weapon;
            primaryWeapon.Init(this);
        }
        else if(secondaryWeapon == null)
        {
            secondaryWeapon = weapon;
            secondaryWeapon.Init(this);
        }

        if (curWeapon == null)
        {
            EquipWeapon(weapon);
        }
    }
    public void InventoryWeaponUnequip(bool isPrimary)
    {
        if (isPrimary)
        {
            if(curWeapon == primaryWeapon)
            {
                UnequipWeapon(primaryWeapon);
            }
            Player.Instance.inventory.AddItem(primaryWeapon.itemData);
            primaryWeapon = null;
        }
        else
        {
            if(curWeapon == secondaryWeapon)
            {
                UnequipWeapon(secondaryWeapon);
            }
            Player.Instance.inventory.AddItem(secondaryWeapon.itemData);
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

        weaponStatHandler.UnequipWeapon();
        //curWeapon.stateMachine.currentState.RemoveInputActionsCallbacks();
        curWeapon.CurrentWeaponUnEquip();
        curWeapon.input_ = null;
        curWeapon = null;
    }

    public void playerUIEventInvoke()
    {
        if (playerUI) stateMachine.playerUIEvent(this);
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
        Weapon beforeWeapon = curWeapon;

        UnequipWeapon(curWeapon);
        while(beforeWeapon.gameObject.activeSelf)
        {
            yield return null;
        }
        if (beforeWeapon == primaryWeapon) EquipWeapon(secondaryWeapon);
        else EquipWeapon(primaryWeapon);

        yield return YieldCacher.WaitForSeconds(curWeapon.animator.GetCurrentAnimatorStateInfo(0).length);
        SwapCoroutine = null;
    }
}
