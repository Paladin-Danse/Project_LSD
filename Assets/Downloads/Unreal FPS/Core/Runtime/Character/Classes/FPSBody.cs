/* =====================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnrealFPS.Utility;

namespace UnrealFPS.Runtime
{
    [RequireComponent(typeof(Animator))]
    public class FPSBody : MonoBehaviour
    {
        public const string SPEED = "Speed";
        public const string DIRECTION = "Direction";
        public const string CROUCH = "IsCrouching";
        public const string GROUNDED = "IsGrounded";

        public readonly static int SPEED_HASH = Animator.StringToHash(SPEED);
        public readonly static int DIRECTION_HASH = Animator.StringToHash(DIRECTION);
        public readonly static int CROUCH_HASH = Animator.StringToHash(CROUCH);
        public readonly static int GROUNDED_HASH = Animator.StringToHash(GROUNDED);

        [SerializeField] private FPController controller;

        [SerializeField] private float stayBodyHeight = -1.55f;
        [SerializeField] private float crouchBodyHeight = -0.55f;
        [SerializeField] private float moveHeightsmooth = 7.0f;

        private Animator animator;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// Called only once during the lifetime of the script instance and after all objects are initialized
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            UpdateAnimationParameters();
            ProcessingBodyHeight();
        }

        /// <summary>
        /// Updating FPS body animation parameters.
        /// </summary>
        protected virtual void UpdateAnimationParameters()
        {
            if (animator.GetFloat(SPEED_HASH) != GetFixedSpeed())
                animator.SetFloat(SPEED_HASH, GetFixedSpeed());
            if (animator.GetFloat(DIRECTION_HASH) != GetFixedSpeed())
                animator.SetFloat(DIRECTION_HASH, GetFixedDirection());
            if (animator.GetBool(CROUCH_HASH) != controller.GetCrouchSystem().IsCrouch())
                animator.SetBool(CROUCH_HASH, controller.GetCrouchSystem().IsCrouch());
            if (animator.GetBool(GROUNDED_HASH) != controller.IsGrounded())
                animator.SetBool(GROUNDED_HASH, controller.IsGrounded());
        }

        /// <summary>
        /// Fixing the position of the body height.
        /// </summary>
        protected virtual void ProcessingBodyHeight()
        {
            float fixedVerticalPosition = controller.GetCrouchSystem().IsCrouch() ? crouchBodyHeight : stayBodyHeight;
            if (UMathf.Approximately(transform.localPosition.y, fixedVerticalPosition))
            {
                Vector3 targetPosition = new Vector3(transform.localPosition.x, fixedVerticalPosition, transform.localPosition.z);
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, moveHeightsmooth * Time.deltaTime);
            }
        }

        /// <summary>
        /// Returning fixed speed of the player for using in animator component parameters.
        /// </summary>
        /// <returns></returns>
        public virtual float GetFixedSpeed()
        {
            if (UInput.GetAxis(INC.CHAR_VERTICAL) == 0)
                return 0;

            float speed = 0;

            if (controller.IsWalking())
                speed = 0.5f;
            else if (controller.IsRunning())
                speed = 1.0f;
            else if (controller.IsSprinting())
                speed = 2.0f;

            speed = speed * Mathf.Sign(UInput.GetAxis(INC.CHAR_VERTICAL));

            return speed;
        }

        /// <summary>
        /// Returning fixed direction of the player for using in animator component parameters.
        /// </summary>
        /// <returns></returns>
        public virtual float GetFixedDirection()
        {
            if (UInput.GetAxis(INC.CHAR_HORIZONTAL) == 0)
                return 0;

            float direction = 0;

            if (controller.IsWalking())
                direction = 0.5f;
            else if (controller.IsRunning())
                direction = 1.0f;
            else if (controller.IsSprinting())
                direction = 2.0f;

            direction = direction * Mathf.Sign(UInput.GetAxis(INC.CHAR_HORIZONTAL));

            return direction;
        }

        public FPController GetPlayer()
        {
            return controller;
        }

        public void SetPlayer(FPController value)
        {
            controller = value;
        }

        public float GetStayBodyHeight()
        {
            return stayBodyHeight;
        }

        public void SetStayBodyHeight(float value)
        {
            stayBodyHeight = value;
        }

        public float GetCrouchBodyHeight()
        {
            return crouchBodyHeight;
        }

        public void SetCrouchBodyHeight(float value)
        {
            crouchBodyHeight = value;
        }

        public float GetMoveHeightSmooth()
        {
            return moveHeightsmooth;
        }

        public void SetMoveHeightSmooth(float value)
        {
            moveHeightsmooth = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        public void SetAnimator(Animator value)
        {
            animator = value;
        }
    }
}