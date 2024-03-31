using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public bool isJump = true;
    public Inventory inventory;
    public float MovementSpeed { get; private set; }
    public float MovementSpeedModifier { get; set; }
    public float JumpCoolTime = 1.0f;

    [field: Header("Camera")]
    public Camera FPCamera { get; private set; }
    public Transform playerCamTransform;
    public float camXRotate = 0f;

    [field: Header("Camera")]
    public Transform firePos;
    public float fireRateDelay;

    [SerializeField] public Weapon curWeapon;
    private WeaponStatHandler weaponStatHandler;

    public Action<PlayerStateMachine> SetWeaponEvent;

    [SerializeField]
    private Weapon primaryWeapon;
    [SerializeField]
    private Weapon secondaryWeapon;

    public Dictionary<int, float> AnimHashFloats = new Dictionary<int, float>();
    //public Action<PlayerStateMachine> SetWeaponEvent;

    //UI
    public PlayerUI playerUI;

    //Coroutine
    IEnumerator JumpCoolTimeCoroutine;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine(this);
        rigidbody_ = GetComponent<Rigidbody>();
        dungeonInteract = GetComponent<DungeonInteract>();
        AnimationData = new PlayerAnimationData();
        inventory = GetComponent<Inventory>();
        playerCamTransform = transform.Find("FPCamera");
        FPCamera = playerCamTransform.GetComponent<Camera>();

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

    public void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        float movementSpeed = GetMovementSpeed();
        GetComponent<Rigidbody>().MovePosition(transform.position + (movementDirection * movementSpeed * Time.deltaTime));
    }
    //InputAction Event
    public void Rotate(InputAction.CallbackContext callbackContext)
    {
        Vector2 rotateDirection = callbackContext.ReadValue<Vector2>();

        PlayerCharacter player = stateMachine.player;
        PlayerData SOData = player.Data;
        Transform camTransform = player.playerCamTransform;
        Rigidbody rigidbody = stateMachine.player.rigidbody_;


        player.camXRotate += rotateDirection.y * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime * -1;
        player.camXRotate = Mathf.Clamp(player.camXRotate, -SOData.UpdownMaxAngle, SOData.UpdownMaxAngle);
        stateMachine.playerYRotate += rotateDirection.x * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime;

        camTransform.localRotation = Quaternion.Euler(new Vector3(player.camXRotate - player.curWeapon.curRecoil, 0, 0));
        rigidbody.transform.rotation = Quaternion.Euler(new Vector3(0, stateMachine.playerYRotate, 0));
    }
    public void Rotate()
    {
        PlayerCharacter player = stateMachine.player;
        Vector2 rotateDirection = player.input.playerActions.Look.ReadValue<Vector2>();

        PlayerData SOData = player.Data;
        Transform camTransform = player.playerCamTransform;
        Rigidbody rigidbody = player.rigidbody_;

        player.camXRotate += rotateDirection.y * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime * -1;
        player.camXRotate = Mathf.Clamp(player.camXRotate, -SOData.UpdownMaxAngle, SOData.UpdownMaxAngle);
        stateMachine.playerYRotate += rotateDirection.x * (SOData.LookRotateSpeed * SOData.LookRotateModifier) * Time.deltaTime;

        camTransform.localRotation = Quaternion.Euler(new Vector3(player.camXRotate - player.curWeapon.curRecoil, 0, 0));
        rigidbody.transform.rotation = Quaternion.Euler(new Vector3(0, stateMachine.playerYRotate, 0));
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
    public void MoveLerpAnimation(int ParameterHash, float setFloat)
    {
        if (!AnimHashFloats.ContainsKey(ParameterHash)) AnimHashFloats.Add(ParameterHash, 0);
        AnimHashFloats[ParameterHash] = math.lerp(AnimHashFloats[ParameterHash], setFloat, 0.1f);
        stateMachine.player.animator.SetFloat(ParameterHash, AnimHashFloats[ParameterHash]);
    }
    public void EquipWeapon(Weapon weapon)
    {
        weaponStatHandler.EquipWeapon(weapon);
        curWeapon = weapon;
        curWeapon.Init(this);
    }

    public void UnequipWeapon(Weapon weapon)
    {
        weaponStatHandler.UnequipWeapon();
        curWeapon.stateMachine.currentState.RemoveInputActionsCallbacks();
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
}
