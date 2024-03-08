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
    [UPMItem("Mobile SDK", 1, ItemType.Integration)]
    public sealed class MobileUItem
    {
        private const string URL = "https://assetstore.unity.com/packages/templates/systems/unreal-fps-mobile-132782";

        private static Texture2D logo;

        /// <summary>
        /// This function is called when the ItemManager window becomes enabled and active.
        /// </summary>
        public static void OnEnable()
        {
            logo = UEditorResourcesHelper.GetIcon("Logo Short");
        }


        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        public static void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (logo != null)
                GUILayout.Label(logo, GUILayout.Width(450));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Label(UnrealFPSInfo.NAME + " Mobile not downloaded.\nAll " + UnrealFPSInfo.NAME + " customers can download " + UnrealFPSInfo.NAME + " Mobile for free.", UEditorStyles.CenteredLabel);

            GUILayout.Space(5);

            UEditor.HorizontalLine();

            GUILayout.Space(3);


            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(" Open in Editor "))
            {
                UnityEditorInternal.AssetStore.Open(URL);
            }
            GUILayout.Space(10);
            if (GUILayout.Button(" Open in Browser "))
            {
                Application.OpenURL(URL);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(5);

            GUILayout.EndVertical();
        }
    }
}