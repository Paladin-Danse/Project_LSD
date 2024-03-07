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
    public interface IPoolManager
    {
        /// <summary>
        /// Pop gameobject from pool.
        /// </summary>
        GameObject Pop(string ID);

        /// <summary>
        /// Push gameObject in pool.
        /// </summary>
        void Push(GameObject gameObject);
    }
}