/* =====================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

namespace UnrealFPS.Runtime
{
    public interface IWeaponReloading
    {
        bool IsReloading();

        void BulletSubtraction();

        bool AddBullets(int value);

        bool AddClips(int value);
        
        void SetBulletCount(int value);

        int GetBulletCount();

        void SetClipCount(int value);

        int GetClipCount();

        void SetMaxBulletCount(int value);

        int GetMaxBulletCount();

        void SetMaxClipCount(int value);

        int GetMaxClipCount();

        bool ClipsIsEmpty();

        bool BulletsIsEmpty();
        
    }
}