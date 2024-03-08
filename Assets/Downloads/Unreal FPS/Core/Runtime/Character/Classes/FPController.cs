/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS.Runtime
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FPController : MonoBehaviour, IPlayerController, IController
    {
        /// <summary>
        /// Sprint directions.
        /// </summary>
        public enum SprintDirection
        {
            Free,
            ForwardOnly,
            ForwardWithSides
        }

        #region [Movement Variable]
        /* --- Speed / Steps /--- */
        [SerializeField] private float runSpeed = 5.0f;
        [SerializeField] private float runStepLength = 1.2f;
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float walkStepLength = 1.5f;
        [SerializeField] private float sprintSpeed = 7.0f;
        [SerializeField] private float sprintStepLength = 1.0f;
        [SerializeField] private float stepInterval = 5;
        [SerializeField] private SprintDirection sprintDirection = SprintDirection.ForwardOnly;

        /* --- Jump / Air / Gravity --- */
        [SerializeField] private float jumpForce = 5.0f;
        [SerializeField] private bool simulateJumpDirectionImpulse = true;
        [SerializeField] private float jumpDirectionImpulse = 1.0f;
        [SerializeField] private bool airControl = false;
        [SerializeField] private float airControlSpeed = 2.0f;
        [SerializeField] private float stickToGroundForce = 10.0f;
        [SerializeField] private float gravityMultiplier = 2.0f;

        /* --- Footstep --- */
        [SerializeField] private FootstepProperties footstepProperties;
        #endregion

        #region [Instances]
        [SerializeField] private Camera _FPCamera;
        [SerializeField] private FPCameraLook cameraLook;
        [SerializeField] private CameraFOVKick fovKickSystem = new CameraFOVKick();
        [SerializeField] private UCurveControlledBob curveControlledBobSystem = new UCurveControlledBob();
        [SerializeField] private ULerpControlledBob jumpBobSystem = new ULerpControlledBob();
        [SerializeField] private FPCrouch crouchSystem = new FPCrouch();
        [SerializeField] private FPGrab grabSystem = new FPGrab();
        [SerializeField] private CameraTilt cameraTilt = new CameraTilt();
        [SerializeField] private FPSimpleClimb climbSystem = new FPSimpleClimb();
        [SerializeField] private PickupItemSystem pickupItemSystem = new PickupItemSystem();
        private CharacterController characterController;
        #endregion

        #region [Other]
        [SerializeField] private bool fovKickIsEnabled = true;
        [SerializeField] private bool headBobIsEnabled = true;
        #endregion

        #region [Actions]
        private bool isJumping;
        private bool isWalking;
        private bool isRunning;
        private bool isSprinting;
        #endregion

        #region [Require fields]
        private float speed;
        private Vector2 input;
        private Vector3 moveDirection;
        private CollisionFlags collisionFlags;
        private bool previouslyGrounded;
        private Vector3 originalCameraPosition;
        private AudioSource audioSource;
        private Collider _collider;
        private bool sprintReturned;
        private float stepCycle;
        private float nextStep;
        #endregion

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            characterController = GetComponent<CharacterController>();
            originalCameraPosition = _FPCamera.transform.localPosition;
            stepCycle = 0f;
            nextStep = stepCycle / 2f;
            isJumping = false;
            moveDirection = Vector3.zero;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            IHealth health = GetComponent<IHealth>();
            IInventory inventory = GetComponent<IInventory>();

            fovKickSystem.Initialize(_FPCamera);
            curveControlledBobSystem.Initialize(_FPCamera);
            cameraLook.Initialize(transform, _FPCamera.transform);
            crouchSystem.Initialize(this, transform, characterController);
            grabSystem.Initialize(_FPCamera.transform, this, health, inventory);
            cameraTilt.Initialize(_FPCamera.transform, cameraLook);
            climbSystem.Initialize(transform, cameraLook, characterController, audioSource);
            pickupItemSystem.Initialize(inventory, health);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            cameraLook.RotationHandler();
            ProcessingSubSystems();
            ProcesingLanding();
            if (!characterController.isGrounded && !isJumping && previouslyGrounded)
                moveDirection.y = 0f;
            previouslyGrounded = characterController.isGrounded;
        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (!climbSystem.IsClimbing())
                CharacterMovementSystem();
        }

        /// <summary>
        /// Processing character movement.
        /// </summary>
        protected virtual void CharacterMovementSystem()
        {
            ReadInput();

            UpdateMovementStates(speed);

            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * input.y + transform.right * input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo,
                characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            moveDirection.x = desiredMove.x * speed;
            moveDirection.z = desiredMove.z * speed;

            if (characterController.isGrounded)
            {
                moveDirection.y = -stickToGroundForce;

                if (UInput.GetButtonDown(INC.JUMP) && !crouchSystem.IsCrouch())
                {
                    moveDirection.y = jumpForce;

                    if (simulateJumpDirectionImpulse)
                    {
                        if (IsIdle())
                            input = new Vector2(0, 1);
                        speed += jumpDirectionImpulse;
                    }

                    AudioClip jumpSound = GetJumpSound();
                    if (jumpSound != null)
                        audioSource.PlayOneShot(jumpSound);

                    nextStep = stepCycle + 0.5f;

                    isJumping = true;
                }
            }
            else
            {
                // Processing player world position by gravity law. 
                moveDirection += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
            }

            collisionFlags = characterController.Move(moveDirection * Time.fixedDeltaTime);

            ProgressStepCycle(speed);

            if (!cameraTilt.IsTilt())
            {
                HeadBobProcessing(speed);
            }

            FieldOfViewProcessing();
        }

        /// <summary>
        /// Processing subsystems.
        /// </summary>
        protected virtual void ProcessingSubSystems()
        {
            grabSystem.Update();
            cameraTilt.Update();
            if (characterController.isGrounded)
            {
                crouchSystem.Update();
            }
        }

        /// <summary>
        /// Procesing landing sound and landing effect.
        /// </summary>
        protected virtual void ProcesingLanding()
        {
            if (!previouslyGrounded && characterController.isGrounded && !crouchSystem.IsCrouch() && !climbSystem.IsClimbing())
            {
                StartCoroutine(jumpBobSystem.DoBobCycle());
                AudioClip landSound = GetLandSound();
                if (landSound != null)
                    audioSource.PlayOneShot(landSound);
                moveDirection.y = 0f;
                isJumping = false;
            }
        }

        /// <summary>
        /// Calculating player step cycle.
        /// </summary>
        /// <param name="speed"></param>
        protected virtual void ProgressStepCycle(float speed)
        {
            if (characterController.velocity.sqrMagnitude > 0 && (input.x != 0 || input.y != 0))
            {
                stepCycle += (characterController.velocity.magnitude + (speed * GetStepLength(speed))) *
                    Time.fixedDeltaTime;
            }

            if (!(stepCycle > nextStep))
            {
                return;
            }

            nextStep = stepCycle + stepInterval;

            AudioClip stepSound = GetStepSound();
            if (stepSound != null)
            {
                audioSource.PlayOneShot(stepSound);
            }
        }

        /// <summary>
        /// Processing camera position.
        /// </summary>
        /// <param name="speed"></param>
        private void HeadBobProcessing(float speed)
        {
            Vector3 newCameraPosition = originalCameraPosition;

            if (!headBobIsEnabled)
            {
                return;
            }
            if (characterController.velocity.magnitude > 0 && characterController.isGrounded)
            {
                Vector3 targetPosition = curveControlledBobSystem.DoHeadBob(characterController.velocity.magnitude + (speed * GetStepLength(speed)));
                _FPCamera.transform.localPosition = Vector3.Lerp(_FPCamera.transform.localPosition, targetPosition, cameraLook.GetSmoothTime() * Time.deltaTime);
                newCameraPosition = _FPCamera.transform.localPosition;
                newCameraPosition.y = _FPCamera.transform.localPosition.y - jumpBobSystem.GetOffset();
            }
            else
            {
                newCameraPosition = Vector3.Lerp(newCameraPosition, _FPCamera.transform.localPosition, cameraLook.GetSmoothTime() * Time.deltaTime);
                newCameraPosition.y = originalCameraPosition.y - jumpBobSystem.GetOffset();
            }
            _FPCamera.transform.localPosition = Vector3.Lerp(_FPCamera.transform.localPosition, newCameraPosition, cameraLook.GetSmoothTime() * Time.deltaTime);
        }

        private void FieldOfViewProcessing()
        {
            if (!fovKickIsEnabled)
                return;

            if (isSprinting && sprintReturned)
            {
                StopAllCoroutines();
                StartCoroutine(fovKickSystem.Increase());
                sprintReturned = false;
            }
            else if (!isSprinting && !sprintReturned)
            {
                StopAllCoroutines();
                StartCoroutine(fovKickSystem.Decrease());
                sprintReturned = true;
            }
        }

        /// <summary>
        /// Read input data value.
        /// </summary>
        /// <param name="speed"></param>
        private void ReadInput()
        {
            // Read input
            float horizontal = UInput.GetAxis(INC.CHAR_HORIZONTAL);
            float vertical = UInput.GetAxis(INC.CHAR_VERTICAL);

            if ((vertical == 0 && horizontal == 0) && characterController.isGrounded)
            {
                speed = 0;
                return;
            }
            else if (!characterController.isGrounded && !airControl)
            {
                speed = Mathf.SmoothStep(speed, 0, 5.0f * Time.deltaTime);
                return;
            }
            else if (!characterController.isGrounded && airControl)
            {
                speed = Mathf.SmoothStep(speed, airControlSpeed, 5.0f * Time.deltaTime);
                return;
            }

            if (crouchSystem.IsCrouch())
                speed = crouchSystem.GetSpeed();
            else if (UInput.GetButton(INC.WALK))
                speed = walkSpeed;
            else if (UInput.GetButton(INC.SPRINT) && IsSprintDirection(sprintDirection, vertical, horizontal))
                speed = sprintSpeed;
            else
                speed = runSpeed;

            input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (input.sqrMagnitude > 1)
                input.Normalize();
        }

        /// <summary>
        /// OnControllerColliderHit is called when the controller hits a
        /// collider while performing a Move.
        /// </summary>
        /// <param name="hit">The ControllerColliderHit data associated with this collision.</param>
        protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            //dont move the rigidbody if the player is on top of it
            if (collisionFlags == CollisionFlags.Below)
                return;

            if (body == null || body.isKinematic)
                return;

            body.AddForceAtPosition(characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        protected virtual void OnTriggerEnter(Collider other)
        {
            climbSystem.OnTriggerEnter(other);
            pickupItemSystem.OnTriggerEnter(other);
        }

        /// <summary>
        /// OnTriggerStay is called once per frame for every Collider other
        /// that is touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        protected virtual void OnTriggerStay(Collider other)
        {
            climbSystem.OnTriggerStay(other);
            pickupItemSystem.OnTriggerStay(other);
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        protected virtual void OnTriggerExit(Collider other)
        {
            climbSystem.OnTriggerExit(other);
            pickupItemSystem.OnTriggerExit(other);
        }

        /// <summary>
        /// Returning step sound by current surface on which is stand player.
        /// </summary>
        /// <returns>AudioClip: Step sound</returns>
        public virtual AudioClip GetStepSound()
        {
            if (footstepProperties == null || footstepProperties.GetLength() == 0)
                return null;

            RaycastHit footstepshit;
            if (Physics.Raycast(transform.position, Vector3.down, out footstepshit))
            {
                Object surfaceInfo = SurfaceHelper.GetSurfaceAuto(footstepshit.collider, transform.position);
                if (!surfaceInfo)
                    return null;

                for (int i = 0; i < footstepProperties.GetLength(); i++)
                {
                    if (footstepProperties.GetProperty(i).GetPhysicMaterial() == surfaceInfo || footstepProperties.GetProperty(i).GetTexture() == surfaceInfo)
                    {
                        if (footstepProperties.GetProperty(i).GetStepSoundsLength() > 0)
                        {
                            int randomSound = Random.Range(0, footstepProperties.GetProperty(i).GetStepSoundsLength());
                            return footstepProperties.GetProperty(i).GetStepSound(randomSound);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returning jump sound by current surface on which is stand player.
        /// </summary>
        /// <returns>AudioClip: Land sound</returns>
        public virtual AudioClip GetJumpSound()
        {
            if (footstepProperties == null || footstepProperties.GetLength() == 0)
                return null;

            RaycastHit footstepshit;
            if (Physics.Raycast(transform.position, Vector3.down, out footstepshit))
            {
                Object surfaceInfo = SurfaceHelper.GetSurfaceAuto(footstepshit.collider, transform.position);
                if (!surfaceInfo)
                    return null;

                for (int i = 0; i < footstepProperties.GetLength(); i++)
                {
                    if (footstepProperties.GetProperty(i).GetPhysicMaterial() == surfaceInfo || footstepProperties.GetProperty(i).GetTexture() == surfaceInfo)
                    {
                        if (footstepProperties.GetProperty(i).GetJumpSoundsLength() > 0)
                        {
                            int randomSound = Random.Range(0, footstepProperties.GetProperty(i).GetJumpSoundsLength());
                            return footstepProperties.GetProperty(i).GetJumpSound(randomSound);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returning land sound by current surface on which is stand player.
        /// </summary>
        /// <returns>AudioClip: Land sound</returns>
        public virtual AudioClip GetLandSound()
        {
            if (footstepProperties == null || footstepProperties.GetLength() == 0)
                return null;

            RaycastHit footstepshit;
            if (Physics.Raycast(transform.position, Vector3.down, out footstepshit))
            {
                Object surfaceInfo = SurfaceHelper.GetSurfaceAuto(footstepshit.collider, transform.position);
                if (!surfaceInfo)
                    return null;

                for (int i = 0; i < footstepProperties.GetLength(); i++)
                {
                    if (footstepProperties.GetProperty(i).GetPhysicMaterial() == surfaceInfo || footstepProperties.GetProperty(i).GetTexture() == surfaceInfo)
                    {
                        if (footstepProperties.GetProperty(i).GetLandSoundsLength() > 0)
                        {
                            int randomSound = Random.Range(0, footstepProperties.GetProperty(i).GetLandSoundsLength());
                            return footstepProperties.GetProperty(i).GetLandSound(randomSound);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Return step length value by current player speed.
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public virtual float GetStepLength(float speed)
        {
            if (speed == walkSpeed)
                return walkStepLength;
            else if (speed == runSpeed)
                return runStepLength;
            else if (speed == sprintSpeed)
                return sprintStepLength;
            return 1.0f;
        }

        /// <summary>
        /// Return permission for sprint by vertical and horizontal input data.
        /// </summary>
        /// <param name="sprintDirection"></param>
        /// <param name="vertical"></param>
        /// <param name="horizontal"></param>
        /// <returns></returns>
        public bool IsSprintDirection(SprintDirection sprintDirection, float vertical, float horizontal)
        {
            if (sprintDirection == SprintDirection.Free)
                return true;
            else if (sprintDirection == SprintDirection.ForwardOnly && (vertical > 0 && horizontal == 0))
                return true;
            else if (sprintDirection == SprintDirection.ForwardWithSides && !(vertical < 0))
                return true;
            return false;
        }

        protected virtual void UpdateMovementStates(float speed)
        {
            isWalking = speed == walkSpeed;
            isRunning = speed == runSpeed;
            isSprinting = speed == sprintSpeed;
        }

        #region [Getter / Setter]

        public Vector3 GetVelocity()
        {
            return characterController.velocity;
        }

        public void ControllerEnabled(bool enabled)
        {
            characterController.enabled = enabled;
        }

        public bool IsIdle()
        {
            return speed == 0;
        }

        public float GetRunSpeed()
        {
            return runSpeed;
        }

        public void SetRunSpeed(float value)
        {
            runSpeed = value;
        }

        public float GetRunStepLength()
        {
            return runStepLength;
        }

        public void SetRunStepLength(float value)
        {
            runStepLength = value;
        }

        public float GetWalkSpeed()
        {
            return walkSpeed;
        }

        public void SetWalkSpeed(float value)
        {
            walkSpeed = value;
        }

        public float GetWalkStepLength()
        {
            return walkStepLength;
        }

        public void SetWalkStepLength(float value)
        {
            walkStepLength = value;
        }

        public float GetSprintSpeed()
        {
            return sprintSpeed;
        }

        public void SetSprintSpeed(float value)
        {
            sprintSpeed = value;
        }

        public float GetSprintStepLength()
        {
            return sprintStepLength;
        }

        public void SetSprintStepLength(float value)
        {
            sprintStepLength = value;
        }

        public float GetStepInterval()
        {
            return stepInterval;
        }

        public void SetStepInterval(float value)
        {
            stepInterval = value;
        }

        public SprintDirection GetSprintDirection()
        {
            return sprintDirection;
        }

        public void SetSprintDirection(SprintDirection value)
        {
            sprintDirection = value;
        }

        public float GetJumpForce()
        {
            return jumpForce;
        }

        public void SetJumpForce(float value)
        {
            jumpForce = value;
        }

        public bool GetSimulateJumpDirectionImpulse()
        {
            return simulateJumpDirectionImpulse;
        }

        public void SetSimulateJumpDirectionImpulse(bool value)
        {
            simulateJumpDirectionImpulse = value;
        }

        public float GetJumpDirectionImpulse()
        {
            return jumpDirectionImpulse;
        }

        public void SetJumpDirectionImpulse(float value)
        {
            jumpDirectionImpulse = value;
        }

        public bool GetAirControl()
        {
            return airControl;
        }

        public void SetAirControl(bool value)
        {
            airControl = value;
        }

        public float GetAirControlSpeed()
        {
            return airControlSpeed;
        }

        public void SetAirControlSpeed(float value)
        {
            airControlSpeed = value;
        }

        public float GetStickToGroundForce()
        {
            return stickToGroundForce;
        }

        public void SetStickToGroundForce(float value)
        {
            stickToGroundForce = value;
        }

        public float GetGravityMultiplier()
        {
            return gravityMultiplier;
        }

        public void SetGravityMultiplier(float value)
        {
            gravityMultiplier = value;
        }

        public FootstepProperties GetFootstepProperties()
        {
            return footstepProperties;
        }

        public void SetFootstepProperties(FootstepProperties value)
        {
            footstepProperties = value;
        }

        public Camera GetFPCamera()
        {
            return _FPCamera;
        }

        public void SetFPCamera(Camera value)
        {
            _FPCamera = value;
        }

        public FPCameraLook GetCameraLook()
        {
            return cameraLook;
        }

        protected void SetCameraLook(FPCameraLook cameraLook)
        {
            this.cameraLook = cameraLook;
        }

        public CameraFOVKick GetFovKickSystem()
        {
            return fovKickSystem;
        }

        public UCurveControlledBob GetCurveControlledBobSystem()
        {
            return curveControlledBobSystem;
        }

        public ULerpControlledBob GetJumpBobSystem()
        {
            return jumpBobSystem;
        }

        public FPCrouch GetCrouchSystem()
        {
            return crouchSystem;
        }

        public FPGrab GetGrabSystem()
        {
            return grabSystem;
        }

        public CameraTilt GetCameraTilt()
        {
            return cameraTilt;
        }

        public FPSimpleClimb GetClimbSystem()
        {
            return climbSystem;
        }

        public PickupItemSystem GetPickupItemSystem()
        {
            return pickupItemSystem;
        }

        public CharacterController GetCharacterController()
        {
            return characterController;
        }

        public bool FovKickIsEnabled()
        {
            return fovKickIsEnabled;
        }

        public void SetFovKickEnabled(bool value)
        {
            fovKickIsEnabled = value;
        }

        public bool HeadBobIsEnabled()
        {
            return headBobIsEnabled;
        }

        public void SetHeadBobEnabled(bool value)
        {
            headBobIsEnabled = value;
        }

        public bool IsJumping()
        {
            return isJumping;
        }

        public bool IsWalking()
        {
            return isWalking;
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public bool IsSprinting()
        {
            return isSprinting;
        }

        public bool IsGrounded()
        {
            return characterController.isGrounded;
        }
        #endregion
    }
}