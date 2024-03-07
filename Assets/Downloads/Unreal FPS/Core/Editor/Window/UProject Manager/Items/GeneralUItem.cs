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
using static UnrealFPS.Editor.UProjectManager;

namespace UnrealFPS.Editor
{
    [UPMItem("General", 0, ItemType.General)]
    public sealed class GeneralUItem
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent BaseOptions = new GUIContent("Base Options");
            public readonly static GUIContent VerificationProject = new GUIContent("Check Project (Current Session)", "Verification project settings on errors each compile.");
            public readonly static GUIContent PropertiesNull = new GUIContent("Editor Properties not found...\nCheck Editor Properties asset contatins in Resources/" + UEditorResourcesHelper.PROPETIES_PATH);

            public readonly static GUIContent GUIColor = new GUIContent("Inspector GUI Colors");
            public readonly static GUIContent HeaderColor = new GUIContent("Header Color");
            public readonly static GUIContent WindowColor = new GUIContent("Window Color");
            public readonly static GUIContent BackgronudColor = new GUIContent("Backgronud Color");
            public readonly static GUIContent BoxColor = new GUIContent("Box Color");
            public readonly static GUIContent SubBoxColor = new GUIContent("Sub Box Color");
        }

        private static UEditorProperties editorProperties;
        private static UEditorGUIProperties editorGUIProperties;

        /// <summary>
        /// 
        /// </summary>
        public static void OnEnable()
        {
            if (editorProperties == null)
                editorProperties = UEditorResourcesHelper.GetEditorProperties();
            if (editorGUIProperties == null)
                editorGUIProperties = UEditorResourcesHelper.GetEditorGUIProperties();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void OnGUI()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.Label(ContentProperties.BaseOptions, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(5);
            if (editorProperties != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.VerificationProject, GUILayout.Width(200));
                editorProperties.VerificationEachCompile(EditorGUILayout.Toggle(editorProperties.VerificationEachCompile()));
                GUILayout.EndHorizontal();
                if (GUI.changed)
                    EditorUtility.SetDirty(editorProperties);
            }
            else
            {
                GUILayout.Label(ContentProperties.PropertiesNull);
            }

            GUILayout.Space(10);

            GUILayout.Label(ContentProperties.GUIColor, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(10);

            if (editorGUIProperties != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.HeaderColor, GUILayout.Width(125));
                editorGUIProperties.SetHeaderColor(EditorGUILayout.ColorField(editorGUIProperties.GetHeaderColor()));
                if (GUILayout.Button("Reset", GUILayout.Width(70)))
                {
                    editorGUIProperties.SetHeaderColor(Color.white);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.WindowColor, GUILayout.Width(125));
                editorGUIProperties.SetWindowColor(EditorGUILayout.ColorField(editorGUIProperties.GetWindowColor()));
                if (GUILayout.Button("Reset", GUILayout.Width(70)))
                {
                    editorGUIProperties.SetWindowColor(Color.gray);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.BackgronudColor, GUILayout.Width(125));
                editorGUIProperties.SetBoxBackgroundColor(EditorGUILayout.ColorField(editorGUIProperties.GetBoxBackgroundColor()));
                if (GUILayout.Button("Reset", GUILayout.Width(70)))
                {
                    editorGUIProperties.SetBoxBackgroundColor(Color.white);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.BoxColor, GUILayout.Width(125));
                editorGUIProperties.SetBoxColor(EditorGUILayout.ColorField(editorGUIProperties.GetBoxColor()));
                if (GUILayout.Button("Reset", GUILayout.Width(70)))
                {
                    editorGUIProperties.SetBoxColor(Color.white);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.SubBoxColor, GUILayout.Width(125));
                editorGUIProperties.SetSubBboxColor(EditorGUILayout.ColorField(editorGUIProperties.GetSubBboxColor()));
                if (GUILayout.Button("Reset", GUILayout.Width(70)))
                {
                    editorGUIProperties.SetSubBboxColor(new Color32(210, 210, 210, 200));
                }
                GUILayout.EndHorizontal();

                if (GUI.changed)
                    EditorUtility.SetDirty(editorGUIProperties);

            }
            else
            {
                UEditorHelpBoxMessages.Error("Editor GUI Properties not found.", "Create UEditorGUIProperties asset from " + UEditorPaths.EDITOR + "Editor GUI Properties and move it in Unreal FPS/Resources/Editor/Properties folder.");
            }
            GUILayout.Space(5);
            GUILayout.EndVertical();
        }
    }
}