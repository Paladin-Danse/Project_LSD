/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;
using UnrealFPS.Utility;

namespace UnrealFPS.Runtime
{
    [Serializable]
    public class FPCrouch
    {
        public enum CrouchType
        {
            OncePress,
            Hold
        }

        [SerializeField] private CrouchType crouchType;
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float smooth = 5.0f;
        [SerializeField] private float crouchHeight = 0.7f;

        private Transform player;
        private CharacterController characterController;
        private MonoBehaviour monoBehaviour;
        private IEnumerator doCrouch;
        private float wasControllerHeight;
        private bool isCrouch;
        private float targetHeight;
        private Vector3 targetPos;

        /// <summary>
        /// Initialize the required components
        /// </summary>
        /// <param name="player"></param>
        /// <param name="characterController"></param>
        public virtual void Initialize(MonoBehaviour monoBehaviour, Transform player, CharacterController characterController)
        {
            this.monoBehaviour = monoBehaviour;
            this.player = player;
            this.characterController = characterController;
            wasControllerHeight = characterController.height;

            targetHeight = isCrouch ? crouchHeight : wasControllerHeight;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update()
        {
            if (UInput.GetButtonDown(INC.CROUCH) && !isCrouch)
            {
                isCrouch = true;
                targetHeight = crouchHeight;
            }
            else if ((crouchType == CrouchType.OncePress && UInput.GetButtonDown(INC.CROUCH)) || (crouchType == CrouchType.Hold && UInput.GetButtonUp(INC.CROUCH)) && isCrouch)
            {
                isCrouch = false;
                targetHeight = wasControllerHeight;
            }

            targetPos = player.position;
            float lastFPHeight = characterController.height;
            characterController.height = Mathf.Lerp(characterController.height, targetHeight, smooth * Time.deltaTime);
            targetPos.y += (characterController.height - lastFPHeight) / 2;
            player.position = targetPos;
        }

        public CrouchType GetCrouchType()
        {
            return crouchType;
        }

        public void SetCrouchType(CrouchType value)
        {
            crouchType = value;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public void SetSpeed(float value)
        {
            speed = value;
        }

        public float GetSmooth()
        {
            return smooth;
        }

        public void SetSmooth(float value)
        {
            smooth = value;
        }

        public float GetCrouchHeight()
        {
            return crouchHeight;
        }

        public void SetCrouchHeight(float value)
        {
            crouchHeight = value;
        }

        public Transform GetPlayer()
        {
            return player;
        }

        public CharacterController GetCharacterController()
        {
            return characterController;
        }

        public bool IsCrouch()
        {
            return isCrouch;
        }

        public MonoBehaviour GetPlayerMonoBehaviourInstance()
        {
            return monoBehaviour;
        }
    }
}