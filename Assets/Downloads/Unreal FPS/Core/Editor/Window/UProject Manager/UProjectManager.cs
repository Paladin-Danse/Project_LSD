/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.TreeViewExamples;
using UnityEngine;

namespace UnrealFPS.Editor
{
    public partial class UProjectManager : EditorWindow
    {
        private const string WINDOW_NAME = "UProject Manager";

        private static Vector2 WindowMaxSize = new Vector2(800, 600);
        private static Vector2 WindowMinSize = new Vector2(300, 300);
        private static Texture2D WindowIcon;

        private TreeViewState treeViewState;
        private UPMTreeView treeView;
        private SearchField searchField;
        private ItemProperty[] itemProperties;

        private Vector2 scrollPosItems;
        private Vector2 scrollPosEditor;

        /// <summary>
        /// Open UManager window.
        /// </summary>
        [MenuItem(UEditorPaths.DEFAULT + WINDOW_NAME, false, 91)]
        public static void Open()
        {
            UProjectManager window = GetWindow<UProjectManager>();
            WindowIcon = UEditorResourcesHelper.GetIcon("Window");
            window.titleContent = new GUIContent(WINDOW_NAME, WindowIcon);
            window.maxSize = WindowMaxSize;
            window.minSize = WindowMinSize;
            window.Show();
        }

        /// <summary>
        /// This function is called when the window becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            itemProperties = ReflectionHelper.GetItemProperties();

            if (treeViewState == null)
                treeViewState = new TreeViewState();
            treeView = new UPMTreeView(treeViewState, itemProperties);
            treeView.ExpandAll();
            searchField = new SearchField();
            searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;

            InitializeUItemEntities(itemProperties);
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        protected virtual void OnGUI()
        {
            GUILayout.BeginVertical();
            DrawSearch();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            DrawTreeView();
            GUILayout.EndVertical();

            UEditor.VerticalLine(Screen.height, 1.5f);

            GUILayout.BeginVertical(GUILayout.MinWidth(300), GUILayout.MaxWidth(Screen.width - 100));
            scrollPosEditor = GUILayout.BeginScrollView(scrollPosEditor);
            DrawItemGUI(treeViewState.lastClickedID);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Label(GUIContent.none, "ToolbarButton");

            GUILayout.EndVertical();
        }

        protected virtual void OnInspectorUpdate()
        {
            Repaint();
        }

        protected virtual void DrawTreeView()
        {
            Rect rect = GUILayoutUtility.GetRect(100, 300, 0, Screen.height);
            treeView.OnGUI(rect);
        }

        protected virtual void DrawSearch()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Space(100);
            GUILayout.FlexibleSpace();
            treeView.searchString = searchField.OnToolbarGUI(treeView.searchString);
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draw UItem GUI methods in UManager window.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gui"></param>
        protected virtual void DrawItemGUI(int selectedID)
        {
            if (selectedID <= 0)
            {
                GUILayout.Label(GUIContent.none);
                return;
            }

            for (int i = 0; i < itemProperties.Length; i++)
            {
                if (itemProperties[i].GetID() == selectedID)
                {
                    DrawItemHeader(itemProperties[i].GetName());
                    if (itemProperties[i].GetOnGUIDelegate() != null)
                    {
                        GUILayout.BeginVertical();
                        itemProperties[i].InvokeOnGUI();
                        GUILayout.EndVertical();
                    }
                }
            }
        }

        protected virtual void DrawItemHeader(string itemName)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label(itemName, UEditorStyles.LargeCenteredGrayLabel);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }

        /// <summary>
        /// Initialize all UItems when the window becomes enabled and active.
        /// Called OnEnable method of each UItem class.
        /// </summary>
        /// <param name="itemProperties"></param>
        public void InitializeUItemEntities(ItemProperty[] itemProperties)
        {
            for (int i = 0; i < itemProperties.Length; i++)
            {
                if (itemProperties[i].GetOnEnableDelegate() != null)
                {
                    itemProperties[i].InvokeOnEnable();
                }
            }
        }
    }
}