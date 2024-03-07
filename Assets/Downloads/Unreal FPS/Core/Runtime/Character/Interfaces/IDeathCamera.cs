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
    public interface IDeathCamera
    {
        /// <summary>
        /// Initialize Death Camera system.
        /// </summary>
        /// <param name="mono">IHealth MonoBehaviour.</param>
        void Initialize(MonoBehaviour mono);

        /// <summary>
        /// Start processing player death camera.
        /// </summary>
        void StartProcessing();

        /// <summary>
        /// Stop processing player death camera.
        /// </summary>
        void StopProcessing();
    }
}