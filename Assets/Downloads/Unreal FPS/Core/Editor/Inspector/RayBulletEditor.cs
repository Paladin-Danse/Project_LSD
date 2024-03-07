/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnrealFPS.Runtime;

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(RayBullet))]
    [CanEditMultipleObjects]
    public class RayBulletEditor : UEditor<RayBullet>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseSettings = new GUIContent("Base Settings");
            public readonly static GUIContent Model = new GUIContent("Model", "Bullet model.");
            public readonly static GUIContent Damage = new GUIContent("Damage", "Bullet hit damage.");
            public readonly static GUIContent Variance = new GUIContent("Variance", "Bullet variance (for shotgun bullet).");
            public readonly static GUIContent NumberBullet = new GUIContent("Number Bullet", "Number bullet (for shotgun bullet).");
            public readonly static GUIContent DecalProperties = new GUIContent("Decal Properties", "Decal properties asset.");
        }

        public override void BaseGUI()
        {
            BeginBox();
            GUILayout.Label(ContentProperties.BaseSettings, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            instance.SetModel(EditorGUILayout.TextField(ContentProperties.Model, instance.GetModel()));
            instance.SetDamage(EditorGUILayout.IntField(ContentProperties.Damage, instance.GetDamage()));
            instance.SetVariance(EditorGUILayout.FloatField(ContentProperties.Variance, instance.GetVariance()));
            instance.SetNumberBullet(EditorGUILayout.IntField(ContentProperties.NumberBullet, instance.GetNumberBullet()));
            instance.SetDecalProperties(ObjectField<DecalProperties>(ContentProperties.DecalProperties, instance.GetDecalProperties(), false));
            EndBox();
        }
    }
}