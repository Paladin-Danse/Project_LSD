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

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(ReSpawnManager))]
    [CanEditMultipleObjects]
    public class ReSpawnManagerEditor : UEditor<ReSpawnManager>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent SpawnProperties = new GUIContent("Spawn Properties");
            public readonly static GUIContent ShapeProperties = new GUIContent("Shape Properties");
            public readonly static GUIContent UIProperties = new GUIContent("UI Properties");

            public readonly static GUIContent SpawnType = new GUIContent("Spawn Type");
            public readonly static GUIContent Player = new GUIContent("Player");
            public readonly static GUIContent ReSpawnHealth = new GUIContent("ReSpawn Health", "Health point count that be set player after respawn.");
            public readonly static GUIContent ReSpawnRotation = new GUIContent("ReSpawn Rotation", "Rotation of the player to be set after respawn.");
            public readonly static GUIContent RandomRotation = new GUIContent("Random Rotation", "Use if want to set random roation for the player every respawn.");
            public readonly static GUIContent ReSpawnDelay = new GUIContent("Delay", "Delay before respawn.");
            public readonly static GUIContent SpawnKey = new GUIContent("Key", "Key to confirm respawn.");

            public readonly static GUIContent SoundEffect = new GUIContent("Sound Effect", "Sound that be player when respawn.");

            public readonly static GUIContent ShapeType = new GUIContent("Shape Type");
            public readonly static GUIContent Radius = new GUIContent("Radius", "Radius of the circle.");
            public readonly static GUIContent Length = new GUIContent("Length", "Length of the rectangle.");
            public readonly static GUIContent Weight = new GUIContent("Weight", "Weight of the rectangle.");

            public readonly static GUIContent Panel = new GUIContent("Panel", "Parent object of the message.");
            public readonly static GUIContent Text = new GUIContent("Text", "Text UI object");
            public readonly static GUIContent Message = new GUIContent("Message", "Message that be displayed before respawn.");

        }

        private IHealth health;
        private bool spawnShapeFoldout;

        public override string HeaderName()
        {
            return "ReSpawn Manager";
        }

        public virtual void OnSceneGUI()
        {
            ReSpawnManager.ShapeProperties shapeProperties = instance.GetShapeProperties();
            Vector3 pos = instance.transform.position;
            switch (shapeProperties.shape)
            {
                case ReSpawnManager.ShapeProperties.Shape.Rectangle:
                    Vector3[] verts = new Vector3[]
                    {
                        new Vector3(pos.x - shapeProperties.length, pos.y, pos.z - shapeProperties.weight),
                        new Vector3(pos.x - shapeProperties.length, pos.y, pos.z + shapeProperties.weight),
                        new Vector3(pos.x + shapeProperties.length, pos.y, pos.z + shapeProperties.weight),
                        new Vector3(pos.x + shapeProperties.length, pos.y, pos.z - shapeProperties.weight)
                    };
                    Handles.DrawSolidRectangleWithOutline(verts, new Color32(35, 35, 35, 150), Color.white);

                    shapeProperties.length = Handles.ScaleValueHandle(shapeProperties.length,
                        new Vector3(pos.x + shapeProperties.length, pos.y, pos.z),
                        Quaternion.identity,
                        0.5f,
                        Handles.CubeHandleCap,
                        1.0f);

                    shapeProperties.length = Handles.ScaleValueHandle(shapeProperties.length,
                        new Vector3(pos.x - shapeProperties.length, pos.y, pos.z),
                        Quaternion.identity,
                        0.5f,
                        Handles.CubeHandleCap,
                        1.0f);

                    shapeProperties.weight = Handles.ScaleValueHandle(shapeProperties.weight,
                        new Vector3(pos.x, pos.y, pos.z + shapeProperties.weight),
                        Quaternion.identity,
                        0.5f,
                        Handles.CubeHandleCap,
                        1.0f);

                    shapeProperties.weight = Handles.ScaleValueHandle(shapeProperties.weight,
                        new Vector3(pos.x, pos.y, pos.z - shapeProperties.weight),
                        Quaternion.identity,
                        0.5f,
                        Handles.CubeHandleCap,
                        1.0f);

                    break;
                case ReSpawnManager.ShapeProperties.Shape.Circle:
                    Handles.color = new Color32(35, 35, 35, 150);
                    Handles.DrawSolidDisc(pos, new Vector3(0, 1, 0), shapeProperties.radius);
                    Handles.color = Color.white;
                    Handles.DrawWireDisc(pos, new Vector3(0, 1, 0), shapeProperties.radius);
                    Vector3[] radiusHandles = new Vector3[]
                    {
                        new Vector3(pos.x + shapeProperties.radius, pos.y, pos.z),
                        new Vector3(pos.x - shapeProperties.radius, pos.y, pos.z),
                        new Vector3(pos.x, pos.y, pos.z + shapeProperties.radius),
                        new Vector3(pos.x, pos.y, pos.z - shapeProperties.radius)
                    };
                    foreach (Vector3 posCube in radiusHandles)
                    {
                        shapeProperties.radius = Handles.ScaleValueHandle(shapeProperties.radius,
                            posCube,
                            Quaternion.identity,
                            0.5f,
                            Handles.CubeHandleCap,
                            1.0f);
                    }
                    break;
            }

            instance.SetShapeProperties(shapeProperties);
        }

        public override void BaseGUI()
        {
            BeginBox();
            GUILayout.Label(ContentProperties.ShapeProperties, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            ReSpawnManager.ShapeProperties shapeProperties = instance.GetShapeProperties();
            shapeProperties.shape = (ReSpawnManager.ShapeProperties.Shape) EditorGUILayout.EnumPopup(ContentProperties.ShapeType, shapeProperties.shape);
            switch (shapeProperties.shape)
            {
                case ReSpawnManager.ShapeProperties.Shape.Circle:
                    shapeProperties.radius = EditorGUILayout.FloatField(ContentProperties.Radius, shapeProperties.radius);
                    break;
                case ReSpawnManager.ShapeProperties.Shape.Rectangle:
                    shapeProperties.length = EditorGUILayout.FloatField(ContentProperties.Length, shapeProperties.length);
                    shapeProperties.weight = EditorGUILayout.FloatField(ContentProperties.Weight, shapeProperties.weight);
                    break;
            }
            instance.SetShapeProperties(shapeProperties);

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.SpawnProperties, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            instance.SetSpawnType((ReSpawnManager.SpawnType) EditorGUILayout.EnumPopup(ContentProperties.SpawnType, instance.GetSpawnType()));
            if (instance.GetSpawnType() == ReSpawnManager.SpawnType.ByKey)
            {
                instance.SetSpawnKey((KeyCode) EditorGUILayout.EnumPopup(ContentProperties.SpawnKey, instance.GetSpawnKey()));
            }

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
                health = instance.GetPlayer().GetComponent<IHealth>();
                if (health == null)
                {
                    UEditorHelpBoxMessages.Error("Player not have Health component!", "Add Health component on the player object or change player object.");
                }
            }

            EditorGUI.BeginDisabledGroup(health == null);
            instance.SetRespawnHealth(EditorGUILayout.IntSlider(ContentProperties.ReSpawnHealth, instance.GetRespawnHealth(), 0, health != null ? health.GetMaxHealth() : 100));
            EditorGUI.EndDisabledGroup();

            instance.RandomRotation(EditorGUILayout.Toggle(ContentProperties.RandomRotation, instance.RandomRotation()));

            if (!instance.RandomRotation())
            {
                instance.SetReSpawnRotation(EditorGUILayout.Vector3Field(ContentProperties.ReSpawnRotation, instance.GetReSpawnRotation()));
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField(ContentProperties.ReSpawnRotation, "Processed automatically.");
                EditorGUI.EndDisabledGroup();
            }

            instance.SetRespawnDelay(EditorGUILayout.FloatField(ContentProperties.ReSpawnDelay, instance.GetRespawnDelay()));
            instance.SetSoundEffect(ObjectField<AudioClip>(ContentProperties.SoundEffect, instance.GetSoundEffect(), true));

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.UIProperties, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            ReSpawnManager.SpawnUI spawnUI = instance.GetSpawnUI();
            spawnUI.panel = ObjectField<RectTransform>(ContentProperties.Panel, spawnUI.panel, true);
            spawnUI.text = ObjectField<Text>(ContentProperties.Text, spawnUI.text, true);
            spawnUI.message = EditorGUILayout.TextField(ContentProperties.Message, spawnUI.message);
            instance.SetSpawnUI(spawnUI);
            EndBox();
        }

    }
}