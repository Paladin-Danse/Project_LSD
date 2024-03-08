/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnrealFPS.Runtime;
using static UnrealFPS.Editor.UProjectManager;

namespace UnrealFPS.Editor
{
    [UPMItem("Player", 1, ItemType.General)]
    public sealed class PlayerUItem
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent PlayerType = new GUIContent("Type");
            public readonly static GUIContent InventoryType = new GUIContent("Inventory Type");
            public readonly static GUIContent Name = new GUIContent("Name");
            public readonly static GUIContent BodyModel = new GUIContent("Body Model");
            public readonly static GUIContent Controller = new GUIContent("Animator Controller");
        }

        internal sealed class PlayerProperties
        {
            public enum InventoryType
            {
                SimpleInventory,
                AdvancedInventory
            }

            private const string DEFAULT_NAME = "Write player name here...";

            private InventoryType inventoryType;
            private string name;
            private GameObject bodyModel;
            private AnimatorController controller;

            public PlayerProperties()
            {
                this.name = DEFAULT_NAME;
            }

            public PlayerProperties(string name)
            {
                this.name = name;
            }

            public InventoryType GetInventoryType()
            {
                return inventoryType;
            }

            public void SetInventoryType(InventoryType value)
            {
                inventoryType = value;
            }

            public string GetName()
            {
                return name;
            }

            public void SetName(string value)
            {
                name = value;
            }

            public GameObject GetBodyModel()
            {
                return bodyModel;
            }

            public void SetBodyModel(GameObject value)
            {
                bodyModel = value;
            }

            public AnimatorController GetController()
            {
                return controller;
            }

            public void SetController(AnimatorController value)
            {
                controller = value;
            }

            public bool NameIsValid()
            {
                return name != DEFAULT_NAME &&
                    !string.IsNullOrEmpty(name);
            }

            public void SetDefaultName()
            {
                name = DEFAULT_NAME;
            }
        }

        private static PlayerProperties properties;
        private static IEditorDelay delay;
        private static GameObject player;
        private static bool addInventory;
        private static bool addHealth;
        private static bool addBody;

        /// <summary>
        /// This function is called when the window becomes enabled and active.
        /// </summary>
        public static void OnEnable()
        {
            properties = new PlayerProperties();
            delay = new EditorDelay(0.1f);
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        public static void OnGUI()
        {
            DrawProperties();
        }

        private static void DrawProperties()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(3);
            GUILayout.Label("Base Options", UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(5);

            properties.SetName(EditorGUILayout.DelayedTextField(ContentProperties.Name, properties.GetName()));
            if (properties.GetName() == "")
            {
                properties.SetDefaultName();
            }

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            addHealth = EditorGUILayout.ToggleLeft("Add Health System", addHealth, GUILayout.Width(150));
            addInventory = EditorGUILayout.ToggleLeft("Add Inventory System", addInventory, GUILayout.Width(150));
            addBody = EditorGUILayout.ToggleLeft("Add Body", addBody, GUILayout.Width(150));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(!addInventory);
            properties.SetInventoryType((PlayerProperties.InventoryType)EditorGUILayout.EnumPopup(ContentProperties.InventoryType, properties.GetInventoryType()));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!addBody);
            properties.SetBodyModel((GameObject)EditorGUILayout.ObjectField(ContentProperties.BodyModel, properties.GetBodyModel(), typeof(GameObject), true));
            properties.SetController((AnimatorController)EditorGUILayout.ObjectField(ContentProperties.Controller, properties.GetController(), typeof(AnimatorController), true));
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(5);
            UEditor.HorizontalLine();
            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(!properties.NameIsValid() || (addBody && properties.GetBodyModel() != null));
            if (UEditor.Button("Create", "Right", GUILayout.Width(70)))
            {
                player = CreatePlayer(properties);
            }
            if (player != null && delay.WaitForSeconds())
            {
                if (UDisplayDialogs.Message("Create Successful", "Player was created on scene!\nSetup Player components before start play.", "Select", "Ok"))
                {
                    Selection.activeGameObject = player;
                }
                player = null;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(5);
            GUILayout.EndVertical();
        }

        private static GameObject CreatePlayer(PlayerProperties properties)
        {
            // Create player object.
            GameObject player = new GameObject();
            player.name = properties.GetName();
            player.tag = TNC.PLAYER;
            player.layer = LayerMask.NameToLayer(LNC.PLAYER);

            // Create first person camera.
            GameObject _FPCamera = new GameObject();
            _FPCamera.name = "FPCamera";
            _FPCamera.tag = TNC.CAMERA;

            // Create first person camera weapon layer.
            GameObject _FPCameraLayer = new GameObject();
            _FPCameraLayer.name = "WeaponLayer";
            _FPCameraLayer.layer = LayerMask.NameToLayer(TNC.CAMERA_LAYER);
            _FPCameraLayer.transform.SetParent(_FPCamera.transform);

            // Add player components
            CharacterController characterController = UEditorInternal.AddComponent<CharacterController>(player);
            FPController controller = UEditorInternal.AddComponent<FPController>(player);

            SimpleInventory simpleInventory = null;
            AdvancedInventory advancedInventory = null;
            if (addInventory)
            {
                switch (properties.GetInventoryType())
                {
                    case PlayerProperties.InventoryType.SimpleInventory:
                        simpleInventory = UEditorInternal.AddComponent<SimpleInventory>(player);
                        break;
                    case PlayerProperties.InventoryType.AdvancedInventory:
                        advancedInventory = UEditorInternal.AddComponent<AdvancedInventory>(player);
                        break;
                }
            }

            FPHealth health = null;
            if (addHealth)
            {
                health = UEditorInternal.AddComponent<FPHealth>(player);
            }

            AudioSource audioSource = UEditorInternal.AddComponent<AudioSource>(player);

            // Add camera components
            Camera cameraComponent = UEditorInternal.AddComponent<Camera>(_FPCamera);
            UEditorInternal.AddComponent<AudioListener>(_FPCamera);
            UEditorInternal.AddComponent<ShakeCamera>(_FPCamera);

            _FPCamera.transform.SetParent(player.transform);

            // Add camera layer componetns
            Camera weaponLayerCameraComponent = UEditorInternal.AddComponent<Camera>(_FPCameraLayer);

            // Set player components
            controller.SetFPCamera(cameraComponent);
            //inventory.SetCamera(_FPCamera.transform);

            // Set camera componets
            cameraComponent.cullingMask = ~((1 << LayerMask.NameToLayer(LNC.WEAPON)) | (1 << LayerMask.NameToLayer(LNC.REMOTE_BODY)));

            // Set camera layer components
            weaponLayerCameraComponent.cullingMask = 1 << LayerMask.NameToLayer(LNC.WEAPON);

            // Set player components position
            UEditorInternal.MoveComponentBottom<CharacterController>(player.transform);
            UEditorInternal.MoveComponentBottom<FPController>(player.transform);

            if (simpleInventory != null || advancedInventory != null)
            {
                switch (properties.GetInventoryType())
                {
                    case PlayerProperties.InventoryType.SimpleInventory:
                        UEditorInternal.MoveComponentBottom<SimpleInventory>(player.transform);
                        break;
                    case PlayerProperties.InventoryType.AdvancedInventory:
                        UEditorInternal.MoveComponentBottom<AdvancedInventory>(player.transform);
                        break;
                }
            }

            if (health != null)
            {
                UEditorInternal.MoveComponentBottom<FPHealth>(player.transform);

            }
            UEditorInternal.MoveComponentBottom<AudioSource>(player.transform);

            // Optional: Create first person player body.
            if (addBody)
            {
                GameObject _FPSBody = GameObject.Instantiate(properties.GetBodyModel(), Vector3.zero, Quaternion.identity, player.transform);
                Animator animator = UEditorInternal.AddComponent<Animator>(_FPSBody);
                animator.runtimeAnimatorController = properties.GetController();
                UEditorInternal.AddComponent<FPSBody>(_FPSBody);
            }

            return player;
        }
    }
}