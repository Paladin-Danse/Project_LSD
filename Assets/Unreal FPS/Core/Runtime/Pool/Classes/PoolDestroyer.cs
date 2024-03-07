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
    public class PoolDestroyer : MonoBehaviour, IPoolDestroyer
    {
        [SerializeField] private float delay;

        protected virtual void OnEnable()
        {
            StartCoroutine(StartDelay(delay));
        }

        public virtual IEnumerator StartDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
            yield break;
        }

        public float GetDelay()
        {
            return delay;
        }

        public void SetDelay(float value)
        {
            delay = value;
        }
    }
}