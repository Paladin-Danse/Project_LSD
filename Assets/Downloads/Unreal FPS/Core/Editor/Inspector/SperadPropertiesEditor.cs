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
using UnrealFPS.Utility;

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(SpreadProperties))]
    [CanEditMultipleObjects]
    public class SperadPropertiesEditor : UEditor<SpreadProperties>
    {
        public new static class ContentProperties
        {
            public readonly static GUIContent BulletSpreadProperties = new GUIContent("Bullet Spread Properties");
            public readonly static GUIContent CameraShakeProperties = new GUIContent("Camera Shake Properties");
        }

        private float maxSpreadValue = 1;
        private float minSpreadValue = -1;
        private bool editSpreadValues = false;

        private SerializedProperty properties;
        private List<bool> propertiesFoldout;

        public override void InitializeProperties()
        {
            maxSpreadValue = GetMaxSpreadValue();
            minSpreadValue = GetMinSpreadValue();

            properties = serializedObject.FindProperty("properties");
            propertiesFoldout = CreatePropertiesFoldouts();
        }

        public override void BaseGUI()
        {
            for (int i = 0, length = properties.arraySize; i < length; i++)
            {
                BeginBox();
                IncreaseIndentLevel();
                SpreadProperty property = instance.GetProperty(i);

                Rect removeButtonRect = GUILayoutUtility.GetRect(0, 0);
                removeButtonRect.x = removeButtonRect.width + 5;
                removeButtonRect.y += 1;
                removeButtonRect.width = 16.5f;
                removeButtonRect.height = 16.5f;
                if (GUI.Button(removeButtonRect, GUIContent.none, GUI.skin.GetStyle("OL Minus")))
                {
                    properties.DeleteArrayElementAtIndex(i);
                    break;
                }

                propertiesFoldout[i] = EditorGUILayout.Foldout(propertiesFoldout[i], "Property " + (i + 1), true);
                if (propertiesFoldout[i])
                {
                    GUILayout.Space(3);
                    property.SetActionState((WeaponActionState) EditorGUILayout.EnumPopup("Active State", property.GetActionState()));

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    if (GUILayout.Button("Spread", EditorStyles.label))
                    {
                        editSpreadValues = !editSpreadValues;
                    }
                    GUILayout.Space(EditorGUIUtility.labelWidth - 78.5f);
                    float min = property.GetMinBulletSpread();
                    float max = property.GetMaxBulletSpread();
                    min = EditorGUILayout.FloatField(min, GUILayout.Width(55));
                    EditorGUILayout.MinMaxSlider(ref min, ref max, minSpreadValue, maxSpreadValue);
                    max = EditorGUILayout.FloatField(max, GUILayout.Width(55));
                    property.SetMinBulletSpread(UMathf.AllocatePart(min));
                    property.SetMaxBulletSpread(UMathf.AllocatePart(max));
                    GUILayout.EndHorizontal();

                    if (editSpreadValues)
                    {
                        minSpreadValue = EditorGUILayout.FloatField("Min Value", minSpreadValue);
                        maxSpreadValue = EditorGUILayout.FloatField("Max Value", maxSpreadValue);
                        if (UEditor.MiniButton("Apply"))
                        {
                            editSpreadValues = false;
                        }
                        GUILayout.Space(3);
                    }

                    ShakeCamera.ShakeProperties shakeProperties = property.GetShakeProperties();
                    shakeProperties.SetTarget((ShakeCamera.ShakeEvent.Target) EditorGUILayout.EnumPopup("Target", shakeProperties.GetTarget()));
                    shakeProperties.SetAmplitude(EditorGUILayout.FloatField("Amplitude", shakeProperties.GetAmplitude()));
                    shakeProperties.SetFrequency(EditorGUILayout.FloatField("Frequency", shakeProperties.GetFrequency()));
                    shakeProperties.SetDuration(EditorGUILayout.FloatField("Duration", shakeProperties.GetDuration()));
                    shakeProperties.SetBlendOverLifetime(EditorGUILayout.CurveField("Blend Over Life time", shakeProperties.GetBlendOverLifetime()));
                    property.SetShakeProperties(shakeProperties);
                    GUILayout.Space(3);
                }
                DecreaseIndentLevel();
                EndBox();
                instance.SetProperty(i, property);
            }

            if (instance.GetLength() == 0)
            {
                UEditorHelpBoxMessages.Tip("Properties is empty!", "Add new properties.");
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Property", "ButtonLeft", GUILayout.Width(120)))
            {
                properties.arraySize++;
                propertiesFoldout.Add(false);
            }

            if (GUILayout.Button("Clear All Properties", "ButtonRight", GUILayout.Width(120)))
            {
                if (UDisplayDialogs.Confirmation(string.Format("Are you sure want clear camera shake properites:[{0}]", properties.arraySize)))
                {
                    properties.arraySize = 0;
                    propertiesFoldout.Clear();
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }

        protected virtual float GetMaxSpreadValue()
        {
            if (instance.GetLength() == 0)
                return 1;
            float value = instance.GetProperties().Max(h => h.GetMaxBulletSpread());
            if (value == 0)
                return 1;
            return Mathf.Ceil(value);
        }

        protected virtual float GetMinSpreadValue()
        {
            if (instance.GetLength() == 0)
                return -1;
            float value = instance.GetProperties().Min(h => h.GetMinBulletSpread());
            if (value == 0)
                return -1;
            return Mathf.Floor(value);
        }

        private List<bool> CreatePropertiesFoldouts()
        {
            List<bool> foldouts = new List<bool>();
            for (int i = 0, length = instance.GetLength(); i < length; i++)
            {
                foldouts.Add(false);
            }
            return foldouts;
        }
    }
}