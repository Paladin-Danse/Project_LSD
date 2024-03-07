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
namespace UnrealFPS.Editor
{
    public class UDocumentation
    {
        [MenuItem(UEditorPaths.DEFAULT + "Documentation", false, 1)]
        public static void Open()
        {
            UserHelper.OpenDocumentation();
        }
    }
}