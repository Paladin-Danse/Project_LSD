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
	[CustomEditor(typeof(FootstepProperties))]
	[CanEditMultipleObjects]
	public class FootstepPropertiesEditor : UEditor<FootstepProperties>
	{
		private SerializedProperty properties;
		private List<bool> propertyFoldouts;
		private bool stepFoldout;
		private bool jumpFoldout;
		private bool landFoldout;

		public override void InitializeProperties()
		{
			properties = serializedObject.FindProperty("properties");
			propertyFoldouts = new List<bool>();
			FillPropertyFoldouts();
		}

		public override string HeaderName()
		{
			return "Footstep Properties";
		}

		public override void BaseGUI()
		{
			if (instance == null)
			{
				return;
			}

			for (int i = 0; i < instance.GetLength(); i++)
			{
				BeginBox();
				IncreaseIndentLevel();
				FootstepProperty property = instance.GetProperty(i);
				SerializedProperty serializedProperty = properties.GetArrayElementAtIndex(i);
				string propertyName = "New Property " + (i + 1);
				if (property.GetPhysicMaterial() != null)
				{
					propertyName = property.GetPhysicMaterial().name;
				}
				else if (property.GetTexture() != null)
				{
					propertyName = property.GetTexture().name;
				}

				Rect removeButtonRect = GUILayoutUtility.GetRect(0, 0);
				removeButtonRect.x = removeButtonRect.width + 5;
				removeButtonRect.y += 1;
				removeButtonRect.width = 16.5f;
				removeButtonRect.height = 16.5f;
				if (GUI.Button(removeButtonRect, GUIContent.none, GUI.skin.GetStyle("OL Minus")))
				{
					properties.DeleteArrayElementAtIndex(i);
					propertyFoldouts.RemoveAt(i);
					break;
				}
				propertyFoldouts[i] = EditorGUILayout.Foldout(propertyFoldouts[i], propertyName, true);
				if (propertyFoldouts[i])
				{
					GUILayout.Space(3);
					GUILayout.Label("Surface", UEditorStyles.SectionHeaderLabel);
					GUILayout.Space(5);
					property.SetPhysicMaterial((PhysicMaterial) EditorGUILayout.ObjectField("Physic Material", property.GetPhysicMaterial(), typeof(PhysicMaterial), true));
					property.SetTexture((Texture2D) EditorGUILayout.ObjectField("Surface Texture", property.GetTexture(), typeof(Texture2D), true));
					if (property.GetPhysicMaterial() == null && property.GetTexture() == null)
					{
						UEditorHelpBoxMessages.Tip("Add Physic Material or Texture2D for handle surface.", "For create Physic Material press right mouse button Create > Physic Material.", true);
					}

					GUILayout.Space(10);
					GUILayout.Label("Sounds", UEditorStyles.SectionHeaderLabel);
					GUILayout.Space(5);
					BeginSubBox();
					stepFoldout = EditorGUILayout.Foldout(stepFoldout, "Step Sounds", true);
					if (stepFoldout)
					{
						if (property.GetStepSoundsLength() == 0)
						{
							UEditorHelpBoxMessages.Tip("Step sounds is empty!", "Add new step sound by click on [Add] button.", true);
						}
						for (int s = 0; s < property.GetStepSoundsLength(); s++)
						{
							GUILayout.BeginHorizontal();
							GUILayout.Space(3);
							GUILayout.Label("Clip " + (s + 1), GUILayout.Width(35));
							property.SetStepSound(s, (AudioClip) EditorGUILayout.ObjectField(property.GetStepSound(s), typeof(AudioClip), true));
							if (GUILayout.Button("", GUI.skin.GetStyle("OL Minus"), GUILayout.Width(20)))
							{
								serializedProperty.FindPropertyRelative("stepSounds").DeleteArrayElementAtIndex(s);
							}
							GUILayout.Space(3);
							GUILayout.EndHorizontal();
						}
						GUILayout.Space(3);
						GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						if (GUILayout.Button(" Add "))
						{
							serializedProperty.FindPropertyRelative("stepSounds").arraySize++;
						}
						GUILayout.EndHorizontal();
					}
					EndSubBox();

					BeginSubBox();
					jumpFoldout = EditorGUILayout.Foldout(jumpFoldout, "Jump Sounds", true);
					if (jumpFoldout)
					{
						if (property.GetJumpSoundsLength() == 0)
						{
							UEditorHelpBoxMessages.Tip("Jump sounds is empty!", "Add new jump sound by click on [Add] button.", true);
						}
						for (int j = 0; j < property.GetJumpSoundsLength(); j++)
						{
							GUILayout.BeginHorizontal();
							GUILayout.Space(3);
							GUILayout.Label("Clip " + (j + 1), GUILayout.Width(35));
							property.SetJumpSound(j, (AudioClip) EditorGUILayout.ObjectField(property.GetJumpSound(j), typeof(AudioClip), true));
							if (GUILayout.Button("", GUI.skin.GetStyle("OL Minus"), GUILayout.Width(20)))
							{
								serializedProperty.FindPropertyRelative("jumpSounds").DeleteArrayElementAtIndex(j);
							}
							GUILayout.Space(3);
							GUILayout.EndHorizontal();
						}
						GUILayout.Space(3);
						GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						if (GUILayout.Button(" Add "))
						{
							serializedProperty.FindPropertyRelative("jumpSounds").arraySize++;
						}
						GUILayout.EndHorizontal();
					}
					EndSubBox();

					BeginSubBox();
					landFoldout = EditorGUILayout.Foldout(landFoldout, "Land Sounds", true);
					if (landFoldout)
					{
						if (property.GetLandSoundsLength() == 0)
						{
							UEditorHelpBoxMessages.Tip("Land sounds is empty!", "Add new land sound by click on [Add] button.", true);
						}
						for (int l = 0; l < property.GetLandSoundsLength(); l++)
						{
							GUILayout.BeginHorizontal();
							GUILayout.Space(3);
							GUILayout.Label("Clip " + (l + 1), GUILayout.Width(35));
							property.SetLandSound(l, (AudioClip) EditorGUILayout.ObjectField(property.GetLandSound(l), typeof(AudioClip), true));
							if (GUILayout.Button("", GUI.skin.GetStyle("OL Minus"), GUILayout.Width(20)))
							{
								serializedProperty.FindPropertyRelative("landSounds").DeleteArrayElementAtIndex(l);
							}
							GUILayout.Space(3);
							GUILayout.EndHorizontal();
						}
						GUILayout.Space(3);
						GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						if (GUILayout.Button(" Add "))
						{
							serializedProperty.FindPropertyRelative("landSounds").arraySize++;
						}
						GUILayout.EndHorizontal();
					}
					EndSubBox();
					GUILayout.Space(3);
					instance.SetProperty(i, property);
				}
				DecreaseIndentLevel();
				EndBox();
			}

			if (instance.GetLength() == 0)
			{
				UEditorHelpBoxMessages.Tip("Properties is empty!", "Add new properties.");
			}

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(" Add Property ", "ButtonLeft", GUILayout.Width(120)))
			{
				properties.arraySize++;
				propertyFoldouts.Add(false);
			}

			if (GUILayout.Button(" Clear All Properties ", "ButtonRight", GUILayout.Width(120)))
			{
				if (UDisplayDialogs.Confirmation("Are you really want to remove all properties from this Footstep Properties asset?"))
				{
					properties.ClearArray();
				}
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		protected virtual void FillPropertyFoldouts()
		{
			if (propertyFoldouts == null)
				propertyFoldouts = new List<bool>();

			int propertyFoldoutLenght = instance.GetLength();

			for (int i = 0; i < propertyFoldoutLenght; i++)
			{
				propertyFoldouts.Add(false);
			}
		}
	}
}