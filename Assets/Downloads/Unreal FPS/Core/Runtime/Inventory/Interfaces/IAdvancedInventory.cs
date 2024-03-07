/* =====================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS.Runtime
{
    public interface IAdvancedInventory
    {
        /// <summary>
        /// Add new weapon in available slot in inventory.
        /// </summary>
        bool Add(WeaponID weapon);

        /// <summary>
        /// Remove weapon from inventory.
        /// </summary>
        bool Remove(WeaponID weapon);

        /// <summary>
        /// Replace current weapon on new.
        /// </summary>
        bool Replace(WeaponID weapon);

        /// <summary>
        /// Replace inventory weapon on some new weapon.
        /// </summary>
        bool Replace(WeaponID inventoryWeapon, WeaponID someWeapon);

        /// <summary>
        /// Add new group in inventory.
        /// </summary>
        bool AddGroup(string groupName, params InventorySlot[] slots);

        /// <summary>
        /// Remove group from inventory including weapons contained in this group.
        /// </summary>
        bool RemoveGroup(string groupName);

        /// <summary>
        /// Add new slot in group.
        /// </summary>
        bool AddSlotInGroup(string groupName, InventorySlot slot);

        /// <summary>
        /// Remove slot from group including weapon contained in this slot.
        /// </summary>
        bool RemoveSlotFromGroup(string groupName, KeyCode key);

        /// <summary>
        /// Activate weapon by key.
        /// </summary>
        bool ActivateWeapon(KeyCode key);

        /// <summary>
        /// Activate weapon by ID.
        /// </summary>
        bool ActivateWeapon(WeaponID weapon);

        /// <summary>
        /// Get weapon transform by id.
        /// </summary>
        Transform GetWeapon(WeaponID weapon);

        /// <summary>
        /// Hide active weapon.
        /// </summary>
        void HideWeapon(bool hide);

        /// <summary>
        /// Drop active weapon.
        /// </summary>
        void Drop();

        /// <summary>
        /// Get active weapon transform.
        /// </summary>
        Transform GetActiveWeaponTransform();

        /// <summary>
        /// Get active WeaponID.
        /// </summary>
        /// <returns></returns>
        WeaponID GetActiveWeaponID();

        /// <summary>
        /// Weapon count.
        /// </summary>
        /// <returns></returns>
        int WeaponCount();

        /// <summary>
        /// Slot count, the maximum possible wearable number of weapons.
        /// </summary>
        int SlotCount(string groupName);

        /// <summary>
        /// Group count.
        /// </summary>
        int GroupCount(string groupName);

        /// <summary>
        /// 
        /// </summary>
        bool IsFull(string group);
    }
}