/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS.Editor
{
    [CreateAssetMenu(fileName = "EditorProperties", menuName = UEditorPaths.EDITOR + "EditorProperties", order = 131)]
    public class UEditorProperties : ScriptableObject
    {
        private bool checkEachCompile = true;

        public bool VerificationEachCompile()
        {
            return checkEachCompile;
        }

        public void VerificationEachCompile(bool value)
        {
            checkEachCompile = value;
        }
    }
}