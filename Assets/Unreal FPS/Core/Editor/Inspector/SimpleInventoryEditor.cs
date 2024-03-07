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
using UnityEngine;
using UnrealFPS.Runtime;

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(SimpleInventory))]
    public class SimpleInventoryEditor : UEditor<SimpleInventory>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseSettings = new GUIContent("Base Settings");
            public readonly static GUIContent Weapons = new GUIContent("Weapons");
            public readonly static GUIContent Camera = new GUIContent("Camera", "First Person Camera");
            public readonly static GUIContent Switch = new GUIContent("Switch Mode");
            public readonly static GUIContent AllowIdenticalWeapons = new GUIContent("Allow Identical Weapons");

            public readonly static GUIContent AddWeaponButton = new GUIContent(" Add Weapon ", "Add new weapon in group.");
            public readonly static GUIContent AddGroupButton = new GUIContent(" Add Group ", "Add new group in inventory.");
            public readonly static GUIContent RemoveGroupButton = new GUIContent(" Remove Group ", "Remove group from inventory.");
            public readonly static GUIContent EditGroupButton = new GUIContent(" Edit ", "Edit inventory groups.");
            public readonly static GUIContent ApplyChangesButton = new GUIContent(" Apply ", "Apply all changes.");
        }

        public override string HeaderName()
        {
            return "First Person Inventory";
        }

        private SerializedProperty slotsProperty;
        private bool weaponsEditFoldout;
        private List<bool> radioToggles;
        private int savedIndex;

        public override void InitializeProperties()
        {
            slotsProperty = serializedObject.FindProperty("slots");
            radioToggles = CreateRadioToggles();
        }

        public override void BaseGUI()
        {
            BeginBox();
            GUILayout.Label(ContentProperties.BaseSettings, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);

            if (instance.GetFPCamera() != null)
            {
                instance.SetFPCamera((Transform)EditorGUILayout.ObjectField(ContentProperties.Camera, instance.GetFPCamera(), typeof(Transform), true));
            }
            else
            {
                GUILayout.BeginHorizontal();
                instance.SetFPCamera((Transform)EditorGUILayout.ObjectField(ContentProperties.Camera, instance.GetFPCamera(), typeof(Transform), true));
                if (SearchButton())
                {
                    Camera camera = UEditorInternal.FindFPCamera(instance.transform);
                    if (camera != null)
                    {
                        instance.SetFPCamera(camera.transform);
                    }
                    else
                    {
                        UDisplayDialogs.Message("Searching", "Camera not found, try find it manually.");
                    }
                }
                GUILayout.EndHorizontal();
                UEditorHelpBoxMessages.CameraError();
            }
            instance.SetSwitchMode((SwitchWeaponMode)EditorGUILayout.EnumPopup(ContentProperties.Switch, instance.GetSwitchMode()));
            instance.AllowIdenticalWeapons(EditorGUILayout.Toggle(ContentProperties.AllowIdenticalWeapons, instance.AllowIdenticalWeapons()));
            EndBox();

            BeginBox(3);
            IncreaseIndentLevel();
            weaponsEditFoldout = EditorGUILayout.Foldout(weaponsEditFoldout, ContentProperties.Weapons, true);
            if (weaponsEditFoldout)
            {
                GUILayout.Space(5);
                GUILayout.BeginVertical(GUI.skin.window);
                for (int i = 0, length = slotsProperty.arraySize; i < length; i++)
                {
                    SerializedProperty slot = slotsProperty.GetArrayElementAtIndex(i);
                    InventorySlot _slot = instance.GetSlots()[i];
                    string slotName = _slot.GetWeapon() != null ? _slot.GetWeapon().name : "Empty Slot " + (i + 1);
                    GUILayout.BeginHorizontal();
                    EditorGUI.BeginDisabledGroup(_slot.GetWeapon() == null || _slot.GetKey() == KeyCode.None);
                    radioToggles[i] = EditorGUILayout.Toggle(radioToggles[i], EditorStyles.radioButton, GUILayout.Width(15));
                    if (radioToggles[i])
                    {
                        int except = !(_slot.GetKey() == KeyCode.None) ? i : -1;
                        SetToggleValues(except, false);
                        savedIndex = i;
                    }
                    EditorGUI.EndDisabledGroup();
                    GUILayout.Label(slotName);
                    EditorGUILayout.PropertyField(slot.FindPropertyRelative("key"), GUIContent.none, GUILayout.MaxWidth(110));
                    EditorGUILayout.PropertyField(slot.FindPropertyRelative("weapon"), GUIContent.none);
                    GUILayout.BeginVertical();
                    GUILayout.Space(0.7f);
                    if (GUILayout.Button("", GUI.skin.GetStyle("OL Minus"), GUILayout.Width(20)))
                    {
                        slotsProperty.DeleteArrayElementAtIndex(i);
                        break;
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(5);
                if (savedIndex != -1 && GUILayout.Button("Disable Start Weapon", EditorStyles.miniButton, GUILayout.Height(17)))
                {
                    savedIndex = -1;
                }

                if (MiniButton(ContentProperties.AddWeaponButton, "Right", GUILayout.Height(17)))
                {
                    instance.GetSlots().Add(new InventorySlot(KeyCode.None, null));
                    radioToggles.Add(false);
                }
                GUILayout.Space(3);
                GUILayout.EndVertical();
                GUILayout.Space(5);
            }
            DecreaseIndentLevel();
            EndBox();
            ApplySavedToggle();
        }

        private List<bool> CreateRadioToggles()
        {
            List<bool> toggles = new List<bool>();
            for (int i = 0, length = slotsProperty.arraySize; i < length; i++)
            {
                toggles.Add(false);
            }
            return toggles;
        }

        private void ApplySavedToggle()
        {
            if (savedIndex > instance.GetSlots().Count - 1)
            {
                SetToggleValues(-1, false);
                instance.SetStartWeaponKey(KeyCode.None);
                savedIndex = -1;
            }
            else if (savedIndex >= 0)
            {
                InventorySlot _slot = instance.GetSlots()[savedIndex];
                instance.SetStartWeaponKey(_slot.GetKey());
            }
        }
        private void SetToggleValues(int except, bool isActive)
        {
            for (int i = 0, length = radioToggles.Count; i < length; i++)
            {
                if (except != i)
                {
                    radioToggles[i] = isActive;
                }
            }
        }
    }
}