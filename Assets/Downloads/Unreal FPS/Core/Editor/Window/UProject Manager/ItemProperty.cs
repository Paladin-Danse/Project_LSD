/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System;

namespace UnrealFPS.Editor
{
    public partial class UProjectManager
    {
        /// <summary>
        /// UProject Manager item require properties struct.
        /// </summary>
        public struct ItemProperty : IEquatable<ItemProperty>
        {
            private int id;
            private int index;
            private string name;
            private ItemType itemType;
            private Action onEnable;
            private Action onGUI;

            /// <summary>
            /// Item property constructor.
            /// </summary>
            /// <param name="id">Item ID.</param>
            /// <param name="index">Item index (position on tree view).</param>
            /// <param name="name">Item name.</param>
            /// <param name="type">Item type.</param>
            /// <param name="onEnable">Item onEnable delegate.</param>
            /// <param name="onGUI">Item onGUI delegate.</param>
            internal ItemProperty(int id, int index, string name, ItemType type, Action onEnable, Action onGUI)
            {
                this.id = id;
                this.index = index;
                this.name = name;
                this.itemType = type;
                this.onEnable = onEnable;
                this.onGUI = onGUI;
            }

            /// <summary>
            /// Return item ID.
            /// </summary>
            /// <returns></returns>
            public int GetID()
            {
                return id;
            }

            /// <summary>
            /// Set item ID.
            /// </summary>
            /// <param name="value"></param>
            public void SetID(int value)
            {
                id = value;
            }

            /// <summary>
            /// Return item index (position on tree view).
            /// </summary>
            /// <returns></returns>
            public int GetIndex()
            {
                return index;
            }

            /// <summary>
            /// Set item index (position on tree view).
            /// </summary>
            /// <param name="value"></param>
            public void SetIndex(int value)
            {
                index = value;
            }

            /// <summary>
            /// Return item name.
            /// </summary>
            /// <returns></returns>
            public string GetName()
            {
                return name;
            }

            /// <summary>
            /// Set item name.
            /// </summary>
            /// <param name="value"></param>
            public void SetName(string value)
            {
                name = value;
            }

            /// <summary>
            /// Return item type.
            /// </summary>
            /// <returns></returns>
            public ItemType GetItemType()
            {
                return itemType;
            }

            /// <summary>
            /// Set item type.
            /// </summary>
            /// <param name="value"></param>
            public void SetItemType(ItemType value)
            {
                itemType = value;
            }

            /// <summary>
            /// Return item onEnable delegate.
            /// </summary>
            /// <returns></returns>
            public Action GetOnEnableDelegate()
            {
                return onEnable;
            }

            /// <summary>
            /// Set item onEnable delegate.
            /// </summary>
            /// <param name="value"></param>
            public void SetOnEnableDelegate(Action value)
            {
                onEnable = value;
            }

            /// <summary>
            /// Invoke item onEnable delegate.
            /// </summary>
            /// <returns></returns>
            public void InvokeOnEnable()
            {
                onEnable();
            }

            /// <summary>
            /// Return item onGUI delegate.
            /// </summary>
            /// <returns></returns>
            public Action GetOnGUIDelegate()
            {
                return onGUI;
            }

            /// <summary>
            /// Set item onGUI delegate.
            /// </summary>
            /// <param name="value"></param>
            public void SetOnGUIDelegate(Action value)
            {
                onGUI = value;
            }

            /// <summary>
            /// Invoke item onGUI delegate.
            /// </summary>
            /// <returns></returns>
            public void InvokeOnGUI()
            {
                onGUI();
            }

            /// <summary>
            /// Empty item property.
            /// </summary>
            /// <returns></returns>
            public readonly static ItemProperty Empty = new ItemProperty(-1, -1, "", ItemType.Other, () => { }, () => { });

            public static bool operator ==(ItemProperty left, ItemProperty right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ItemProperty left, ItemProperty right)
            {
                return !Equals(left, right);
            }

            public override bool Equals(object obj)
            {
                return (obj is ItemProperty metrics) && Equals(metrics);
            }

            public bool Equals(ItemProperty other)
            {
                return (id, index, name, itemType, onEnable, onGUI) == (other.id, other.index, other.name, other.itemType, other.onEnable, other.onGUI);
            }

            public override int GetHashCode()
            {
                return (id, index, name, itemType, onEnable, onGUI).GetHashCode();
            }
        }
    }
}