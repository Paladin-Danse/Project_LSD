/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;
using UnrealFPS.Runtime;

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(InputController), true)]
    [CanEditMultipleObjects]
    public class InputControllerEditor : UEditor<InputController>
    {
        public override string HeaderName()
        {
            return "First Person Input Controller";
        }

        public override void BaseGUI()
        {
            UEditorHelpBoxMessages.Message("Management of the player is carried out by means of the controller: " + "[" + instance.GetType().Name.AddSpaces() + "]", MessageType.Info);
        }
    }
}