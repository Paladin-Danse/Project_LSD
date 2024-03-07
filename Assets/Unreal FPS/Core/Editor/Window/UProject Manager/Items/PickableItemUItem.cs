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
using UnrealFPS.Runtime;
using static UnrealFPS.Editor.UProjectManager;

namespace UnrealFPS.Editor
{
    [UPMItem("Pickable Item", 4, ItemType.General)]
    public sealed class PickableItemUItem
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent Options = new GUIContent("Base Options");

            public readonly static GUIContent ItemProperties = new GUIContent("Item Properties");
            public readonly static GUIContent Effects = new GUIContent("Effects");
            public readonly static GUIContent UI = new GUIContent("UI Properties");

            public readonly static GUIContent Name = new GUIContent("Name", "Pickable item name.");
            public readonly static GUIContent Item = new GUIContent("Item", "Item transform.");
            public readonly static GUIContent PickupType = new GUIContent("Pickup Type", "Pickable system processing type.");
            public readonly static GUIContent ObjecftType = new GUIContent("Item Type", "Pickable item type.");
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

        internal sealed class PickableItemProperties
        {
            private const string DEFAULT_NAME = "Write item name here...";

            private string name;
            private GameObject item;
            private PickableItem.PickupType pickupType;
            private PickableItem.ItemType objectType;
            private string pickUpMessage;
            private string replaceMessage;
            private WeaponID weapon;
            private bool autoActivate;

            private int value;
            private bool isReusable;
            private float reusableDelay;
            private bool destroyAfterUse;
            private KeyCode pickUpKey;
            private AudioClip soundEffect;

            public PickableItemProperties()
            {
                name = DEFAULT_NAME;
            }

            public PickableItemProperties(string name)
            {
                this.name = name;
            }

            public string GetName()
            {
                return name;
            }

            public void SetName(string value)
            {
                name = value;
            }

            public void SetDefaultName()
            {
                name = DEFAULT_NAME;
            }

            public GameObject GetItem()
            {
                return item;
            }

            public void SetItem(GameObject value)
            {
                item = value;
            }

            public bool AutoActivate()
            {
                return autoActivate;
            }

            public void AutoActivate(bool value)
            {
                autoActivate = value;
            }

            public bool IsReusable()
            {
                return isReusable;
            }

            public void IsReusable(bool value)
            {
                isReusable = value;
            }

            public float GetReusableDelay()
            {
                return reusableDelay;
            }

            public void SetReusableDelay(float value)
            {
                reusableDelay = value;
            }

            public bool DestroyAfterUse()
            {
                return destroyAfterUse;
            }

            public void DestroyAfterUse(bool value)
            {
                destroyAfterUse = value;
            }

            public PickableItem.ItemType GetObjectType()
            {
                return objectType;
            }

            public void SetObjectType(PickableItem.ItemType value)
            {
                objectType = value;
            }

            public WeaponID GetWeapon()
            {
                return weapon;
            }

            public void SetWeapon(WeaponID value)
            {
                weapon = value;
            }

            public int GetValue()
            {
                return value;
            }

            public void SetValue(int value)
            {
                this.value = value;
            }

            public PickableItem.PickupType GetProcessingType()
            {
                return pickupType;
            }

            public void SetProcessingType(PickableItem.PickupType value)
            {
                pickupType = value;
            }

            public KeyCode GetPickUpKey()
            {
                return pickUpKey;
            }

            public void SetPickUpKey(KeyCode value)
            {
                pickUpKey = value;
            }

            public AudioClip GetSoundEffect()
            {
                return soundEffect;
            }

            public void SetSoundEffect(AudioClip value)
            {
                soundEffect = value;
            }

            public string GetPickUpMessage()
            {
                return pickUpMessage;
            }

            public void SetPickUpMessage(string value)
            {
                pickUpMessage = value;
            }

            public string GetReplaceMessage()
            {
                return replaceMessage;
            }

            public void SetReplaceMessage(string value)
            {
                replaceMessage = value;
            }

            public bool NameIsValid()
            {
                return !string.IsNullOrEmpty(name) && name != DEFAULT_NAME;
            }

            public static bool NameIsValid(string name)
            {
                return !string.IsNullOrEmpty(name) && name != DEFAULT_NAME;
            }
        }

        private static PickableItemProperties properties;
        private static GameObject pickableObject;
        private static IEditorDelay editorDelay;
        private static bool useSoundEffect;

        public static void OnEnable()
        {
            properties = new PickableItemProperties();
            useSoundEffect = properties.GetSoundEffect() != null;
            editorDelay = new EditorDelay(0.1f);
        }

        public static void OnGUI()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            GUILayout.Label(ContentProperties.Options, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);

            properties.SetName(EditorGUILayout.TextField(ContentProperties.Name, properties.GetName()));
            properties.SetProcessingType((PickableItem.PickupType) EditorGUILayout.EnumPopup(ContentProperties.PickupType, properties.GetProcessingType()));
            properties.SetObjectType((PickableItem.ItemType) EditorGUILayout.EnumPopup(ContentProperties.ObjecftType, properties.GetObjectType()));

            if (properties.GetProcessingType() == PickableItem.PickupType.ByKey)
            {
                properties.SetPickUpKey((KeyCode) EditorGUILayout.EnumPopup(ContentProperties.PickupKey, properties.GetPickUpKey()));
            }

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.ItemProperties, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            properties.SetItem(UEditor.ObjectField<GameObject>(ContentProperties.Item, properties.GetItem(), true));
            if (properties.GetItem() == null)
            {
                UEditorHelpBoxMessages.Error("Item cannot be empty!", "Add item game object!");
            }
            
            switch (properties.GetObjectType())
            {
                case PickableItem.ItemType.Weapon:
                    properties.SetWeapon(UEditor.ObjectField<WeaponID>(ContentProperties.Weapon, properties.GetWeapon(), true));
                    properties.AutoActivate(EditorGUILayout.Toggle(ContentProperties.AutoActivate, properties.AutoActivate()));
                    break;
                case PickableItem.ItemType.Ammo:
                    properties.SetWeapon(UEditor.ObjectField<WeaponID>(new GUIContent("Target Weapon", "Target weapon for ammo."), properties.GetWeapon(), true));
                    properties.SetValue(EditorGUILayout.IntField(new GUIContent("Bullets", "How many bullet add in weapon ammo."), properties.GetValue()));
                    break;
                case PickableItem.ItemType.HealthBox:
                    properties.SetValue(EditorGUILayout.IntField(new GUIContent("Health Point", "How many health point add in target."), properties.GetValue()));
                    break;
            }

            if (!properties.DestroyAfterUse())
            {
                float refReusableDelay = properties.GetReusableDelay();
                bool refIsReusable = properties.IsReusable();
                UEditor.HiddenFloatField(ContentProperties.ReusableDelay, ContentProperties.IsReusable, ref refReusableDelay, ref refIsReusable);
                properties.SetReusableDelay(refReusableDelay);
                properties.IsReusable(refIsReusable);
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                properties.IsReusable(EditorGUILayout.Toggle(ContentProperties.IsReusable, false));
                EditorGUI.EndDisabledGroup();
            }

            if (!properties.IsReusable())
            {
                properties.DestroyAfterUse(EditorGUILayout.Toggle(ContentProperties.DestroyAfterUse, properties.DestroyAfterUse()));
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                properties.DestroyAfterUse(EditorGUILayout.Toggle(ContentProperties.DestroyAfterUse, false));
                EditorGUI.EndDisabledGroup();
            }

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.UI, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);

            if (properties.GetWeapon() != null)
            {
                GUILayout.BeginHorizontal();
                properties.SetPickUpMessage(EditorGUILayout.TextField(ContentProperties.PickUpMessage, properties.GetPickUpMessage()));
                if (UEditor.GenerateButton())
                {
                    properties.SetPickUpMessage("HOLD [{0}] TO PICKUP \"{1}\"");
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                properties.SetPickUpMessage(EditorGUILayout.TextField(ContentProperties.PickUpMessage, properties.GetPickUpMessage()));
            }

            if (properties.GetWeapon() != null)
            {
                GUILayout.BeginHorizontal();
                properties.SetReplaceMessage(EditorGUILayout.TextField(ContentProperties.ReplaceMessage, properties.GetReplaceMessage()));
                if (UEditor.GenerateButton())
                {
                    properties.SetPickUpMessage("HOLD [{0}] TO CHANGE \"{1}\"");
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                properties.SetReplaceMessage(EditorGUILayout.TextField(ContentProperties.ReplaceMessage, properties.GetReplaceMessage()));
            }

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.Effects, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            AudioClip se = properties.GetSoundEffect();
            UEditor.ObjectHiddenField<AudioClip>(ContentProperties.SoundEffect, ContentProperties.SoundEffect, ref se, ref useSoundEffect);
            properties.SetSoundEffect(useSoundEffect ? se : null);

            GUILayout.Space(5);
            UEditor.HorizontalLine();
            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(!properties.NameIsValid() || properties.GetItem() == null);
            if (UEditor.Button("Create", "Right", GUILayout.Width(70)))
            {
                pickableObject = CreateObject(properties);
            }
            EditorGUI.EndDisabledGroup();

            if (pickableObject != null && editorDelay.WaitForSeconds())
            {
                if (UDisplayDialogs.Message("Create Successful", "Pickable item was created on scene!\nSetup item components before start play.", "Select", "Ok"))
                {
                    Selection.activeGameObject = pickableObject;
                }
                pickableObject = null;
            }

            GUILayout.Space(5);
            GUILayout.EndVertical();
        }

        private static GameObject CreateObject(PickableItemProperties properties)
        {
            // Initialize gameobjects.
            GameObject itemObject = new GameObject(properties.GetName());
            GameObject item = GameObject.Instantiate(properties.GetItem(), Vector3.zero, Quaternion.identity, itemObject.transform);

            // Initialize itemObject components.
            PickableItem pickupItem = UEditorInternal.AddComponent<PickableItem>(itemObject);
            Rigidbody physicsRigidbody = UEditorInternal.AddComponent<Rigidbody>(itemObject);
            AudioSource audioSource = UEditorInternal.AddComponent<AudioSource>(itemObject);
            SphereCollider triggerCollider = UEditorInternal.AddComponent<SphereCollider>(itemObject);
            BoxCollider physicsCollider = UEditorInternal.AddComponent<BoxCollider>(itemObject);

            // Setup PickableItem component.
            pickupItem.SetProcessingType(properties.GetProcessingType());
            pickupItem.SetObjectType(properties.GetObjectType());
            pickupItem.SetPickUpKey(properties.GetPickUpKey());
            pickupItem.SetItem(item.transform);
            pickupItem.SetWeapon(properties.GetWeapon());
            pickupItem.AutoActivate(properties.AutoActivate());
            pickupItem.SetValue(properties.GetValue());
            pickupItem.SetReusableDelay(properties.GetReusableDelay());
            pickupItem.IsReusable(properties.IsReusable());
            pickupItem.DestroyAfterUse(properties.DestroyAfterUse());
            pickupItem.SetPickUpMessage(properties.GetPickUpMessage());
            pickupItem.SetSoundEffect(properties.GetSoundEffect());

            // Setup SphereCollider component.
            triggerCollider.isTrigger = true;
            triggerCollider.radius = 1.25f;

            // Setup BoxCollider component.
            Renderer renderer = item.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                physicsCollider.center = renderer.bounds.center;
                physicsCollider.size = renderer.bounds.size;
            }

            // Apply components position.
            UEditorInternal.MoveComponentBottom<PickableItem>(itemObject.transform);
            UEditorInternal.MoveComponentBottom<Rigidbody>(itemObject.transform);
            UEditorInternal.MoveComponentBottom<SphereCollider>(itemObject.transform);
            UEditorInternal.MoveComponentBottom<BoxCollider>(itemObject.transform);
            UEditorInternal.MoveComponentBottom<AudioSource>(itemObject.transform);

            return itemObject;
        }
    }
}