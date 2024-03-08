/* ================================================================
   ---------------------------------------------------
   Project   :    #0001
   Publisher :    #0002
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;
using UnrealFPS.Runtime;
using UnrealFPS.UI;

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(PickableItem))]
    [CanEditMultipleObjects]
    public class PickableItemEditor : UEditor<PickableItem>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseSettings = new GUIContent("Base Settings");
            public readonly static GUIContent ItemProperties = new GUIContent("Item Properties");
            public readonly static GUIContent Effects = new GUIContent("Effects");
            public readonly static GUIContent UI = new GUIContent("UI Properties");

            public readonly static GUIContent Item = new GUIContent("Item", "Item transform.");
            public readonly static GUIContent PickupType = new GUIContent("Pickup Type", "Pickable system processing type.");
            public readonly static GUIContent ObjecftType = new GUIContent("Object Type", "Pickable object type.");
            public readonly static GUIContent Target = new GUIContent("Target", "How can pick up this object.");
            public readonly static GUIContent PickupKey = new GUIContent("Button", "Button for pickup obejct.");
            public readonly static GUIContent Weapon = new GUIContent("Weapon", "Weapon ID asset.");
            public readonly static GUIContent AutoActivate = new GUIContent("Auto Activate", "Automatically activate new pickup weapon.");
            public readonly static GUIContent IsReusable = new GUIContent("Is Reusable", "This object is reusable.");
            public readonly static GUIContent ReusableDelay = new GUIContent("Reusable Delay", "Reusable delay (in seconds).");
            public readonly static GUIContent DestroyAfterUse = new GUIContent("Destroy After Use", "Destroy object after used.");
            public readonly static GUIContent SoundEffect = new GUIContent("Sound Effect", "Sound effect that will player when pickup.");
            public readonly static GUIContent PickUpMessage = new GUIContent("Pick Up Message", "Message that will displayed on HUD, when player pickup new weapon.");
            public readonly static GUIContent ReplaceMessage = new GUIContent("Replace Message", "Message that will displayed on HUD, when player already have active.");

        }

        private bool useSoundEffect;

        public override void InitializeProperties()
        {
            useSoundEffect = instance.GetSoundEffect() != null;
        }

        public override void BaseGUI()
        {
            BeginBox();
            GUILayout.Label(ContentProperties.BaseSettings, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            instance.SetProcessingType((PickableItem.PickupType) EditorGUILayout.EnumPopup(ContentProperties.PickupType, instance.GetProcessingType()));
            instance.SetObjectType((PickableItem.ItemType) EditorGUILayout.EnumPopup(ContentProperties.ObjecftType, instance.GetObjectType()));

            if (instance.GetProcessingType() == PickableItem.PickupType.ByKey)
            {
                instance.SetPickUpKey((KeyCode) EditorGUILayout.EnumPopup(ContentProperties.PickupKey, instance.GetPickUpKey()));
            }

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.ItemProperties, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            instance.SetItem((Transform) EditorGUILayout.ObjectField(ContentProperties.Item, instance.GetItem(), typeof(Transform), true));
            if (instance.GetItem() == null)
            {
                UEditorHelpBoxMessages.Error("Item cannot be empty!", "Add item game object!");
            }
            switch (instance.GetObjectType())
            {
                case PickableItem.ItemType.Weapon:
                    instance.SetWeapon((WeaponID) EditorGUILayout.ObjectField(ContentProperties.Weapon, instance.GetWeapon(), typeof(WeaponID), true));
                    instance.AutoActivate(EditorGUILayout.Toggle(ContentProperties.AutoActivate, instance.AutoActivate()));
                    break;
                case PickableItem.ItemType.Ammo:
                    instance.SetWeapon((WeaponID) EditorGUILayout.ObjectField(new GUIContent("Target Weapon", "Target weapon for ammo."), instance.GetWeapon(), typeof(WeaponID), true));
                    instance.SetValue(EditorGUILayout.IntField(new GUIContent("Bullets", "How many bullet add in weapon ammo."), instance.GetValue()));
                    break;
                case PickableItem.ItemType.HealthBox:
                    instance.SetValue(EditorGUILayout.IntField(new GUIContent("Health Point", "How many health point add in target."), instance.GetValue()));
                    break;
            }
            if (!instance.DestroyAfterUse())
            {
                float refReusableDelay = instance.GetReusableDelay();
                bool refIsReusable = instance.IsReusable();
                HiddenFloatField(ContentProperties.ReusableDelay, ContentProperties.IsReusable, ref refReusableDelay, ref refIsReusable);
                instance.SetReusableDelay(refReusableDelay);
                instance.IsReusable(refIsReusable);
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                instance.IsReusable(EditorGUILayout.Toggle(ContentProperties.IsReusable, false));
                EditorGUI.EndDisabledGroup();
            }

            if (!instance.IsReusable())
            {
                instance.DestroyAfterUse(EditorGUILayout.Toggle(ContentProperties.DestroyAfterUse, instance.DestroyAfterUse()));
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                instance.DestroyAfterUse(EditorGUILayout.Toggle(ContentProperties.DestroyAfterUse, false));
                EditorGUI.EndDisabledGroup();
            }

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.UI, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);

            instance.SetPickUpMessage(EditorGUILayout.TextField(ContentProperties.PickUpMessage, instance.GetPickUpMessage()));
            instance.SetReplaceMessage(EditorGUILayout.TextField(ContentProperties.ReplaceMessage, instance.GetReplaceMessage()));

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.Effects, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            AudioClip se = instance.GetSoundEffect();
            ObjectHiddenField<AudioClip>(ContentProperties.SoundEffect, ContentProperties.SoundEffect, ref se, ref useSoundEffect);
            instance.SetSoundEffect(useSoundEffect ? se : null);

            EndBox();
        }
    }
}