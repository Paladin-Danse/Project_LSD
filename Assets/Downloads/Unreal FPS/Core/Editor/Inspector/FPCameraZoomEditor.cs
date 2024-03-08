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

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(FPCameraZoom))]
    [CanEditMultipleObjects]
    public class FPCameraZoomEditor : UEditor<FPCameraZoom>
    {
        public new static class ContentProperties
        {
            public readonly static GUIContent FPCamera = new GUIContent("FPCamera", "First person camera.");
            public readonly static GUIContent FPWeaponCamera = new GUIContent("FPWeapon Camera", "First person weapon camera.");
            public readonly static GUIContent DefaultFOV = new GUIContent("Default FOV", "Default camera field of view value.");
            public readonly static GUIContent ZoomFOV = new GUIContent("Zoom FOV", "Zoom camera field of view value.");
            public readonly static GUIContent Speed = new GUIContent("Speed", "Zoom speed.");
        }

        public override string HeaderName()
        {
            return "FPCamera Zoom";
        }

        public override void BaseGUI()
        {
            BeginBox();
            if (instance.GetFPCamera() != null)
            {
                instance.SetFPCamera(ObjectField<Camera>(ContentProperties.FPCamera, instance.GetFPCamera(), true));
            }
            else
            {
                GUILayout.BeginHorizontal();
                instance.SetFPCamera(ObjectField<Camera>(ContentProperties.FPCamera, instance.GetFPCamera(), true));
                if (SearchButton())
                {
                    Camera camera = UEditorInternal.FindFPCamera(instance.transform);
                    if (camera != null)
                    {
                        instance.SetFPCamera(camera);
                    }
                    else
                    {
                        UDisplayDialogs.Message("Searching", "Camera not found, try find it manually.");
                    }
                }
                GUILayout.EndHorizontal();
                UEditorHelpBoxMessages.CameraError();
            }
            if (instance.GetFPWeaponLayer() != null)
            {
                instance.SetFPWeaponLayer(ObjectField<Camera>(ContentProperties.FPWeaponCamera, instance.GetFPWeaponLayer(), true));
            }
            else
            {
                GUILayout.BeginHorizontal();
                instance.SetFPWeaponLayer(ObjectField<Camera>(ContentProperties.FPWeaponCamera, instance.GetFPWeaponLayer(), true));
                if (SearchButton())
                {
                    Camera camera = UEditorInternal.FindFPWeaponLayer(instance.transform);
                    if (camera != null)
                    {
                        instance.SetFPWeaponLayer(camera);
                    }
                    else
                    {
                        UDisplayDialogs.Message("Searching", "Weapon camera not found, try find it manually.");
                    }
                }
                GUILayout.EndHorizontal();
                UEditorHelpBoxMessages.CameraError();
            }
            instance.SetDefaultFOVValue(EditorGUILayout.Slider(ContentProperties.DefaultFOV, instance.GetDefaultFOVValue(), 0.0f, 179.0f));
            instance.SetZoomFOVValue(EditorGUILayout.Slider(ContentProperties.ZoomFOV, instance.GetZoomFOVValue(), 0.0f, 179.0f));
            instance.SetZoomTime(EditorGUILayout.FloatField(ContentProperties.Speed, instance.GetZoomTime()));
            EndBox();
        }
    }
}