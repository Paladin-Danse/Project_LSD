/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnrealFPS.Editor.ProjectSetupAssistant;

namespace UnrealFPS.Editor
{
    [PSAItem("Tags", 1, ItemType.Important)]
    public class TagPSAItem
    {
        private const string TNC_PATH = "Assets/Unreal FPS/Core/Runtime/Name Convention/TNC.cs";


        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        public static void OnGUI()
        {
            string[] missingTags = UEditorInternal.GetMissingTags();
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.Label("Setup tags", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(10);
            GUILayout.Label(UnrealFPSInfo.NAME + " uses special tags that must be declared in the project settings.", UEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label(UnrealFPSInfo.NAME + " uses the following tags:", UEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label("[" + string.Join(", ", TNC.GetTags()) + "]", UEditorStyles.CenteredBoldLabelWrap);
            GUILayout.Space(10);

            if (missingTags != null && missingTags.Length > 0)
            {
                GUILayout.Label("Add the following tags:", UEditorStyles.LabelWrap);
                GUILayout.Space(5);
                GUILayout.Label("[" + string.Join(", ", missingTags) + "]", UEditorStyles.CenteredBoldLabelWrap);

                GUILayout.Space(5);
                UEditor.HorizontalLine();
                GUILayout.Space(3);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(" Add Manually ", "ButtonLeft"))
                {
#if UNITY_2018_1_OR_NEWER
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings...");
#else
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tag and Layers");
#endif
                }

                if (GUILayout.Button(" Add Auto ", "ButtonRight"))
                {
                    UEditorInternal.AddMissingTags();
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if ((missingTags != null && missingTags.Length == 0) || missingTags == null)
            {
                GUILayout.Space(3);
                UEditor.HorizontalLine(0.5f);
                GUILayout.Space(3);

                GUILayout.Label("Tag setup complite!", UEditorStyles.CenteredBoldLabelWrap);
            }
            GUILayout.Space(5);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.Label("Edit tags", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(10);
            GUILayout.Label("You can change default tags. To do this, go to the TNC file. The default location of the file \"Unreal FPS > Core > Runtime > Name Convention > TNC\"", UEditorStyles.CenteredLabelWrap);
            GUILayout.Space(5);
            UEditor.HorizontalLine();
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(" Edit TNC "))
            {
                if (System.IO.File.Exists(TNC_PATH))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(TNC_PATH, 15);
                }
                else
                {
                    UDisplayDialogs.Message("Tag Name Convention", "TNC file not found, may be location changed try find it manually from Project window by Search > [TNC]");
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.EndVertical();
        }

        public static bool IsReady()
        {
            string[] missingTags = UEditorInternal.GetMissingTags();
            if (missingTags == null && missingTags.Length == 0)
            {
                return true;
            }
            else if (missingTags != null && missingTags.Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}