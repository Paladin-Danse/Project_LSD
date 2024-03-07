/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

namespace UnrealFPS.Runtime
{
    public interface IPickupItemSystem
    {
        /// <summary>
        /// Pickup item object.
        /// </summary>
        void Pickup(PickableItem item);
    }
}