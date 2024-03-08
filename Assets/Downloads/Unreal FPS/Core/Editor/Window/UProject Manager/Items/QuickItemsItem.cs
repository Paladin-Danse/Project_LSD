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
    [UPMItem("Quick Items", 5, ItemType.General)]
    public sealed class QuickItems
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent Desc = new GUIContent("Fast access items that will be created through the MenuItem tabs.");
            public readonly static GUIContent PlayerItem = new GUIContent("Player");
            public readonly static GUIContent AIItem = new GUIContent("AI");
            public readonly static GUIContent WeaponItem = new GUIContent("Weapon");
            public readonly static GUIContent WeaponAmmoItem = new GUIContent("Ammo");
            public readonly static GUIContent SpawZoneItem = new GUIContent("Spawn Zone");
        }

        private static UMenuItemsProperties menuItemsProperties;
        private static bool[] menuitems = new bool[5];

        /// <summary>
        /// 
        /// </summary>
        public static void OnEnable()
        {
            if (menuItemsProperties == null)
                menuItemsProperties = UEditorResourcesHelper.GetMenuItemsProperties();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void OnGUI()
        {
            if (menuItemsProperties != null)
            {
                GUILayout.Label(ContentProperties.Desc, UEditorStyles.CenteredBoldLabel);
                GUILayout.Space(5);
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Space(3);
                menuitems[0] = EditorGUILayout.Foldout(menuitems[0], ContentProperties.PlayerItem, true);
                if (menuitems[0])
                {
                    EditorGUI.indentLevel++;
                    UMenuItemsProperties.PlayerProperties playerProperties = menuItemsProperties.GetPlayerProperties();
                    playerProperties.player = (GameObject) EditorGUILayout.ObjectField("Object", playerProperties.player, typeof(GameObject), false);
                    playerProperties.position = EditorGUILayout.Vector3Field("Position", playerProperties.position);
                    playerProperties.rotation = EditorGUILayout.Vector3Field("Position", playerProperties.rotation);
                    menuItemsProperties.SetPlayerProperties(playerProperties);
                    EditorGUI.indentLevel--;
                }
                GUILayout.Space(3);
                GUILayout.EndVertical();

                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Space(3);
                menuitems[1] = EditorGUILayout.Foldout(menuitems[1], ContentProperties.WeaponItem, true);
                if (menuitems[1])
                {
                    EditorGUI.indentLevel++;
                    UMenuItemsProperties.WeaponProperties weaponProperties = menuItemsProperties.GetWeaponProperties();
                    weaponProperties.weapon = (GameObject) EditorGUILayout.ObjectField("Object", weaponProperties.weapon, typeof(GameObject), false);
                    weaponProperties.position = EditorGUILayout.Vector3Field("Position", weaponProperties.position);
                    weaponProperties.rotation = EditorGUILayout.Vector3Field("Position", weaponProperties.rotation);
                    menuItemsProperties.SetWeaponProperties(weaponProperties);
                    EditorGUI.indentLevel--;
                }
                GUILayout.Space(3);
                GUILayout.EndVertical();

                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Space(3);
                menuitems[2] = EditorGUILayout.Foldout(menuitems[2], ContentProperties.WeaponAmmoItem, true);
                if (menuitems[2])
                {
                    EditorGUI.indentLevel++;
                    UMenuItemsProperties.WeaponAmmoProperties weaponAmmoProperties = menuItemsProperties.GetWeaponAmmoProperties();
                    weaponAmmoProperties.weaponAmmo = (GameObject) EditorGUILayout.ObjectField("Object", weaponAmmoProperties.weaponAmmo, typeof(GameObject), false);
                    weaponAmmoProperties.position = EditorGUILayout.Vector3Field("Position", weaponAmmoProperties.position);
                    weaponAmmoProperties.rotation = EditorGUILayout.Vector3Field("Position", weaponAmmoProperties.rotation);
                    menuItemsProperties.SetWeaponAmmoProperties(weaponAmmoProperties);
                    EditorGUI.indentLevel--;
                }
                GUILayout.Space(3);
                GUILayout.EndVertical();

                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Space(3);
                menuitems[3] = EditorGUILayout.Foldout(menuitems[3], ContentProperties.AIItem, true);
                if (menuitems[3])
                {
                    EditorGUI.indentLevel++;
                    UMenuItemsProperties.AIProperties aiProperties = menuItemsProperties.GetAIProperties();
                    aiProperties.ai = (GameObject) EditorGUILayout.ObjectField("Object", aiProperties.ai, typeof(GameObject), false);
                    aiProperties.position = EditorGUILayout.Vector3Field("Position", aiProperties.position);
                    aiProperties.rotation = EditorGUILayout.Vector3Field("Position", aiProperties.rotation);
                    menuItemsProperties.SetAIProperties(aiProperties);
                    EditorGUI.indentLevel--;
                }
                GUILayout.Space(3);
                GUILayout.EndVertical();

                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Space(3);
                menuitems[4] = EditorGUILayout.Foldout(menuitems[4], ContentProperties.SpawZoneItem, true);
                if (menuitems[4])
                {
                    EditorGUI.indentLevel++;
                    UMenuItemsProperties.SpawnZoneProperties spawnZoneProperties = menuItemsProperties.GetSpawnZoneProperties();
                    spawnZoneProperties.spawnZone = (GameObject) EditorGUILayout.ObjectField("Object", spawnZoneProperties.spawnZone, typeof(GameObject), false);
                    spawnZoneProperties.position = EditorGUILayout.Vector3Field("Position", spawnZoneProperties.position);
                    spawnZoneProperties.rotation = EditorGUILayout.Vector3Field("Position", spawnZoneProperties.rotation);
                    menuItemsProperties.SetSpawnZoneProperties(spawnZoneProperties);
                    EditorGUI.indentLevel--;
                }
                GUILayout.Space(3);
                GUILayout.EndVertical();

                if (GUI.changed)
                    EditorUtility.SetDirty(menuItemsProperties);
            }
            else
            {
                UEditorHelpBoxMessages.Error("Item Properties not found.", "Create UItemsProperties asset from Resources/" + UEditorResourcesHelper.PROPETIES_PATH + "Menu Items Properties and move it in Unreal FPS/Resources/Editor/Properties folder.");
            }
        }
    }
}