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
    public interface IBullet
    {
        string GetModel();

        int GetDamage();

        float GetVariance();

        int GetNumberBullet();

        DecalProperties GetDecalProperties();
    }
}