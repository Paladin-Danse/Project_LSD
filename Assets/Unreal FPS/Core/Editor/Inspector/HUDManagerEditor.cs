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
using UnityEngine.UI;
using UnrealFPS.Runtime;
using UnrealFPS.UI;

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(HUDManager))]
    [CanEditMultipleObjects]
    public class HUDManagerEditor : UEditor<HUDManager>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent RequireProperties = new GUIContent("Base Properties");
            public readonly static GUIContent UIComponents = new GUIContent("UI Components");
            public readonly static GUIContent HUDWindow = new GUIContent("HUD Window", "Base HUD window background.");

            public readonly static GUIContent Player = new GUIContent("Player");

            public readonly static GUIContent HealthScrollbar = new GUIContent("Health Scrollbar");
            public readonly static GUIContent HealthPoint = new GUIContent("Healt hPoint");
            public readonly static GUIContent BulletCount = new GUIContent("Bullet Count");
            public readonly static GUIContent ClipCount = new GUIContent("Clip Count");
            public readonly static GUIContent WeaponName = new GUIContent("Weapon Name");
            public readonly static GUIContent WeaponImage = new GUIContent("Weapon Image");
            public readonly static GUIContent MessageWindow = new GUIContent("Message Window");
            public readonly static GUIContent Message = new GUIContent("Message");

            public readonly static GUIContent AutoFindButton = new GUIContent("Find Auto");
        }

        public override string HeaderName()
        {
            return "HUD Manager";
        }

        public override void BaseGUI()
        {
            BeginBox();
            GUILayout.Label(ContentProperties.RequireProperties, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);

            if (instance.GetPlayer() != null)
            {
                instance.SetPlayer(ObjectField<Transform>(ContentProperties.Player, instance.GetPlayer(), true));
            }
            else
            {
                GUILayout.BeginHorizontal();
                instance.SetPlayer(ObjectField<Transform>(ContentProperties.Player, instance.GetPlayer(), true));
                if (SearchButton())
                {
                    Transform camera = UEditorInternal.FindPlayer();
                    if (camera != null)
                    {
                        instance.SetPlayer(camera);
                    }
                    else
                    {
                        UDisplayDialogs.Message("Searching", "Player not found, try find it manually.");
                    }
                }
                GUILayout.EndHorizontal();
            }

            if (instance.GetPlayer() != null)
            {
                IHealth health = instance.GetPlayer().GetComponent<IHealth>();
                IInventory inventory = instance.GetPlayer().GetComponent<IInventory>();
                if (health == null || inventory == null)
                {
                    UEditorHelpBoxMessages.Error(
                        string.Format("Player object not contain required component{0} ({1}{2})!",
                            health == null && inventory == null ? "s" : "",
                            health == null ? "Health, " : "",
                            inventory == null ? "Inventory" : ""),
                        "Add required components or change Player object.");
                }
            }
            else
            {
                UEditorHelpBoxMessages.Error("Player is empty!", "Add player obejct!");
            }

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.UIComponents, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            HUDComponents hudComponents = instance.GetHUDComponents();
            hudComponents.hudWindow = (RectTransform) EditorGUILayout.ObjectField(ContentProperties.HUDWindow, hudComponents.hudWindow, typeof(RectTransform), true);
            hudComponents.healthScrollbar = (Scrollbar) EditorGUILayout.ObjectField(ContentProperties.HealthScrollbar, hudComponents.healthScrollbar, typeof(Scrollbar), true);
            hudComponents.healthPoint = (Text) EditorGUILayout.ObjectField(ContentProperties.HealthPoint, hudComponents.healthPoint, typeof(Text), true);
            hudComponents.bulletCount = (Text) EditorGUILayout.ObjectField(ContentProperties.BulletCount, hudComponents.bulletCount, typeof(Text), true);
            hudComponents.clipCount = (Text) EditorGUILayout.ObjectField(ContentProperties.ClipCount, hudComponents.clipCount, typeof(Text), true);
            hudComponents.weaponName = (Text) EditorGUILayout.ObjectField(ContentProperties.WeaponName, hudComponents.weaponName, typeof(Text), true);
            hudComponents.weaponImage = (Image) EditorGUILayout.ObjectField(ContentProperties.WeaponImage, hudComponents.weaponImage, typeof(Image), true);

            hudComponents.messageWindow = (RectTransform) EditorGUILayout.ObjectField(ContentProperties.MessageWindow, hudComponents.messageWindow, typeof(RectTransform), true);
            hudComponents.message = (Text) EditorGUILayout.ObjectField(ContentProperties.Message, hudComponents.message, typeof(Text), true);

            instance.SetComponents(hudComponents);
            EndBox();

        }
    }
}