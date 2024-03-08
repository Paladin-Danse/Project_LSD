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
    [CustomEditor(typeof(ExplosionProperties))]
    [CanEditMultipleObjects]
    public class ExplosionPropertiesEditor : UEditor<ExplosionProperties>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent ExplosionProperties = new GUIContent("Explosions Properties");
            public readonly static GUIContent Damage = new GUIContent("Damage", "Explosion damage.");
            public readonly static GUIContent Amplitude = new GUIContent("Amplitude", "Camera shake amplitude.");
            public readonly static GUIContent Impulse = new GUIContent("Impulse", "Explosion physics impulse.");
            public readonly static GUIContent ExplosionRadius = new GUIContent("Explosion Radius", "Explosion physics radius.");
            public readonly static GUIContent UpwardsModifier = new GUIContent("Upwards Modifier", "Physics upwards modifier.");
            public readonly static GUIContent MinMaxDistance = new GUIContent("Distance", "Min/Max distance.");
        }

        private SerializedProperty properties;
        private List<bool> propertiesFoldout;
        private float maxDistance;
        private bool editMaxDistance;

        public override void InitializeProperties()
        {
            properties = serializedObject.FindProperty("properties");
            propertiesFoldout = CreateSubExplosionPropertiesFoldouts();
            maxDistance = GetMaxDistance();
        }

        public override void BaseGUI()
        {
            for (int i = 0, length = properties.arraySize; i < length; i++)
            {
                bool foldout = propertiesFoldout[i];
                ExplosionProperty property = instance.GetProperty(i);
                BeginBox();
                IncreaseIndentLevel();

                Rect removeButtonRect = GUILayoutUtility.GetRect(0, 0);
                removeButtonRect.x = removeButtonRect.width + 5;
                removeButtonRect.y += 1;
                removeButtonRect.width = 16.5f;
                removeButtonRect.height = 16.5f;
                if (GUI.Button(removeButtonRect, GUIContent.none, GUI.skin.GetStyle("OL Minus")))
                {
                    properties.DeleteArrayElementAtIndex(i);
                    propertiesFoldout.RemoveAt(i);
                    break;
                }

                foldout = EditorGUILayout.Foldout(foldout, "Property " + (i + 1), true);
                if (foldout)
                {
                    GUILayout.Space(3);
                    property.SetDamage(EditorGUILayout.IntField(ContentProperties.Damage, property.GetDamage()));
                    property.SetAmplitude(EditorGUILayout.FloatField(ContentProperties.Amplitude, property.GetAmplitude()));
                    property.SetImpulse(EditorGUILayout.FloatField(ContentProperties.Impulse, property.GetImpulse()));
                    property.SetUpwardsModifier(EditorGUILayout.FloatField(ContentProperties.UpwardsModifier, property.GetUpwardsModifier()));

                    float min = property.GetMinDistance();
                    float max = property.GetMaxDistance() > maxDistance ? maxDistance : property.GetMaxDistance();
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    if (GUILayout.Button("Distance", EditorStyles.label))
                    {
                        editMaxDistance = !editMaxDistance;
                    }
                    GUILayout.Space(EditorGUIUtility.labelWidth - 87.5f);
                    min = EditorGUILayout.FloatField(min, GUILayout.Width(55));
                    EditorGUILayout.MinMaxSlider(ref min, ref max, 0.0f, maxDistance);
                    max = EditorGUILayout.FloatField(max, GUILayout.Width(55));
                    property.SetMinDistance(UMathf.AllocatePart(min));
                    property.SetMaxDistance(UMathf.AllocatePart(max));
                    GUILayout.EndHorizontal();

                    if (editMaxDistance)
                    {
                        maxDistance = EditorGUILayout.FloatField("Max Distance", maxDistance);
                        if (UEditor.MiniButton(" Apply "))
                        {
                            editMaxDistance = false;
                        }
                        GUILayout.Space(3);
                    }
                    GUILayout.Space(3);
                }
                DecreaseIndentLevel();
                EndBox();
                propertiesFoldout[i] = foldout;
                instance.SetProperty(i, property);
            }

            if (instance.GetLength() == 0)
            {
                UEditorHelpBoxMessages.Tip("Properties is empty!", "Add new properties.", false);
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(" Add Property ", "ButtonLeft", GUILayout.Width(120)))
            {
                properties.arraySize++;
                propertiesFoldout.Add(false);
            }

            if (GUILayout.Button(" Clear All Properties ", "ButtonRight", GUILayout.Width(120)))
            {
                if (UDisplayDialogs.Confirmation(string.Format("Are you sure want to remove explosion properties: [{0}]", properties.arraySize)))
                {
                    properties.arraySize = 0;
                    propertiesFoldout.Clear();
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }

        private List<bool> CreateSubExplosionPropertiesFoldouts()
        {
            List<bool> foldouts = new List<bool>();
            for (int i = 0, length = properties.arraySize; i < length; i++)
            {
                foldouts.Add(false);
            }
            return foldouts;
        }

        private float GetMaxDistance()
        {
            if (instance.GetProperties() == null || instance.GetLength() == 0)
            {
                return 20;
            }
            float max = instance.GetProperties().Max(m => m.GetMaxDistance());
            if(max == 0)
            {
                return 20;
            }
            return Mathf.Ceil(max);
        }
    }
}