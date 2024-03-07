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
    /// Input name conventions of the all Unreal FPS inputs.
    /// </summary>
    public sealed class INC
    {
        /*
         *  _____________INPUT AXES_____________
         *
         *  All input name axes that used in SDK.
         *  All these axes must be contained in default Unity input manager.
         */
        public const string CHAR_VERTICAL = "Vertical";
        public const string CHAR_HORIZONTAL = "Horizontal";
        public const string CAM_VERTICAL = "Mouse X";
        public const string CAM_HORIZONTAL = "Mouse Y";
        public const string MOUSE_WHEEL = "Mouse ScrollWheel";

        public static string[] GetAxes()
        {
            return new string[] { CHAR_VERTICAL, CHAR_HORIZONTAL, CAM_VERTICAL, CAM_HORIZONTAL, MOUSE_WHEEL };
        }

        /*
         *  _____________INPUT ACTION_____________
         * 
         *  All input name action that used in SDK.
         *  All these action must be contained in default Unity input manager.
         */
        public const string CROUCH = "Crouch";
        public const string SPRINT = "Sprint";
        public const string ATTACK = "Fire";
        public const string RELOAD = "Reload";
        public const string SIGHT = "Sight";
        public const string GRAB = "Grab";
        public const string ACTION = "Action";
        public const string WALK = "Light Walk";
        public const string JUMP = "Jump";
        public const string DROP = "Drop";
        public const string LEFT_TILT = "Left Tilt";
        public const string RIGHT_TILT = "Right Tilt";

        public static string[] GetButtons()
        {
            return new string[] { CROUCH, SPRINT, ATTACK, RELOAD, SIGHT, GRAB, ACTION, WALK, JUMP, DROP, LEFT_TILT, RIGHT_TILT };
        }
    }
}