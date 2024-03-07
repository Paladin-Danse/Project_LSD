/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnrealFPS.Editor.ProjectSetupAssistant;

namespace UnrealFPS.Editor
{
    [PSAItem("Layers", 2, ItemType.Important)]
    public class LayerPSAItem : MonoBehaviour
    {
        private const string LNC_PATH = "Assets/Unreal FPS/Core/Runtime/Name Convention/LNC.cs";

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        public static void OnGUI()
        {
            string[] missingLayers = UEditorInternal.GetMissingLayers();
            string[] requireLayers = LNC.GetLayers();

            if (requireLayers != null)
            {
                for (int i = 0; i < requireLayers.Length; i++)
                {
                    requireLayers[i] = (i + 8).ToString() + "-" + requireLayers[i];
                }
            }

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.Label("Setup layers", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(10);
            GUILayout.Label(UnrealFPSInfo.NAME + " uses special layers that must be declared in the project settings.", UEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label("Pay attention on layer number!", UEditorStyles.CenteredBoldLabel);
            GUILayout.Space(5);
            GUILayout.Label(UnrealFPSInfo.NAME + " uses the following layers:");
            GUILayout.Space(5);
            GUILayout.Label("[" + string.Join("], [", requireLayers) + "]", UEditorStyles.CenteredBoldLabelWrap);
            GUILayout.Space(10);

            if (missingLayers != null && missingLayers.Length > 0)
            {
                GUILayout.Label("Add the following layers:");
                GUILayout.Space(5);
                GUILayout.Label("[" + string.Join(", ", missingLayers) + "]", UEditorStyles.CenteredBoldLabelWrap);

                GUILayout.Space(5);
                UEditor.HorizontalLine();
                GUILayout.Space(3);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(" Add Manually "))
                {
#if UNITY_2018_1_OR_NEWER
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings...");
#else
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags and Layers");
#endif
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if ((missingLayers != null && missingLayers.Length == 0) || missingLayers == null)
            {
                GUILayout.Space(3);
                UEditor.HorizontalLine();
                GUILayout.Space(3);

                GUILayout.Label("Layers setup complite!", UEditorStyles.CenteredBoldLabelWrap);
            }
            GUILayout.Space(5);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.Label("Edit layers", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(10);
            GUILayout.Label("You can change default layers. To do this, go to the LNC file. The default location of the file \"Unreal FPS > Core > Runtime > Name Convention > LNC\"", UEditorStyles.CenteredLabelWrap);
            GUILayout.Space(5);
            UEditor.HorizontalLine(0.5f);
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(" Edit LNC "))
            {
                if (System.IO.File.Exists(LNC_PATH))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(LNC_PATH, 15);
                }
                else
                {
                    UDisplayDialogs.Message("Layers Name Convention", "LNC file not found, may be location changed try find it manually from Project window by Search > [LNC]");
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.EndVertical();
        }

        public static bool IsReady()
        {
            string[] missingLayers = UEditorInternal.GetMissingLayers();
            if (missingLayers == null && missingLayers.Length == 0)
            {
                return true;
            }
            else if (missingLayers != null && missingLayers.Length > 0)
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