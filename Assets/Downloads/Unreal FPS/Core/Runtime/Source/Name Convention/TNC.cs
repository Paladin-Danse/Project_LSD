/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

namespace UnrealFPS
{
    /// <summary>
    /// Tag name conventions of the all Unreal FPS tags.
    /// </summary>
    public sealed class TNC
    {
        public const string PLAYER = "Player";
        public const string WEAPON = "Weapon";
        public const string CAMERA = "FPCamera";
        public const string CAMERA_LAYER = "FPWeaponLayer";
        public const string AI = "AI";
        public const string CLIMB = "Climb";

        public static string[] GetTags()
        {
            return new string[] { PLAYER, WEAPON, CAMERA, CAMERA_LAYER, AI, CLIMB };
        }
    }
}