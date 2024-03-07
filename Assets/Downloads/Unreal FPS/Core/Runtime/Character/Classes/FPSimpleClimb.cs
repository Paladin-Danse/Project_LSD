/* ==================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;

namespace UnrealFPS.Runtime
{
    [System.Serializable]
    public class FPSimpleClimb : ISimpleClimb
    {
        public enum ClimbStyle
        {
            Free,
            ByKey
        }

        [SerializeField] private ClimbStyle climbStart = ClimbStyle.Free;
        [SerializeField] private KeyCode startClimbKey;

        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float downThreshold = -0.4f;
        [SerializeField] private bool freezLateralMove = true;

        [SerializeField] private float playSoundSpeed = 1.0f;
        [SerializeField] private ClimbstepProperties climbStepProperties;

        private Transform player;
        private CharacterController characterController;
        private FPCameraLook cameraLook;
        private AudioSource audioSource;
        private float wasPlaySoundSpeed;
        private bool isClimbing;
        private bool canClimb;

        private PhysicMaterial laddeMaterial;
        private Vector3 verticalMove;
        private Vector3 lateralMove;

        /// <summary>
        /// Initialize simple climb instance.
        /// </summary>
        public virtual void Initialize(Transform player, FPCameraLook cameraLook, CharacterController characterController, AudioSource audioSource)
        {
            this.player = player;
            this.cameraLook = cameraLook;
            this.characterController = characterController;
            this.audioSource = audioSource;

            wasPlaySoundSpeed = playSoundSpeed;
            downThreshold = -0.4f;
            isClimbing = false;
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TNC.CLIMB))
            {
                canClimb = true;
            }
        }

        /// <summary>
        /// OnTriggerStay is called once per frame for every Collider other
        /// that is touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        public virtual void OnTriggerStay(Collider other)
        {
            if (!canClimb)
            {
                return;
            }

            if (!isClimbing && ((climbStart == ClimbStyle.ByKey && Input.GetKeyDown(startClimbKey)) || climbStart == ClimbStyle.Free))
            {
                isClimbing = true;
                laddeMaterial = other.sharedMaterial;
            }

            if (isClimbing)
            {

                float direction = (cameraLook.GetCamera().forward.y > downThreshold && UInput.GetAxis(INC.CHAR_VERTICAL) > 0) ? 1 : -1;
                verticalMove.y = Mathf.Abs(UInput.GetAxis(INC.CHAR_VERTICAL)) * direction;
                if (!freezLateralMove)
                {
                    lateralMove = new Vector3(UInput.GetAxis(INC.CHAR_HORIZONTAL), 0, UInput.GetAxis(INC.CHAR_VERTICAL));
                    lateralMove = player.TransformDirection(lateralMove);
                }
                characterController.Move((verticalMove + lateralMove) * (speed * Time.deltaTime));

                if (UInput.GetAxis(INC.CHAR_VERTICAL) != 0)
                    PlayClimbSound(laddeMaterial);
                else if (wasPlaySoundSpeed != playSoundSpeed)
                    wasPlaySoundSpeed = playSoundSpeed;

                if (UInput.GetButtonDown(INC.JUMP))
                {
                    isClimbing = false;
                    canClimb = false;
                    wasPlaySoundSpeed = playSoundSpeed;
                }
            }

        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        public virtual void OnTriggerExit(Collider other)
        {
            if (isClimbing && other.CompareTag(TNC.CLIMB))
            {
                isClimbing = false;
                canClimb = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void PlayClimbSound(PhysicMaterial laddeMaterial)
        {
            if (climbStepProperties == null)
                return;

            wasPlaySoundSpeed -= Time.deltaTime;
            if (wasPlaySoundSpeed <= 0)
            {
                AudioClip clip = null;

                for (int i = 0; i < climbStepProperties.GetLength(); i++)
                {
                    ClimbstepProperty property = climbStepProperties.GetProperty(i);
                    if (property.GetPhysicMaterial() == laddeMaterial)
                    {
                        int clipIndex = Random.Range(0, property.GetSounds().Length);
                        clip = property.GetSound(clipIndex);
                        break;
                    }
                }

                if (clip != null)
                {
                    audioSource.PlayOneShot(clip);
                }

                wasPlaySoundSpeed = playSoundSpeed;
            }
        }

        #region [Getter / Setter]
        public float GetSpeed()
        {
            return speed;
        }

        public void SetSpeed(float value)
        {
            speed = value;
        }

        public float GetPlaySoundCycle()
        {
            return playSoundSpeed;
        }

        public void SetPlaySoundCycle(float value)
        {
            playSoundSpeed = value;
        }

        public bool FreezLateralMove()
        {
            return freezLateralMove;
        }

        public void FreezLateralMove(bool value)
        {
            freezLateralMove = value;
        }

        public ClimbstepProperties GetClimbStepProperties()
        {
            return climbStepProperties;
        }

        public void SetClimbStepProperties(ClimbstepProperties value)
        {
            climbStepProperties = value;
        }

        public Transform GetPlayer()
        {
            return player;
        }

        public FPCameraLook GetCameraLook()
        {
            return cameraLook;
        }

        public CharacterController GetCharacterController()
        {
            return characterController;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        public bool IsClimbing()
        {
            return isClimbing;
        }

        public float GetDownThreshold()
        {
            return downThreshold;
        }

        public void SetDownThreshold(float value)
        {
            downThreshold = value;
        }

        public KeyCode GetStartClimbKey()
        {
            return startClimbKey;
        }

        public void SetStartClimbKey(KeyCode value)
        {
            startClimbKey = value;
        }

        public ClimbStyle GetClimbStart()
        {
            return climbStart;
        }

        public void SetClimbStyle(ClimbStyle value)
        {
            climbStart = value;
        }
        #endregion
    }
}