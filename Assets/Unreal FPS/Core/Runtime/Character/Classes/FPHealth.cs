/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnrealFPS.UI;

namespace UnrealFPS.Runtime
{
    [RequireComponent(typeof(IPlayerController))]
    public partial class FPHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private int health = 100;
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int minHealth = 0;
        [SerializeField] private bool useRegeniration = false;
        [SerializeField] private FallDamageProperties[] fallDamageProperties;
        [SerializeField] private HealthRegenerationSystem regenerationSystem = new HealthRegenerationSystem();
        [SerializeField] private DeathCamera deathCamera = new DeathCamera();
        [SerializeField] private DamageCameraEffect damageCameraEffect = new DamageCameraEffect();

        private IPlayerController controller;
        private float lastHeightPosition;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            controller = GetComponent<IPlayerController>();
            regenerationSystem.Initialize(this, this);
            deathCamera.Initialize(this);
            damageCameraEffect.Initialize(this);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (!IsAlive())
                deathCamera.StartProcessing();
            else
                deathCamera.StopProcessing();
        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (IsAlive())
                FallDamageProcessing(controller.IsGrounded());
            damageCameraEffect.EffectProcessing();
        }

        public virtual void TakeDamage(int amount)
        {
            amount = Mathf.Abs(amount);
            health = (health - amount) >= minHealth ? health - amount : minHealth;
            ShakeCamera.Instance.AddShakeEvent(ShakeCamera.HitShakeProperties());
            if (useRegeniration)
                regenerationSystem.StartRegeneraionProcessing();
        }

        /// <summary>
        /// Health damage processing by fall.
        /// </summary>
        /// <param name="isGrounded">Player is grounded</param>
        protected virtual void FallDamageProcessing(bool isGrounded)
        {
            if (!isGrounded)
            {
                if (lastHeightPosition < transform.position.y)
                {
                    lastHeightPosition = transform.position.y;
                }
            }
            else if (lastHeightPosition > transform.position.y)
            {
                float distance = Mathf.Abs(lastHeightPosition - transform.position.y);
                for (int i = 0; i < fallDamageProperties.Length; i++)
                {
                    FallDamageProperties damageProperty = fallDamageProperties[i];
                    if (distance > damageProperty.GetMinHeight() && distance < damageProperty.GetMaxHeight())
                    {
                        TakeDamage(damageProperty.GetDamage());
                        lastHeightPosition = transform.position.y;
                    }
                }
            }
        }

        public int GetHealth()
        {
            return health;
        }

        public void SetHealth(int value)
        {
            if (value > maxHealth)
            {
                health = maxHealth;
            }
            else if (value < minHealth)
            {
                health = minHealth;
            }
            else
            {
                health = value;
            }
        }

        public bool IsAlive()
        {
            return health > 0;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public void SetMaxHealth(int value)
        {
            maxHealth = value;
        }

        public int GetMinHealth()
        {
            return minHealth;
        }

        public void SetMinHealth(int value)
        {
            if (value < 0)
                value = 0;
            minHealth = value;
        }

        public float GetHealthPercent()
        {
            return ((float)health / maxHealth) * 100;
        }

        public bool RegenirationIsActive()
        {
            return useRegeniration;
        }

        public void RegenirationActive(bool value)
        {
            useRegeniration = value;
        }

        public FallDamageProperties[] GetFallDamageProperties()
        {
            return fallDamageProperties;
        }

        public void SetFallDamagePropertiesRange(FallDamageProperties[] value)
        {
            fallDamageProperties = value;
        }

        public DeathCamera GetDeathCamera()
        {
            return deathCamera;
        }

        public void SetDeathCamera(DeathCamera value)
        {
            deathCamera = value;
        }

        public DamageCameraEffect GetDamageCameraEffect()
        {
            return damageCameraEffect;
        }

        public void SetDamageCameraEffect(DamageCameraEffect value)
        {
            damageCameraEffect = value;
        }

        public HealthRegenerationSystem GetRegenerationSystem()
        {
            return regenerationSystem;
        }

        public IPlayerController GetControllerCallbacks()
        {
            return controller;
        }
    }
}