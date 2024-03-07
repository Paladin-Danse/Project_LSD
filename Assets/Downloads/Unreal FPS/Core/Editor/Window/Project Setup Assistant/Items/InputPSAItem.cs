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
    [PSAItem("Inputs", 3, ItemType.Important)]
    public class InputPSAItem
    {
        private const string TNC_PATH = "Assets/Unreal FPS/Core/Runtime/Name Convention/INC.cs";


        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        public static void OnGUI()
        {
            string[] missingAxes = UEditorInternal.GetMissingInput("Axes");
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.Label("Setup Axes", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(10);
            GUILayout.Label(UnrealFPSInfo.NAME + " uses special axes that must be declared in the project settings.", UEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label(UnrealFPSInfo.NAME + " uses the following axes:", UEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label("[" + string.Join(", ", INC.GetAxes()) + "]", UEditorStyles.CenteredBoldLabelWrap);
            GUILayout.Space(10);

            if (missingAxes != null && missingAxes.Length > 0)
            {
                GUILayout.Label("Add the following axes:", UEditorStyles.LabelWrap);
                GUILayout.Space(5);
                GUILayout.Label("[" + string.Join(", ", missingAxes) + "]", UEditorStyles.CenteredBoldLabelWrap);

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
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Input");
#endif
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if ((missingAxes != null && missingAxes.Length == 0) || missingAxes == null)
            {
                GUILayout.Space(3);
                UEditor.HorizontalLine(0.5f);
                GUILayout.Space(3);

                GUILayout.Label("Axes setup complite!", UEditorStyles.CenteredBoldLabelWrap);
            }
            GUILayout.Space(5);
            GUILayout.EndVertical();

            string[] missingButtons = UEditorInternal.GetMissingInput("Buttons");
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.Label("Setup Buttons", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(10);
            GUILayout.Label(UnrealFPSInfo.NAME + " uses special buttons that must be declared in the project settings.", UEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label(UnrealFPSInfo.NAME + " uses the following buttons:", UEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label("[" + string.Join(", ", INC.GetButtons()) + "]", UEditorStyles.CenteredBoldLabelWrap);
            GUILayout.Space(10);

            if (missingButtons != null && missingButtons.Length > 0)
            {
                GUILayout.Label("Add the following buttons:", UEditorStyles.LabelWrap);
                GUILayout.Space(5);
                GUILayout.Label("[" + string.Join(", ", missingButtons) + "]", UEditorStyles.CenteredBoldLabelWrap);

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
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Input");
#endif
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if ((missingButtons != null && missingButtons.Length == 0) || missingButtons == null)
            {
                GUILayout.Space(3);
                UEditor.HorizontalLine();
                GUILayout.Space(3);

                GUILayout.Label("Buttons setup complite!", UEditorStyles.CenteredBoldLabelWrap);
            }
            GUILayout.Space(5);
            GUILayout.EndVertical();



            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.Label("Edit Inputs", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(10);
            GUILayout.Label("You can change default inputs. To do this, go to the INC file. The default location of the file \"Unreal FPS > Core > Runtime > Name Convention > INC\"", UEditorStyles.CenteredLabelWrap);
            GUILayout.Space(5);
            UEditor.HorizontalLine(0.5f);
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(" Edit INC "))
            {
                if (System.IO.File.Exists(TNC_PATH))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(TNC_PATH, 15);
                }
                else
                {
                    UDisplayDialogs.Message("Input Name Convention", "INC file not found, may be location changed try find it manually from Project window by Search > [INC]");
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.EndVertical();
        }

        public static bool IsReady()
        {
            string[] missingInputs = UEditorInternal.GetMissingInput("All");
            if (missingInputs == null && missingInputs.Length == 0)
            {
                return true;
            }
            else if (missingInputs != null && missingInputs.Length > 0)
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