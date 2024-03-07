/* ================================================================
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
    public class HealthRegenerationSystem
    {
        [SerializeField] private HealthRegenirationProperties regenerationProperties;

        private IHealth health;
        private MonoBehaviour monoBehaviour;
        private IEnumerator regenerationCoroutine;

        public virtual void Initialize(IHealth health, MonoBehaviour monoBehaviour)
        {
            this.health = health;
            this.monoBehaviour = monoBehaviour;
            regenerationCoroutine = null;
        }

        /// <summary>
        /// Health regeneraion processing.
        /// </summary>
        public virtual void StartRegeneraionProcessing()
        {
            if (!health.IsAlive())
                return;

            if (regenerationCoroutine != null)
                monoBehaviour.StopCoroutine(regenerationCoroutine);

            regenerationCoroutine = RegenerationCoroutine(regenerationProperties);
            monoBehaviour.StartCoroutine(regenerationCoroutine);
        }

        /// <summary>
        /// Healt regeneration coroutine.
        /// </summary>
        /// <param name="regenirationProperties"></param>
        /// <returns>IEnumerator</returns>
        protected virtual IEnumerator RegenerationCoroutine(HealthRegenirationProperties regenirationProperties)
        {
            WaitForSeconds rateDelay = new WaitForSeconds(regenerationProperties.GetRate());
            yield return new WaitForSeconds(regenerationProperties.GetDelay());
            while (true)
            {
                if(!health.IsAlive())
                {
                    yield break;
                }
                
                if ((health.GetHealth() + regenerationProperties.GetValue()) < health.GetMaxHealth())
                {
                    health.SetHealth(health.GetHealth() + regenerationProperties.GetValue());
                }
                else
                {
                    health.SetHealth(health.GetMaxHealth());
                    regenerationCoroutine = null;
                    yield break;
                }
                yield return rateDelay;
            }
        }

        public HealthRegenirationProperties GetRegenerationProperties()
        {
            return regenerationProperties;
        }

        public void SetRegenerationProperties(HealthRegenirationProperties value)
        {
            regenerationProperties = value;
        }

        public IHealth GetHealthInstance()
        {
            return health;
        }

        public MonoBehaviour GetMonoBehaviourInstance()
        {
            return monoBehaviour;
        }
    }
}