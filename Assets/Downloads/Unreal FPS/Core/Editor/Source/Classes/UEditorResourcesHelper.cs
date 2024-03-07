/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS.Editor {
    public static class UEditorResourcesHelper {
        public const string PROPETIES_PATH = "Editor/Properties/";
        public const string ICONS_PATH = "Editor/Icons/";

        public static UEditorProperties GetEditorProperties () {
            UEditorProperties editorProperties = null;
            if (editorProperties == null)
                editorProperties = Resources.Load (PROPETIES_PATH + "Editor Properties") as UEditorProperties;
            return editorProperties;
        }

        public static UEditorGUIProperties GetEditorGUIProperties () {
            UEditorGUIProperties editorGUIProperties = null;
            if (editorGUIProperties == null)
                editorGUIProperties = Resources.Load (PROPETIES_PATH + "Editor GUI Properties") as UEditorGUIProperties;
            return editorGUIProperties;
        }

        public static UMenuItemsProperties GetMenuItemsProperties () {
            UMenuItemsProperties menuitemsProperties = null;
            if (menuitemsProperties == null)
                menuitemsProperties = Resources.Load (PROPETIES_PATH + "Menu Items Properties") as UMenuItemsProperties;
            return menuitemsProperties;
        }

        public static Texture2D GetIcon (string iconName) {
            Texture2D icon = null;
            switch (iconName) {
                case "Logo Full":
                    icon = (Texture2D) Resources.Load (ICONS_PATH + "Logo(Full)") as Texture2D;
                    break;
                case "Logo Short":
                    icon = (Texture2D) Resources.Load (ICONS_PATH + "Logo(Short)") as Texture2D;
                    break;
                case "ID":
                    icon = (Texture2D) Resources.Load (ICONS_PATH + "ID") as Texture2D;
                    break;
                case "Window":
                    icon = (Texture2D) Resources.Load (ICONS_PATH + "UWindow") as Texture2D;
                    break;
                case "Search":
                    icon = (Texture2D) Resources.Load (ICONS_PATH + "Search") as Texture2D;
                    break;
                case "List":
                    icon = (Texture2D) Resources.Load (ICONS_PATH + "List") as Texture2D;
                    break;
                case "Generate":
                    icon = (Texture2D) Resources.Load (ICONS_PATH + "Generate") as Texture2D;
                    break;
            }
            return icon;
        }
    }
}