/* ================================================================
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
    public struct InventorySlot : IEquatable<InventorySlot>
    {
        [SerializeField] private KeyCode key;
        [SerializeField] private WeaponID weapon;

        /// <summary>
        /// Inventory slot constructor.
        /// </summary>
        /// <param name="key">Slot key.</param>
        /// <param name="weapon">Slot weapon.</param>
        public InventorySlot(KeyCode key, WeaponID weapon)
        {
            this.key = key;
            this.weapon = weapon;
        }

        /// <summary>
        /// Return slot key.
        /// </summary>
        /// <returns></returns>
        public KeyCode GetKey()
        {
            return key;
        }

        /// <summary>
        /// Set slot key.
        /// </summary>
        /// <param name="value"></param>
        public void SetKey(KeyCode value)
        {
            key = value;
        }

        /// <summary>
        /// Return slot weapon.
        /// </summary>
        /// <returns></returns>
        public WeaponID GetWeapon()
        {
            return weapon;
        }

        /// <summary>
        /// Set slot weapon.
        /// </summary>
        /// <param name="weapon"></param>
        public void SetWeapon(WeaponID weapon)
        {
            this.weapon = weapon;
        }

        /// <summary>
        /// Empty Inventory slot.
        /// </summary>
        /// <returns></returns>
        public readonly static InventorySlot Empty = new InventorySlot(KeyCode.None, null);

        public static bool operator ==(InventorySlot left, InventorySlot right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InventorySlot left, InventorySlot right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is InventorySlot metrics) && Equals(metrics);
        }

        public bool Equals(InventorySlot other)
        {
            return (key, weapon) == (other.key, other.weapon);
        }

        public override int GetHashCode()
        {
            return (key, weapon).GetHashCode();
        }
    }
}