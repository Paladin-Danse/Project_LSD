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
    [CustomEditor(typeof(Grenade))]
    [CanEditMultipleObjects]
    public class GrenadeEditor : UEditor<Grenade>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent GrenadeObject = new GUIContent("Greanade Object", "Object with grenade mesh.");
            public readonly static GUIContent Radius = new GUIContent("Radius", "Radius for detect gameobjects.");
            public readonly static GUIContent ExplosionEffect = new GUIContent("Effect", "Explosion effect.");
            public readonly static GUIContent ExplosionSound = new GUIContent("Sound", "Explosion sound.");
            public readonly static GUIContent ExplosionProperties = new GUIContent("Explosion Properties", "ExplosionProperties asset.");
            public readonly static GUIContent DecalProperties = new GUIContent("Decal Properties", "DecalProperties asset.");
            public readonly static GUIContent Delay = new GUIContent("Delay", "Grenade life time.");
        }

        public virtual void OnSceneGUI()
        {
            Handles.color = new Color32(50, 60, 70, 170);
            Handles.SphereHandleCap(0, instance.transform.position, instance.transform.rotation, instance.GetRadius() * 2, EventType.Repaint);
            Handles.color = new Color32(150, 150, 150, 255);
            instance.SetRadius(Handles.RadiusHandle(instance.transform.rotation, instance.transform.position, instance.GetRadius()));
        }

        public override void BaseGUI()
        {
            BeginBox();
            GUILayout.Label(ContentProperties.BaseProperties, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            instance.SetGrenadeObject((GameObject)EditorGUILayout.ObjectField(ContentProperties.GrenadeObject, instance.GetGrenadeObject(), typeof(GameObject), true));
            instance.SetRadius(EditorGUILayout.FloatField(ContentProperties.Radius, instance.GetRadius()));
            instance.SetExplosionEffect((ParticleSystem)EditorGUILayout.ObjectField(ContentProperties.ExplosionEffect, instance.GetExplosionEffect(), typeof(ParticleSystem), true));
            instance.SetExplosionSound((AudioClip)EditorGUILayout.ObjectField(ContentProperties.ExplosionSound, instance.GetExplosionSound(), typeof(AudioClip), true));
            instance.SetExplosionProperties((ExplosionProperties)EditorGUILayout.ObjectField(ContentProperties.ExplosionProperties, instance.GetExplosionProperties(), typeof(ExplosionProperties), true));
            instance.SetDecalProperties((DecalProperties)EditorGUILayout.ObjectField(ContentProperties.DecalProperties, instance.GetDecalProperties(), typeof(DecalProperties), true));
            instance.SetDelay(EditorGUILayout.FloatField(ContentProperties.Delay, instance.GetDelay()));
            EndBox();
        }
    }
}