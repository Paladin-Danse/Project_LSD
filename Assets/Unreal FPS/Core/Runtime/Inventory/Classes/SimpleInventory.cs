/* =====================================================================
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

namespace UnrealFPS.Runtime
{

    public class SimpleInventory : MonoBehaviour, IInventory, ISimpleInventory
    {
        [SerializeField] private Transform _FPCamera;
        [SerializeField] private List<InventorySlot> slots;
        [SerializeField] private bool allowIdenticalWeapons = false;
        [SerializeField] private SwitchWeaponMode switchMode = SwitchWeaponMode.Both;

        private Dictionary<KeyCode, WeaponID> cacheKeyToID = new Dictionary<KeyCode, WeaponID>();
        private Dictionary<WeaponID, Transform> cacheIDToTransform = new Dictionary<WeaponID, Transform>();
        private KeyCode currentWeaponKey;
        private KeyCode startWeaponKey;
        private IEnumerator switchWeaponAnimation;
        private IEnumerator replaceWeaponAnimation;
        private IEnumerator hideWeaponAnimation;
        private IEnumerator dropWeaponAnimation;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            cacheKeyToID = new Dictionary<KeyCode, WeaponID>();
            cacheIDToTransform = new Dictionary<WeaponID, Transform>();
            SaveKeyToID(ref cacheKeyToID);
            SaveIDToTransform(ref cacheIDToTransform);
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            if (startWeaponKey != KeyCode.None)
            {
                currentWeaponKey = startWeaponKey;
                ActivateWeapon(currentWeaponKey);
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            switch (switchMode)
            {
                case SwitchWeaponMode.ByKey:
                    SelectWeaponByKey();
                    break;
                case SwitchWeaponMode.ByMouseWheel:
                    SelectWeaponByMouseWheel();
                    break;
                case SwitchWeaponMode.Both:
                    SelectWeaponByKey();
                    SelectWeaponByMouseWheel();
                    break;
            }

            if (UInput.GetButtonDown(INC.DROP))
            {
                Drop();
            }
        }

        /// <summary>
        /// Add new weapon in available slot in inventory.
        /// </summary>
        public virtual bool Add(WeaponID weapon)
        {
            if (weapon == null || (allowIdenticalWeapons && cacheKeyToID.ContainsValue(weapon)))
                return false;

            // If not contains slots with empty weapon, immediately added in last slot.
            if (!cacheKeyToID.ContainsValue(null))
            {
                int index = slots.Count - 1;
                InventorySlot slot = slots[index];
                cacheKeyToID[slot.GetKey()] = weapon;
                slot.SetWeapon(weapon);
                slots[index] = slot;
                return true;
            }

            // If contains slots with empty weapon, find slot with empty weapon and add to it.
            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeapon() == null || (i == length - 1 && slot.GetWeapon() != null))
                {
                    cacheKeyToID[slot.GetKey()] = weapon;
                    slot.SetWeapon(weapon);
                    slots[i] = slot;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove weapon from inventory.
        /// </summary>
        public virtual bool Remove(WeaponID weapon)
        {
            if (weapon == null || !cacheKeyToID.ContainsValue(weapon))
                return false;

            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeapon() == weapon)
                {
                    cacheKeyToID[slot.GetKey()] = null;
                    slot.SetWeapon(null);
                    slots[i] = slot;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove weapon from inventory by Key.
        /// </summary>
        public virtual bool Remove(KeyCode key)
        {
            if (key == KeyCode.None || !cacheKeyToID.ContainsKey(key))
                return false;

            // If contains slots with empty weapon, find slot with empty weapon and add to it.
            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetKey() == key)
                {
                    cacheKeyToID[key] = null;
                    slot.SetWeapon(null);
                    slots[i] = slot;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Replace current weapon on new.
        /// </summary>
        public bool Replace(WeaponID weapon)
        {
            if (weapon == null || !cacheKeyToID.ContainsValue(weapon) || currentWeaponKey == KeyCode.None)
                return false;

            replaceWeaponAnimation = ReplaceWeaponAnimation(weapon);
            StartCoroutine(replaceWeaponAnimation);

            WeaponID currentWeapon = GetActiveWeaponID();
            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeapon() == currentWeapon)
                {
                    cacheKeyToID[slot.GetKey()] = weapon;
                    slot.SetWeapon(weapon);
                    slots[i] = slot;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Replace inventory weapon on some new weapon.
        /// </summary>
        public bool Replace(WeaponID inventoryWeapon, WeaponID someWeapon)
        {
            if (inventoryWeapon == null || someWeapon == null || !cacheKeyToID.ContainsValue(inventoryWeapon) || !cacheKeyToID.ContainsValue(someWeapon))
                return false;

            WeaponID currentWeapon = GetActiveWeaponID();
            if (currentWeapon == inventoryWeapon)
            {
                replaceWeaponAnimation = ReplaceWeaponAnimation(someWeapon);
                StartCoroutine(replaceWeaponAnimation);
            }

            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeapon() == inventoryWeapon)
                {
                    cacheKeyToID[slot.GetKey()] = someWeapon;
                    slot.SetWeapon(someWeapon);
                    slots[i] = slot;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add new slot in inventory.
        /// </summary>
        public virtual bool AddSlot(KeyCode key, WeaponID weapon = null)
        {
            if (key == KeyCode.None || cacheKeyToID.ContainsKey(key) || (weapon != null && allowIdenticalWeapons && cacheKeyToID.ContainsValue(weapon)))
                return false;

            cacheKeyToID.Add(key, weapon);
            slots.Add(new InventorySlot(key, weapon));
            return true;
        }

        /// <summary>
        /// Remove slot from inventory including weapons contained in this slot.
        /// </summary>
        public virtual bool RemoveSlot(KeyCode key)
        {
            if (!cacheKeyToID.ContainsKey(key))
                return false;

            for (int i = 0, length = slots.Count; i < length; i++)
            {
                KeyCode slotKey = slots[i].GetKey();
                if (slotKey == key)
                {
                    slots.RemoveAt(i);
                    break;
                }
                else if (i == length - 1 && slotKey != key)
                {
                    return false;
                }
            }
            cacheKeyToID.Remove(key);
            if (cacheKeyToID.Count == 0)
                HideWeapon(true);
            return true;
        }

        /// <summary>
        /// Activate weapon by key.
        /// </summary>
        public virtual bool ActivateWeapon(KeyCode key)
        {
            if (!cacheKeyToID.ContainsKey(key))
                return false;

            if (hideWeaponAnimation != null || switchWeaponAnimation != null || dropWeaponAnimation != null)
                return false;

            switchWeaponAnimation = SwitchWeaponAnimation(key);
            StartCoroutine(switchWeaponAnimation);
            return true;
        }

        /// <summary>
        /// Activate weapon by ID.
        /// </summary>
        public virtual bool ActivateWeapon(WeaponID weapon)
        {
            if (!cacheKeyToID.ContainsValue(weapon))
                return false;

            if (hideWeaponAnimation != null || switchWeaponAnimation != null || dropWeaponAnimation != null)
                return false;

            switchWeaponAnimation = SwitchWeaponAnimation(weapon);
            StartCoroutine(switchWeaponAnimation);
            return true;
        }

        /// <summary>
        /// Get weapon transform by id.
        /// </summary>
        public virtual Transform GetWeapon(WeaponID weapon)
        {
            if (weapon == null || !cacheKeyToID.ContainsValue(weapon))
                return null;
            return cacheIDToTransform[weapon];
        }

        /// <summary>
        /// Hide active weapon.
        /// </summary>
        public virtual void HideWeapon(bool hide)
        {
            if ((cacheKeyToID.Count == 0 || currentWeaponKey == KeyCode.None) ||
                (hideWeaponAnimation != null || switchWeaponAnimation != null || dropWeaponAnimation != null))
                return;

            hideWeaponAnimation = HideWeaponAnimation();
            StartCoroutine(hideWeaponAnimation);
        }

        /// <summary>
        /// Drop active weapon.
        /// </summary>
        public virtual void Drop()
        {
            if ((cacheKeyToID.Count == 0 || currentWeaponKey == KeyCode.None) ||
                (hideWeaponAnimation != null || switchWeaponAnimation != null || dropWeaponAnimation != null))
                return;

            dropWeaponAnimation = DropWeaponAnimation();
            StartCoroutine(dropWeaponAnimation);
        }

        /// <summary>
        /// Get active weapon transform.
        /// </summary>
        public virtual Transform GetActiveWeaponTransform()
        {
            if (cacheKeyToID.Count == 0 || currentWeaponKey == KeyCode.None)
                return null;
            WeaponID weaponID = cacheKeyToID[currentWeaponKey];
            return cacheIDToTransform[weaponID];
        }

        /// <summary>
        /// Get active WeaponID.
        /// </summary>
        /// <returns></returns>
        public virtual WeaponID GetActiveWeaponID()
        {
            if (cacheKeyToID.Count == 0 || currentWeaponKey == KeyCode.None)
                return null;
            return cacheKeyToID[currentWeaponKey];
        }

        /// <summary>
        /// Weapon count.
        /// </summary>
        /// <returns></returns>
        public virtual int WeaponCount()
        {
            int count = 0;
            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeapon() != null)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Slot count, the maximum possible wearable number of weapons.
        /// </summary>
        public virtual int SlotCount()
        {
            return slots.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsFull()
        {
            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if(slot.GetWeapon() == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsFull(object message = null)
        {
            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if(slot.GetWeapon() == null)
                {
                    return false;
                }
            }
            return true;
        }

        protected virtual void SelectWeaponByKey()
        {
            if (!Input.anyKeyDown || (switchWeaponAnimation != null || hideWeaponAnimation != null) || Input.GetKeyDown(currentWeaponKey))
                return;

            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (Input.GetKeyDown(slot.GetKey()))
                {
                    if (slot.GetWeapon() == null)
                        break;
                    ActivateWeapon(slot.GetKey());
                }
            }
        }

        protected virtual void SelectWeaponByMouseWheel()
        {
            if (UInput.GetAxisRaw(INC.MOUSE_WHEEL) == 0 || (switchWeaponAnimation != null || hideWeaponAnimation != null))
                return;

            int sign = UInput.GetAxisRaw(INC.MOUSE_WHEEL) > 0 ? 1 : -1;
            int length = slots.Count;
            int currentWeaponIndex = -1;

            if (currentWeaponKey == KeyCode.None)
            {
                currentWeaponIndex = 0;
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    InventorySlot slot = slots[i];
                    if (slot.GetKey() == currentWeaponKey)
                    {
                        currentWeaponIndex = i;
                    }
                }
            }

            for (int i = sign == 1 ? currentWeaponIndex + 1 : currentWeaponIndex - 1; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeapon() != null)
                {
                    if (slot.GetKey() == currentWeaponKey)
                        return;
                    ActivateWeapon(slot.GetKey());
                    return;
                }

                if (sign == 1 && i == length - 1 && slot.GetWeapon() == null)
                    i = 0;
                else if (sign == -1 && i == 0 && slot.GetWeapon() == null)
                    i = length - 1;
            }
        }

        protected virtual IEnumerator SwitchWeaponAnimation(KeyCode targetWeaponKey)
        {
            if (currentWeaponKey != KeyCode.None)
            {
                WeaponID currentID = cacheKeyToID[currentWeaponKey];
                Transform currentTransform = cacheIDToTransform[currentID];
                IWeaponAnimator currrentAnimator = null;
                if (currentTransform != null)
                {
                    currrentAnimator = currentTransform.GetComponent<IWeaponAnimator>();
                }
                else
                {
                    switchWeaponAnimation = null;
                    yield break;
                }

                if (currrentAnimator != null)
                    yield return new WaitForSeconds(currrentAnimator.GetPutAwayTime());

                currentTransform.gameObject.SetActive(false);
                currentWeaponKey = KeyCode.None;
            }

            WeaponID targetID = cacheKeyToID[targetWeaponKey];
            if (targetID == null)
            {
                switchWeaponAnimation = null;
                yield break;
            }
            Transform targetTransform = cacheIDToTransform[targetID];
            IWeaponAnimator targetAnimator = null;
            if (targetTransform != null)
            {
                targetAnimator = targetTransform.GetComponent<IWeaponAnimator>();
            }
            else
            {
                switchWeaponAnimation = null;
                yield break;
            }

            targetTransform.gameObject.SetActive(true);

            if (targetAnimator != null)
                yield return new WaitForSeconds(targetAnimator.GetTakeTime());

            currentWeaponKey = targetWeaponKey;
            switchWeaponAnimation = null;
            yield break;
        }

        protected virtual IEnumerator SwitchWeaponAnimation(WeaponID targetWeaponID)
        {
            if (currentWeaponKey != KeyCode.None)
            {
                WeaponID currentID = cacheKeyToID[currentWeaponKey];
                Transform currentTransform = cacheIDToTransform[currentID];
                IWeaponAnimator currrentAnimator = null;
                if (currentTransform != null)
                {
                    currrentAnimator = currentTransform.GetComponent<IWeaponAnimator>();
                }
                else
                {
                    switchWeaponAnimation = null;
                    yield break;
                }

                if (currrentAnimator != null)
                    yield return new WaitForSeconds(currrentAnimator.GetPutAwayTime());

                currentTransform.gameObject.SetActive(false);
                currentWeaponKey = KeyCode.None;
            }

            Transform targetTransform = cacheIDToTransform[targetWeaponID];
            IWeaponAnimator targetAnimator = null;
            if (targetTransform != null)
            {
                targetAnimator = targetTransform.GetComponent<IWeaponAnimator>();
            }
            else
            {
                switchWeaponAnimation = null;
                yield break;
            }

            targetTransform.gameObject.SetActive(true);

            if (targetAnimator != null)
                yield return new WaitForSeconds(targetAnimator.GetTakeTime());

            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeapon() == targetWeaponID)
                {
                    currentWeaponKey = slot.GetKey();
                }
            }
            switchWeaponAnimation = null;
            yield break;
        }

        protected virtual IEnumerator ReplaceWeaponAnimation(WeaponID targetWeaponID)
        {
            if (currentWeaponKey != KeyCode.None)
            {
                WeaponID currentID = cacheKeyToID[currentWeaponKey];
                Transform currentTransform = cacheIDToTransform[currentID];
                DropProperties dropProperties = currentID.GetDropProperties();
                IWeaponAnimator currrentAnimator = null;
                if (currentTransform != null)
                {
                    currrentAnimator = currentTransform.GetComponent<IWeaponAnimator>();
                }
                else
                {
                    replaceWeaponAnimation = null;
                    yield break;
                }

                if (currrentAnimator != null)
                    yield return new WaitForSeconds(currrentAnimator.GetPutAwayTime());

                if (dropProperties != DropProperties.Empty)
                {
                    Vector3 instPosition = _FPCamera.transform.localPosition + (_FPCamera.forward * dropProperties.GetDistance());
                    GameObject dropObject = Instantiate(dropProperties.GetDropObject(), instPosition, Quaternion.Euler(dropProperties.GetRotation()));
                    if (dropObject != null)
                    {
                        Rigidbody rigidbody = dropObject.GetComponent<Rigidbody>();
                        if (rigidbody != null)
                        {
                            rigidbody.AddForce(_FPCamera.forward * dropProperties.GetForce(), ForceMode.Impulse);
                        }
                    }
                }
                currentTransform.gameObject.SetActive(false);
                currentWeaponKey = KeyCode.None;
            }

            Transform targetTransform = cacheIDToTransform[targetWeaponID];
            IWeaponAnimator targetAnimator = null;
            if (targetTransform != null)
            {
                targetAnimator = targetTransform.GetComponent<IWeaponAnimator>();
            }
            else
            {
                replaceWeaponAnimation = null;
                yield break;
            }

            targetTransform.gameObject.SetActive(true);

            if (targetAnimator != null)
                yield return new WaitForSeconds(targetAnimator.GetTakeTime());

            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.GetWeapon() == targetWeaponID)
                {
                    currentWeaponKey = slot.GetKey();
                }
            }
            replaceWeaponAnimation = null;
            yield break;
        }

        protected virtual IEnumerator HideWeaponAnimation()
        {
            WeaponID currentID = cacheKeyToID[currentWeaponKey];
            Transform curTransform = cacheIDToTransform[currentID];
            IWeaponAnimator currrentAnimator = null;
            if (curTransform != null)
            {
                currrentAnimator = curTransform.GetComponent<IWeaponAnimator>();
            }
            else
            {
                hideWeaponAnimation = null;
                yield break;
            }

            if (currrentAnimator != null)
                yield return new WaitForSeconds(currrentAnimator.GetPutAwayTime());

            curTransform.gameObject.SetActive(false);
            hideWeaponAnimation = null;
            yield break;
        }

        protected virtual IEnumerator DropWeaponAnimation()
        {
            WeaponID currentID = cacheKeyToID[currentWeaponKey];
            Transform curTransform = cacheIDToTransform[currentID];
            DropProperties dropProperties = currentID.GetDropProperties();
            IWeaponAnimator currrentAnimator = null;
            if (curTransform != null)
                currrentAnimator = curTransform.GetComponent<IWeaponAnimator>();

            if (currrentAnimator != null)
                yield return new WaitForSeconds(currrentAnimator.GetPutAwayTime());

            curTransform.gameObject.SetActive(false);
            Remove(currentID);

            if (dropProperties != DropProperties.Empty)
            {
                Vector3 instPosition = _FPCamera.transform.localPosition + (_FPCamera.forward * dropProperties.GetDistance());
                GameObject dropObject = Instantiate(dropProperties.GetDropObject(), instPosition, Quaternion.Euler(dropProperties.GetRotation()));
                if (dropObject != null)
                {
                    Rigidbody rigidbody = dropObject.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.AddForce(_FPCamera.forward * dropProperties.GetForce(), ForceMode.Impulse);
                    }
                }
            }

            currentWeaponKey = KeyCode.None;
            dropWeaponAnimation = null;
            yield break;
        }

        protected Dictionary<KeyCode, WeaponID> SaveKeyToID(ref Dictionary<KeyCode, WeaponID> cacheKeyToID)
        {
            for (int i = 0, length = slots.Count; i < length; i++)
            {
                InventorySlot slot = slots[i];
                cacheKeyToID.Add(slot.GetKey(), slot.GetWeapon());
            }
            return cacheKeyToID;
        }

        protected Dictionary<WeaponID, Transform> SaveIDToTransform(ref Dictionary<WeaponID, Transform> cacheIDToTransform)
        {
            for (int i = 0, length = _FPCamera.childCount; i < length; i++)
            {
                Transform weapon = _FPCamera.GetChild(i);
                if (!weapon.CompareTag(TNC.WEAPON))
                    continue;

                WeaponIdentifier weaponIdentifier = weapon.GetComponent<WeaponIdentifier>();
                if (weaponIdentifier.GetWeapon() == null)
                    continue;

                cacheIDToTransform.Add(weaponIdentifier.GetWeapon(), weapon);
            }
            return cacheIDToTransform;
        }

        public Transform GetFPCamera()
        {
            return _FPCamera;
        }

        public void SetFPCamera(Transform value)
        {
            _FPCamera = value;
        }

        public List<InventorySlot> GetSlots()
        {
            return slots;
        }

        protected void SetSlots(List<InventorySlot> value)
        {
            slots = value;
        }

        public bool AllowIdenticalWeapons()
        {
            return allowIdenticalWeapons;
        }

        public void AllowIdenticalWeapons(bool value)
        {
            allowIdenticalWeapons = value;
        }

        public SwitchWeaponMode GetSwitchMode()
        {
            return switchMode;
        }

        public void SetSwitchMode(SwitchWeaponMode value)
        {
            switchMode = value;
        }

        public Dictionary<KeyCode, WeaponID> GetCacheKeyToID()
        {
            return cacheKeyToID;
        }

        protected void SetCacheKeyToID(Dictionary<KeyCode, WeaponID> value)
        {
            cacheKeyToID = value;
        }

        public Dictionary<WeaponID, Transform> GetCacheIDToTransform()
        {
            return cacheIDToTransform;
        }

        protected void SetCacheIDToTransform(Dictionary<WeaponID, Transform> value)
        {
            cacheIDToTransform = value;
        }

        public KeyCode GetCurrentWeaponKey()
        {
            return currentWeaponKey;
        }

        protected void SetCurrentWeaponKey(KeyCode value)
        {
            currentWeaponKey = value;
        }

        public KeyCode GetStartWeaponKey()
        {
            return startWeaponKey;
        }

        public void SetStartWeaponKey(KeyCode value)
        {
            startWeaponKey = value;
        }
    }
}