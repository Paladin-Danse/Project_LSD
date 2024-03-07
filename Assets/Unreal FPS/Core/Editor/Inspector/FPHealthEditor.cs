/* ================================================================
   ---------------------------------------------------
   Project   :    #0001
   Publisher :    #0002
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnrealFPS.Runtime;
using UnrealFPS.Utility;

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(FPHealth))]
    [CanEditMultipleObjects]
    public class FPHealthEditor : UEditor<FPHealth>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent HealthProperties = new GUIContent("Health Properties");
            public readonly static GUIContent AdditionalsSystems = new GUIContent("Additional Systems");
            public readonly static GUIContent RegenerationProperties = new GUIContent("Regeneration Properties");
            public readonly static GUIContent FallDamageProperties = new GUIContent("Fall Damage Properties");
            public readonly static GUIContent DeathCameraProperties = new GUIContent("Death Camera Properties");
            public readonly static GUIContent Health = new GUIContent("Health", "Health point value.");
            public readonly static GUIContent MaxHealth = new GUIContent("Max Health", "Max health point value.");
            public readonly static GUIContent MinHealth = new GUIContent("Min Health", "Min health point value.");
            public readonly static GUIContent Rate = new GUIContent("Rate", "Rate (in seconds) of adding health points.\n(V/R - Value per rate).");
            public readonly static GUIContent Value = new GUIContent("Value", "Health point value.");
            public readonly static GUIContent Delay = new GUIContent("Delay", "Delay before start adding health.");
            public readonly static GUIContent ChromaticAberration = new GUIContent("Chromatic Aberration");
            public readonly static GUIContent Vignette = new GUIContent("Vignette");
            public readonly static GUIContent DamageCameraEffect = new GUIContent("Damage Camera Effect", "Damage camera effect system.");
            public readonly static GUIContent Profile = new GUIContent("Profile", "Post processing profile assets.");
            public readonly static GUIContent StartPoint = new GUIContent("Start Point", "From how many percent of health begin to show the effect.");
            public readonly static GUIContent ResetSmooth = new GUIContent("Reset Smooth", "Smooth value for reserting effect, when health more then start ppint value.");
            public readonly static GUIContent ChromaticAberrationSpeed = new GUIContent("Speed", "Chromatic aberration effect speed.");
            public readonly static GUIContent VignetteSmooth = new GUIContent("Smooth", "Vignette effect smooth value.");
        }

        private bool regenerationFoldout;
        private bool fallDamagePropertiesFoldout;
        private bool deathCameraFoldout;
        private bool damageCameraEffect;
        private float maxHeight = 20.0f;
        private bool editMaxHeight = false;
        private ReorderableList fdpList;
        private ReorderableList dccList;
        private ReorderableList dctList;

        public override string HeaderName()
        {
            return "First Person Health";
        }

        public override void InitializeProperties()
        {
            maxHeight = GetMaxHeight();

            SerializedProperty fallDamageProperties = serializedObject.FindProperty("fallDamageProperties");
            fdpList = new ReorderableList(serializedObject, fallDamageProperties, true, true, true, true)
            {
                drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(new Rect(rect.x + 57, rect.y + 1, 65, EditorGUIUtility.singleLineHeight), "Damage");
                        EditorGUI.LabelField(new Rect(rect.x + 120, rect.y + 1, 77, EditorGUIUtility.singleLineHeight), "Min Height");
                        float x = editMaxHeight ? 70 : 25;
                        if (GUI.Button(new Rect(rect.width - x, rect.y + 1, 67, EditorGUIUtility.singleLineHeight), "Max Height", EditorStyles.label))
                            editMaxHeight = !editMaxHeight;
                        if (editMaxHeight)
                            maxHeight = EditorGUI.FloatField(new Rect(rect.width - 12.5f, rect.y + 1, 55, EditorGUIUtility.singleLineHeight - 2), GUIContent.none, maxHeight);
                    },

                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        SerializedProperty property = fallDamageProperties.GetArrayElementAtIndex(index);

                        EditorGUI.LabelField(new Rect(rect.x - 15, rect.y + 1, 150, EditorGUIUtility.singleLineHeight), "Preset " + (index + 1));
                        EditorGUI.PropertyField(new Rect(rect.x + 50, rect.y + 1.5f, 50, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("damage"), GUIContent.none);
                        EditorGUI.LabelField(new Rect(rect.x + 92, rect.y + 1, 50, EditorGUIUtility.singleLineHeight), "->");
                        EditorGUI.PropertyField(new Rect(rect.x + 118, rect.y + 1.5f, 55, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("minHeight"), GUIContent.none);
                        float min = property.FindPropertyRelative("minHeight").floatValue;
                        float max = property.FindPropertyRelative("maxHeight").floatValue;
                        EditorGUI.MinMaxSlider(new Rect(rect.x + 160, rect.y + 1.5f, rect.width - 216, EditorGUIUtility.singleLineHeight), ref min, ref max, 0, maxHeight);
                        property.FindPropertyRelative("minHeight").floatValue = UMathf.AllocatePart(min);
                        property.FindPropertyRelative("maxHeight").floatValue = UMathf.AllocatePart(max);
                        EditorGUI.PropertyField(new Rect(rect.width - 12.5f, rect.y + 1.5f, 55, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("maxHeight"), GUIContent.none);
                    }
            };

            SerializedProperty transformsList = serializedObject.FindProperty("deathCamera").FindPropertyRelative("transformsToDisable");
            dctList = new ReorderableList(serializedObject, transformsList, true, true, true, true)
            {
                drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(new Rect(rect.x - 15, rect.y + 1, 205, EditorGUIUtility.singleLineHeight), "Transmorms To Disable");
                    },

                    drawElementCallback = (rect, index, isActive, isFocus) =>
                    {
                        SerializedProperty property = transformsList.GetArrayElementAtIndex(index);
                        EditorGUI.LabelField(new Rect(rect.x - 15, rect.y + 1.5f, 137, EditorGUIUtility.singleLineHeight), property.objectReferenceValue ? property.objectReferenceValue.name : "Empty transform: " + (index + 1));
                        EditorGUI.PropertyField(new Rect(rect.x + 80, rect.y + 1.5f, rect.width - 80, EditorGUIUtility.singleLineHeight), property, GUIContent.none);
                    }
            };

            SerializedProperty componentsList = serializedObject.FindProperty("deathCamera").FindPropertyRelative("componentsToDisable");
            dccList = new ReorderableList(serializedObject, componentsList, true, true, true, true)
            {
                drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(new Rect(rect.x - 15, rect.y + 1, 205, EditorGUIUtility.singleLineHeight), "Components To Disable");
                    },

                    drawElementCallback = (rect, index, isActive, isFocus) =>
                    {
                        SerializedProperty property = componentsList.GetArrayElementAtIndex(index);
                        EditorGUI.LabelField(new Rect(rect.x - 15, rect.y + 1.5f, 137, EditorGUIUtility.singleLineHeight), property.objectReferenceValue ? property.objectReferenceValue.GetType().Name : "Empty behaviour: " + (index + 1));
                        EditorGUI.PropertyField(new Rect(rect.x + 80, rect.y + 1.5f, rect.width - 80, EditorGUIUtility.singleLineHeight), property, GUIContent.none);
                    }
            };

        }

        public override void BaseGUI()
        {
            CheckHealthPointsValue();
            BeginBox();
            GUILayout.Label(ContentProperties.HealthProperties, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(5);
            instance.SetHealth(EditorGUILayout.IntSlider(ContentProperties.Health, instance.GetHealth(), 0, instance.GetMaxHealth()));
            instance.SetMaxHealth(EditorGUILayout.IntField(ContentProperties.MaxHealth, instance.GetMaxHealth()));
            instance.SetMinHealth(EditorGUILayout.IntField(ContentProperties.MinHealth, instance.GetMinHealth()));

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.AdditionalsSystems, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.BeginDisabledGroup(!instance.RegenirationIsActive());
            IncreaseIndentLevel();
            regenerationFoldout = EditorGUILayout.Foldout(regenerationFoldout, ContentProperties.RegenerationProperties, true);
            if (regenerationFoldout)
            {
                HealthRegenirationProperties regenerationSystem = instance.GetRegenerationSystem().GetRegenerationProperties();
                regenerationSystem.SetRate(EditorGUILayout.FloatField(ContentProperties.Rate, regenerationSystem.GetRate()));
                regenerationSystem.SetValue(EditorGUILayout.IntField(ContentProperties.Value, regenerationSystem.GetValue()));
                regenerationSystem.SetDelay(EditorGUILayout.FloatField(ContentProperties.Delay, regenerationSystem.GetDelay()));
                instance.GetRegenerationSystem().SetRegenerationProperties(regenerationSystem);
            }
            string rpToggleName = instance.RegenirationIsActive() ? "Regeniration Enabled" : "Regeneration Disabled";
            EditorGUI.EndDisabledGroup();
            if (regenerationFoldout && !instance.RegenirationIsActive())
            {
                Notification("Regeneration Disabled");
            }
            instance.RegenirationActive(EditorGUILayout.Toggle(rpToggleName, instance.RegenirationIsActive()));
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(3);
            fallDamagePropertiesFoldout = EditorGUILayout.Foldout(fallDamagePropertiesFoldout, ContentProperties.FallDamageProperties, true);
            if (fallDamagePropertiesFoldout)
            {
                fdpList.DoLayoutList();
            }
            GUILayout.Space(3);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(3);
            deathCameraFoldout = EditorGUILayout.Foldout(deathCameraFoldout, ContentProperties.DeathCameraProperties, true);
            if (deathCameraFoldout)
            {
                instance.GetDeathCamera().SetCamera((Transform)EditorGUILayout.ObjectField("Camera", instance.GetDeathCamera().GetCamera(), typeof(Transform), true));
                instance.GetDeathCamera().SetBody((Transform)EditorGUILayout.ObjectField("Body", instance.GetDeathCamera().GetBody(), typeof(Transform), true));
                instance.GetDeathCamera().SetLookAtTransform((Transform)EditorGUILayout.ObjectField("Look At", instance.GetDeathCamera().GetLookAtTransform(), typeof(Transform), true));
                instance.GetDeathCamera().SetRotationSpeed(EditorGUILayout.FloatField("Rotation Speed", instance.GetDeathCamera().GetRotationSpeed()));
                GUILayout.BeginHorizontal();
                GUILayout.Space(16);
                GUILayout.Label("Freeze Rotation", GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                FPHealth.DeathCamera.FreezeRotation freezeRotation = instance.GetDeathCamera().GetFreezeRotation();
                freezeRotation.x = EditorGUILayout.ToggleLeft("X", freezeRotation.x, GUILayout.Width(50));
                freezeRotation.y = EditorGUILayout.ToggleLeft("Y", freezeRotation.y, GUILayout.Width(50));
                freezeRotation.z = EditorGUILayout.ToggleLeft("Z", freezeRotation.z, GUILayout.Width(50));
                instance.GetDeathCamera().SetFreezeRotation(freezeRotation);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
                dctList.DoLayoutList();
                GUILayout.Space(5);
                dccList.DoLayoutList();
                GUILayout.Space(5);
            }
            GUILayout.Space(3);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(3);
            damageCameraEffect = EditorGUILayout.Foldout(damageCameraEffect, ContentProperties.DamageCameraEffect, true);
            if (damageCameraEffect)
            {
                instance.GetDamageCameraEffect().SetProfile((PostProcessingProfile)EditorGUILayout.ObjectField(ContentProperties.Profile, instance.GetDamageCameraEffect().GetProfile(), typeof(PostProcessingProfile), true));

                instance.GetDamageCameraEffect().SetStartPoint(EditorGUILayout.IntSlider(ContentProperties.StartPoint, instance.GetDamageCameraEffect().GetStartPoint(), instance.GetMinHealth(), instance.GetMaxHealth()));
                instance.GetDamageCameraEffect().SetResetSmooth(EditorGUILayout.FloatField(ContentProperties.ResetSmooth, instance.GetDamageCameraEffect().GetResetSmooth()));

                GUILayout.Space(10);
                GUILayout.Label(ContentProperties.ChromaticAberration, UEditorStyles.SectionHeaderLabel);
                GUILayout.Space(7);
                EditorGUI.BeginDisabledGroup(instance.GetDamageCameraEffect().GetProfile() == null);
                instance.GetDamageCameraEffect().SetChromaticAberrationSpeed(EditorGUILayout.FloatField(ContentProperties.ChromaticAberrationSpeed, instance.GetDamageCameraEffect().GetChromaticAberrationSpeed()));
                EditorGUI.EndDisabledGroup();

                GUILayout.Space(10);
                GUILayout.Label(ContentProperties.Vignette, UEditorStyles.SectionHeaderLabel);
                GUILayout.Space(7);
                EditorGUI.BeginDisabledGroup(instance.GetDamageCameraEffect().GetProfile() == null);
                instance.GetDamageCameraEffect().SetVignetteSmooth(EditorGUILayout.FloatField(ContentProperties.VignetteSmooth, instance.GetDamageCameraEffect().GetVignetteSmooth()));
                float min = instance.GetDamageCameraEffect().GetVignetteMinValue();
                float max = instance.GetDamageCameraEffect().GetVignetteMaxValue();
                MinMaxSlider("Range", ref min, ref max, 0, 1);
                instance.GetDamageCameraEffect().SetVignetteMinValue(UMathf.AllocatePart(min));
                instance.GetDamageCameraEffect().SetVignetteMaxValue(UMathf.AllocatePart(max));
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.Space(3);
            GUILayout.EndVertical();

            DecreaseIndentLevel();
            EndBox();
        }

        protected virtual void CheckHealthPointsValue()
        {
            if (instance.GetHealth() < instance.GetMinHealth())
            {
                instance.SetHealth(instance.GetMinHealth());
            }
        }

        protected virtual float GetMaxHeight()
        {
            if (instance.GetFallDamageProperties().Length == 0)
                return 20;
            float height = instance.GetFallDamageProperties().Max(h => h.GetMaxHeight());
            return Mathf.Ceil(height);
        }
    }
}