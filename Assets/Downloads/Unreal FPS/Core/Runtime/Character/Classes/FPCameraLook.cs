/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace UnrealFPS.Runtime
{
    [Serializable]
    public class FPCameraLook
    {
        [SerializeField] [Range(0.0f, 50.0f)] private float xSensitivity = 2.0f;
        [SerializeField] [Range(0.0f, 50.0f)] private float ySensitivity = 2.0f;
        [SerializeField] [Range(-360.0f, 360.0f)] private float minimumX = -90.0f;
        [SerializeField] [Range(-360.0f, 360.0f)] private float maximumX = 90.0f;
        [SerializeField] [Range(-360.0f, 360.0f)] private float minimumY = -90.0f;
        [SerializeField] [Range(-360.0f, 360.0f)] private float maximumY = 90.0f;
        [SerializeField] [Range(0.0f, 50.0f)] private float smoothTime = 15.0f;

        [SerializeField] private bool clampVerticalRotation = true;
        [SerializeField] private bool clampHorizontalRotation = false;
        [SerializeField] private bool smooth;
        [SerializeField] private bool lockCursor = true;
        [SerializeField] private bool m_cursorIsLocked = true;


        private Transform player;
        private Transform camera;
        private Quaternion m_PlayerTargetRot;
        private Quaternion m_CameraTargetRot;


        /// <summary>
        /// Initialize FPCameraLook instance.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="camera"></param>
        public virtual void Initialize(Transform player, Transform camera)
        {
            this.player = player;
            this.camera = camera;
            m_PlayerTargetRot = player.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="camera"></param>
        public virtual void RotationHandler()
        {
            float yRot = UInput.GetAxis("Mouse X") * xSensitivity;
            float xRot = UInput.GetAxis("Mouse Y") * ySensitivity;
            m_PlayerTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation || clampHorizontalRotation)
                m_CameraTargetRot = ClampRotation(m_CameraTargetRot);



            if (smooth)
            {
                player.localRotation = Quaternion.Slerp(player.localRotation, m_PlayerTargetRot, smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot, smoothTime * Time.deltaTime);
            }
            else
            {
                player.localRotation = m_PlayerTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
            UpdateCursorLock();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if (!lockCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InternalLockUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_cursorIsLocked = true;
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        /// <summary>
        /// Clamp camera rotation around axis X.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        protected virtual Quaternion ClampRotation(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            if (clampVerticalRotation)
            {
                float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
                angleX = Mathf.Clamp(angleX, minimumX, maximumX);
                q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
            }

            if (clampHorizontalRotation)
            {
                float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
                angleY = Mathf.Clamp(angleY, minimumY, maximumY);
                q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);
            }
            return q;
        }

        #region [Getter / Setter]
        public float GetSensitivityByX()
        {
            return xSensitivity;
        }

        public void SetSensitivityByX(float value)
        {
            xSensitivity = value;
        }

        public float GetSensitivityByY()
        {
            return ySensitivity;
        }

        public void SetSensitivityByY(float value)
        {
            ySensitivity = value;
        }

        public float GetMinimumAngleByX()
        {
            return minimumX;
        }

        public void SetMinimumAngleByX(float value)
        {
            minimumX = value;
        }

        public float GetMaximumAngleByX()
        {
            return maximumX;
        }

        public void SetMaximumAngleByX(float value)
        {
            maximumX = value;
        }

        public float GetSmoothTime()
        {
            return smoothTime;
        }

        public void SetSmoothTime(float value)
        {
            smoothTime = value;
        }

        public bool VerticalRotationClamped()
        {
            return clampVerticalRotation;
        }

        public void ClampVerticalRotation(bool value)
        {
            clampVerticalRotation = value;
        }

        public bool GetSmooth()
        {
            return smooth;
        }

        public void SetSmooth(bool value)
        {
            smooth = value;
        }

        public bool CursorIsLocked()
        {
            return m_cursorIsLocked;
        }

        public Transform GetPlayer()
        {
            return player;
        }

        public Transform GetCamera()
        {
            return camera;
        }

        public Quaternion GetPlayerTargetRot()
        {
            return m_PlayerTargetRot;
        }

        public void SetPlayerTargetRot(Quaternion value)
        {
            m_PlayerTargetRot = value;
        }

        public Quaternion GetCameraTargetRot()
        {
            return m_CameraTargetRot;
        }

        public void SetCameraTargetRot(Quaternion value)
        {
            m_CameraTargetRot = value;
        }

        public bool HorizontalRotationClamped()
        {
            return clampHorizontalRotation;
        }

        public void ClampHorizontalRotation(bool value)
        {
            clampHorizontalRotation = value;
        }

        public float GetMinimumAngleByY()
        {
            return minimumY;
        }

        public void SetMinimumAngleByY(float value)
        {
            minimumY = value;
        }

        public float GetMaximumAngleByY()
        {
            return maximumY;
        }

        public void SetMaximumAngleByY(float value)
        {
            maximumY = value;
        }
        #endregion
    }
}