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
using UnrealFPS.UI;

namespace UnrealFPS.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class PickableItem : MonoBehaviour, IPickableItem
    {
        public enum PickupType
        {
            /// <summary>
            /// Pickup weapon immediately after enter collider.
            /// </summary>
            Auto,

            /// <summary>
            ///  Pickup weapon after enter collider and pressed specefic key.
            /// </summary>
            ByKey
        }

        public enum ItemType
        {
            /// <summary>
            /// Pickable weapon item.
            /// </summary>
            Weapon,

            /// <summary>
            /// Pickable ammo item.
            /// </summary>
            Ammo,

            /// <summary>
            /// Pickable health box item.
            /// </summary>
            HealthBox
        }

        [SerializeField] private Transform item;
        [SerializeField] private PickupType pickupType;
        [SerializeField] private ItemType objectType;
        [SerializeField] private string pickUpMessage;
        [SerializeField] private string replaceMessage;
        [SerializeField] private WeaponID weapon;
        [SerializeField] private bool autoActivate;
        [SerializeField] private int value;
        [SerializeField] private bool isReusable;
        [SerializeField] private float reusableDelay;
        [SerializeField] private bool destroyAfterUse;
        [SerializeField] private KeyCode pickUpKey;
        [SerializeField] private AudioClip soundEffect;

        private bool isUsed;
        private AudioSource audioSource;
        private IEnumerator timer;
        private IEnumerator destroyItem;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (isReusable && isUsed && timer == null)
            {
                timer = ReusableTimer(reusableDelay);
                StartCoroutine(timer);
                PlaySoundEffect();
                item.gameObject.SetActive(false);
            }
            else if (destroyAfterUse && !isReusable && isUsed && destroyItem == null)
            {
                destroyItem = DestroyItem(soundEffect ? soundEffect.length : 0);
                StartCoroutine(destroyItem);
            }
        }

        /// <summary>
        /// Player sound effect
        /// </summary>
        protected void PlaySoundEffect()
        {
            if (soundEffect != null)
                audioSource.PlayOneShot(soundEffect);
        }

        /// <summary>
        /// Reset used state after specific time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        protected virtual IEnumerator ReusableTimer(float time)
        {
            yield return new WaitForSeconds(time);
            isUsed = false;
            item.gameObject.SetActive(true);
            timer = null;
            yield break;
        }

        /// <summary>
        /// Destroy item after specific time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        protected virtual IEnumerator DestroyItem(float time)
        {
            item.gameObject.SetActive(false);
            PlaySoundEffect();
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }

        public Transform GetItem()
        {
            return item;
        }

        public void SetItem(Transform value)
        {
            item = value;
        }

        public bool AutoActivate()
        {
            return autoActivate;
        }

        public void AutoActivate(bool value)
        {
            autoActivate = value;
        }

        public bool IsReusable()
        {
            return isReusable;
        }

        public void IsReusable(bool value)
        {
            isReusable = value;
        }

        public float GetReusableDelay()
        {
            return reusableDelay;
        }

        public void SetReusableDelay(float value)
        {
            reusableDelay = value;
        }

        public bool DestroyAfterUse()
        {
            return destroyAfterUse;
        }

        public void DestroyAfterUse(bool value)
        {
            destroyAfterUse = value;
        }

        public bool IsUsed()
        {
            return isUsed;
        }

        public void IsUsed(bool value)
        {
            isUsed = value;
        }

        public ItemType GetObjectType()
        {
            return objectType;
        }

        public void SetObjectType(ItemType value)
        {
            objectType = value;
        }

        public WeaponID GetWeapon()
        {
            return weapon;
        }

        public void SetWeapon(WeaponID value)
        {
            weapon = value;
        }

        public int GetValue()
        {
            return value;
        }

        public void SetValue(int value)
        {
            this.value = value;
        }

        public PickupType GetProcessingType()
        {
            return pickupType;
        }

        public void SetProcessingType(PickupType value)
        {
            pickupType = value;
        }

        public KeyCode GetPickUpKey()
        {
            return pickUpKey;
        }

        public void SetPickUpKey(KeyCode value)
        {
            pickUpKey = value;
        }

        public AudioClip GetSoundEffect()
        {
            return soundEffect;
        }

        public void SetSoundEffect(AudioClip value)
        {
            soundEffect = value;
        }

        public string GetPickUpMessage()
        {
            return pickUpMessage;
        }

        public void SetPickUpMessage(string value)
        {
            pickUpMessage = value;
        }

        public string GetReplaceMessage()
        {
            return replaceMessage;
        }

        public void SetReplaceMessage(string value)
        {
            replaceMessage = value;
        }
    }
}