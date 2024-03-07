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

namespace UnrealFPS.Runtime
{
    [Serializable]
    public class ULerpControlledBob
    {
        [SerializeField][Range(0.0f, 10.0f)] private float bobDuration = 0.2f;
        [SerializeField][Range(0.0f, 10.0f)] private float bobAmount = 0.1f;

        private float m_Offset = 0f;

        /// <summary>
        /// Bob cycle processing.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator DoBobCycle()
        {
            // make the camera move down slightly
            float t = 0f;
            while (t < bobDuration)
            {
                m_Offset = Mathf.Lerp(0f, bobAmount, t / bobDuration);
                t += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            // make it move back to neutral
            t = 0f;
            while (t < bobDuration)
            {
                m_Offset = Mathf.Lerp(bobAmount, 0f, t / bobDuration);
                t += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            m_Offset = 0f;
        }

        public float GetBobDuration()
        {
            return bobDuration;
        }

        public void SetBobDuration(float value)
        {
            bobDuration = value;
        }

        public float GetBobAmount()
        {
            return bobAmount;
        }

        public void SetBobAmount(float value)
        {
            bobAmount = value;
        }

        public float GetOffset()
        {
            return m_Offset;
        }
    }
}