/* ==================================================================
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
    public class UCurveControlledBob
    {
        [SerializeField] [Range(0.0f, 3.0f)] private float horizontalBobRange = 0.1f;
        [SerializeField] [Range(0.0f, 3.0f)] private float verticalBobRange = 0.1f;
        [SerializeField] [Range(0.0f, 10.0f)] private float verticaltoHorizontalRatio = 1f;
        [SerializeField] [Range(0.0f, 10.0f)] private float bobBaseInterval = 3f;
        [SerializeField] private AnimationCurve bobcurve = SinCurve();

        private float m_CyclePositionX;
        private float m_CyclePositionY;
        private float m_Time;
        private Vector3 m_OriginalCameraPosition;


        /// <summary>
        /// Initialize UCurveControlledBob instance.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="bobBaseInterval"></param>
        public virtual void Initialize(Camera camera)
        {
            m_OriginalCameraPosition = camera.transform.localPosition;
            m_Time = bobcurve[bobcurve.length - 1].time;
        }

        /// <summary>
        /// HeadBob processing.
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public virtual Vector3 DoHeadBob(float speed)
        {
            float xPos = m_OriginalCameraPosition.x + (bobcurve.Evaluate(m_CyclePositionX) * horizontalBobRange);
            float yPos = m_OriginalCameraPosition.y + (bobcurve.Evaluate(m_CyclePositionY) * verticalBobRange);

            m_CyclePositionX += (speed * Time.deltaTime) / bobBaseInterval;
            m_CyclePositionY += ((speed * Time.deltaTime) / bobBaseInterval) * verticaltoHorizontalRatio;

            if (m_CyclePositionX > m_Time)
            {
                m_CyclePositionX = m_CyclePositionX - m_Time;
            }
            if (m_CyclePositionY > m_Time)
            {
                m_CyclePositionY = m_CyclePositionY - m_Time;
            }

            return new Vector3(xPos, yPos, 0f);
        }

        /// <summary>
        /// Sin curve for head bob.
        /// </summary>
        /// <value></value>
        public static AnimationCurve SinCurve()
        {
            return new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f),
                                                        new Keyframe(1f, 0f), new Keyframe(1.5f, -1f),
                                                        new Keyframe(2f, 0f));
        }

        #region [Getter / Setter]
        public float GetHorizontalBobRange()
        {
            return horizontalBobRange;
        }

        public void SetHorizontalBobRange(float value)
        {
            horizontalBobRange = value;
        }

        public float GetVerticalBobRange()
        {
            return verticalBobRange;
        }

        public void SetVerticalBobRange(float value)
        {
            verticalBobRange = value;
        }

        public float GetVerticaltoHorizontalRatio()
        {
            return verticaltoHorizontalRatio;
        }

        public void SetVerticaltoHorizontalRatio(float value)
        {
            verticaltoHorizontalRatio = value;
        }

        public AnimationCurve GetBobCurve()
        {
            return bobcurve;
        }

        public void SetBobCurve(AnimationCurve value)
        {
            bobcurve = value;
        }

        public float GetBobBaseInterval()
        {
            return bobBaseInterval;
        }

        public void SetBobBaseInterval(float value)
        {
            bobBaseInterval = value;
        }

        public float GetCyclePositionX()
        {
            return m_CyclePositionX;
        }
        public float GetCyclePositionY()
        {
            return m_CyclePositionY;
        }

        public float GetTime()
        {
            return m_Time;
        }

        public Vector3 GetOriginalCameraPosition()
        {
            return m_OriginalCameraPosition;
        }
        #endregion
    }
}
