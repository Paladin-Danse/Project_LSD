/* ================================================================
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
    public interface IPickableItem
    {
        /// <summary>
        /// Get used item state.
        /// </summary>
        /// <returns></returns>
        bool IsUsed();

         /// <summary>
        /// Set used item state.
        /// </summary>
        /// <returns></returns>
        void IsUsed(bool value);
    }
}