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
    [CustomEditor(typeof(UserHelper))]
    public class UserHelperEditor : UEditor<UserHelper>
    {
        public override void BaseGUI()
        {
            BeginBox();
            GUILayout.Label("Documetation", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            if (GUILayout.Button(" Online Documetation ", GUILayout.Height(21)))
            {
                UserHelper.OpenOnlineDocumentation();
            }
            EndBox();

            GUILayout.Space(5);

            BeginBox();
            GUILayout.Label("Forums", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!UserHelper.NetworkIsAvailable());
            if (GUILayout.Button(" Official Thread ", GUILayout.Height(21)))
            {
                UserHelper.OpenOfficialThread();
            }
            if (GUILayout.Button(" Discord Channel ", GUILayout.Height(21)))
            {
                UserHelper.OpenDiscordChannel();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            EndBox();

            BeginBox();
            GUILayout.Label("Social networking services", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            if (GUILayout.Button(" Twitter ", GUILayout.Height(21)))
            {
                UserHelper.OpenTwitter();
            }
            EndBox();
        }
    }
}