/* ==================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections;

namespace UnrealFPS.Runtime
{
    public interface IPoolDestroyer
    {
        IEnumerator StartDelay(float delay);
    }
}