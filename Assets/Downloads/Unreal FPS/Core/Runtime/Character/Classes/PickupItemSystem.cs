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
using UnrealFPS.AI;
using UnrealFPS.UI;

namespace UnrealFPS.Runtime
{
    [System.Serializable]
    public class PickupItemSystem : IPickupItemSystem
    {
        [SerializeField] private HUDManager hud;
        [SerializeField] private IInventory inventory;
        [SerializeField] private IHealth health;

        private PickableItem item;

        /// <summary>
        /// Initialize require components.
        /// </summary>
        public virtual void Initialize(IInventory inventory, IHealth health)
        {
            this.inventory = inventory;
            this.health = health;
        }

        /// <summary>
        /// Pickup item object.
        /// </summary>
        /// <param name="item"></param>
        public void Pickup(PickableItem item)
        {
            if (item == null)
            {
                return;
            }

            switch (item.GetObjectType())
            {
                case PickableItem.ItemType.Weapon:
                    WeaponProcessing(item);
                    break;
                case PickableItem.ItemType.Ammo:
                    AmmoProcessing(item);
                    break;
                case PickableItem.ItemType.HealthBox:
                    HealthBoxProcessing(item);
                    break;
            }
            item = null;
            hud.HideMessage();
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        public virtual void OnTriggerEnter(Collider other)
        {
            item = other.GetComponent<PickableItem>();
        }

        /// <summary>
        /// OnTriggerStay is called once per frame for every Collider other
        /// that is touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        public virtual void OnTriggerStay(Collider other)
        {
            if (item == null && other.GetComponent<PickableItem>() != null)
            {
                item = other.GetComponent<PickableItem>();
            }
            else if (item != null && !item.IsUsed())
            {
                WeaponID activeWeapon = inventory.GetActiveWeaponID();
                WeaponID itemWeapon = item.GetWeapon();
                string format = "None";
                string newLine = "\n";

                if (inventory.IsFull(itemWeapon.GetGroup()) && !string.IsNullOrEmpty(item.GetReplaceMessage()))
                    format = item.GetReplaceMessage();
                else if (!inventory.IsFull(itemWeapon.GetGroup()) && !string.IsNullOrEmpty(item.GetPickUpMessage()))
                    format = item.GetPickUpMessage();
                hud.DisplayMessage(string.Format(format, item.GetPickUpKey(), itemWeapon.GetDisplayName(), newLine));

                if (item.GetProcessingType() == PickableItem.PickupType.Auto ||
                    (item.GetProcessingType() == PickableItem.PickupType.ByKey && Input.GetKeyDown(item.GetPickUpKey())))
                {
                    Pickup(item);
                }
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        public virtual void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PickableItem>())
            {
                item = null;
                hud.HideMessage();
            }
        }
        
        private void WeaponProcessing(PickableItem item)
        {
            bool success = false;
            WeaponID weapon = item.GetWeapon();
            if (inventory.IsFull(weapon.GetGroup()))
            {
                success = inventory.Replace(weapon);
            }
            else
            {
                success = inventory.Add(weapon);
                if(success && item.AutoActivate())
                {
                    inventory.ActivateWeapon(weapon);
                }
            }
            item.IsUsed(success);
        }

        private void AmmoProcessing(PickableItem item)
        {
            Transform weaponTransform = inventory.GetWeapon(item.GetWeapon());
            IWeaponReloading weaponReloading = weaponTransform.GetComponent<IWeaponReloading>();
            item.IsUsed(weaponReloading.AddBullets(item.GetValue()));
        }

        private void HealthBoxProcessing(PickableItem item)
        {
            health.SetHealth(health.GetHealth() + item.GetValue());
            item.IsUsed(true);
        }

        public IInventory GetInventory()
        {
            return inventory;
        }

        public void SetInventory(IInventory value)
        {
            inventory = value;
        }

        public IHealth GetHealth()
        {
            return health;
        }

        public void SetHealth(IHealth value)
        {
            health = value;
        }

        public HUDManager GetHUD()
        {
            return hud;
        }

        public void SetHUD(HUDManager value)
        {
            hud = value;
        }
    }
}