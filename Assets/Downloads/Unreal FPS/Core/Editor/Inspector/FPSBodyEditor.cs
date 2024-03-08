/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Linq;
using UnityEditor;
using UnityEngine;
using UnrealFPS.Runtime;

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(FPSBody))]
    [CanEditMultipleObjects]
    public class FPSBodyEditor : UEditor<FPSBody>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseSettings = new GUIContent("Base Settings");
            public readonly static GUIContent AnimatorParameters = new GUIContent("Animator Parameters");
            public readonly static GUIContent CrouchProperties = new GUIContent("Crouch Properties");
            public readonly static GUIContent Player = new GUIContent("Player", "Player transform.");
            public readonly static GUIContent StayBodyHeight = new GUIContent("Stay Height", "Stay body height.");
            public readonly static GUIContent CrouchBodyHeight = new GUIContent("Crouch Height", "Crouch body height.");
            public readonly static GUIContent Smooth = new GUIContent("Smooth", "Change height smooth.");
        }

        private string[] parameterNames;

        public override void InitializeProperties()
        {
            parameterNames = UEditorInternal.GetAnimatorParameterNames(instance.GetComponent<Animator>());
        }

        public override string HeaderName()
        {
            return "FPS Body";
        }

        public override void BaseGUI()
        {
            BeginBox();
            GUILayout.Label(ContentProperties.BaseSettings, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            instance.SetPlayer((FPController) EditorGUILayout.ObjectField(ContentProperties.Player, instance.GetPlayer(), typeof(FPController), true));

            GUILayout.Space(10);
            GUILayout.Label(ContentProperties.CrouchProperties, UEditorStyles.SectionHeaderLabel);
            GUILayout.Space(7);
            instance.SetStayBodyHeight(EditorGUILayout.FloatField(ContentProperties.StayBodyHeight, instance.GetStayBodyHeight()));
            instance.SetCrouchBodyHeight(EditorGUILayout.FloatField(ContentProperties.CrouchBodyHeight, instance.GetCrouchBodyHeight()));
            instance.SetMoveHeightSmooth(EditorGUILayout.FloatField(ContentProperties.Smooth, instance.GetMoveHeightSmooth()));
            EndBox();
        }
    }
}