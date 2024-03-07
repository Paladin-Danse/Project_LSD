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
    public interface IWeaponAnimator
    {
        float GetTakeTime();

        float GetPutAwayTime();

        void SetSpeed(int state);

        void SetAttack(int state);

        void SetReload(int state);

        void SetSight(bool state);

        void PutAway();

        WeaponActionState GetActiveState();

        Animator GetAnimator();
    }
}