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
    [InitializeOnLoad]
    public static class UEditorProjectVerifier
    {
        private static UEditorProperties editorProperties;

        /// <summary>
        /// Verification project settings each compiling.
        /// </summary>
        static UEditorProjectVerifier()
        {
            editorProperties = UEditorResourcesHelper.GetEditorProperties();
            if (editorProperties == null || editorProperties.VerificationEachCompile())
            {
                bool itemsIsReady = ProjectSetupAssistant.ReflectionHelper.ItemsIsReady();
                if (!itemsIsReady)
                {
                    int selectedOption = UDisplayDialogs.ProjectNotConfigured();
                    switch (selectedOption)
                    {
                        case 0:
                            ProjectSetupAssistant.Open();
                            break;
                        case 1:
                            break;
                        case 2:
                            editorProperties.VerificationEachCompile(false);
                            break;
                    }
                }
            }
        }
    }
}