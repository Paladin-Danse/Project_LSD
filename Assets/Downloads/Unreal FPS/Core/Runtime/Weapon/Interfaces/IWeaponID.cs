/* =====================================================================
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
    public interface IWeaponID
    {
        string GetID();

        void SetID(string value);

        string GetDisplayName();

        void SetDisplayName(string value);

        string GetGroup();

        void SetGroup(string value);

        Sprite GetImage();

        void SetImage(Sprite value);

        DropProperties GetDropProperties();

        void SetDropProperties(DropProperties value);
    }
}