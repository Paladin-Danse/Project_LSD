/* ==================================================================
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
    /// <summary>
    /// IInventoryCallbacks interface contains require inventory callbacks.
    /// </summary>
    public interface IInventory
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
        /// <param name="weapon"></param>
        /// <returns></returns>
        bool Replace(WeaponID weapon);

        /// <summary>
        /// Replace inventory weapon on some new weapon.
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        bool Replace(WeaponID inventoryWeapon, WeaponID someWeapon);

        /// <summary>
        /// Activate weapon by ID.
        /// </summary>
        bool ActivateWeapon(WeaponID weapon);

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
        /// 
        /// </summary>
        bool IsFull(object message = null);
    }
}