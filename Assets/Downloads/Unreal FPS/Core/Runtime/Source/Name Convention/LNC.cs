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
    /// Layer name conventions of the all Unreal FPS layers.
    /// </summary>
    public sealed class LNC
    {
        public const string PLAYER = "Player";
        public const string WEAPON = "Weapon";
        public const string AI = "AI";
        public const string AI_FRIENDLY = "AI Friendly";
        public const string AI_ENEMY = "AI Enemy";
        public const string REMOTE_BODY = "Remote Body";
        public const string OBSTACLE = "Obstacle";

        public static string[] GetLayers()
        {
            return new string[] { PLAYER, WEAPON, AI, AI_FRIENDLY, AI_ENEMY, REMOTE_BODY, OBSTACLE };
        }
    }
}