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
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnrealFPS.Editor
{
    public partial class ProjectSetupAssistant : EditorWindow
    {
        public const string WINDOW_NAME = "Project Setup Assistant";

        private static Vector2 windowSize = new Vector2(600, 400);

        private TreeViewState treeViewState;
        private PSATreeView treeView;
        private SearchField searchField;
        private ItemProperty[] itemProperties;

        private Vector2 scrollPosItems;
        private Vector2 scrollPosEditor;

        /// <summary>
        /// Open Project Setup Assistant window.
        /// </summary>
        [MenuItem(UEditorPaths.DEFAULT + WINDOW_NAME, false, 999)]
        public static void Open()
        {
            ProjectSetupAssistant window = ScriptableObject.CreateInstance(typeof(ProjectSetupAssistant)) as ProjectSetupAssistant;
            window.titleContent = new GUIContent(WINDOW_NAME);
            window.maxSize = windowSize;
            window.minSize = windowSize;
            window.ShowUtility();
        }

        /// <summary>
        /// This function is called when the window becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            itemProperties = ReflectionHelper.GetItemProperties();

            if (treeViewState == null)
                treeViewState = new TreeViewState();

            treeView = new PSATreeView(treeViewState, itemProperties);
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
            DrawTreeView();
            DrawProjectSetupStatusBar(itemProperties);
            GUILayout.Space(3);
            GUILayout.EndVertical();

            scrollPosEditor = GUILayout.BeginScrollView(scrollPosEditor);
            GUILayout.BeginVertical(GUILayout.MinWidth(300), GUILayout.MaxWidth(Screen.width - 100));
            UEditor.VerticalLine(Screen.height, 1.5f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginVertical();
            DrawItemGUI(treeViewState.lastClickedID);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            GUILayout.EndHorizontal();

            GUILayout.Label(GUIContent.none, "ToolbarButton");

            GUILayout.EndVertical();
        }

        protected virtual void DrawTreeView()
        {
            Rect rect = GUILayoutUtility.GetRect(100, 200, 0, Screen.height);
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
            for (int i = 0; i < itemProperties.Length; i++)
            {

                if (itemProperties[i].GetID() == selectedID && itemProperties[i].GetOnGUIDelegate() != null)
                {
                    itemProperties[i].InvokeOnGUI();
                }
            }
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

        /// <summary>
        /// Draw project setup complete status bar.
        /// </summary>
        /// <param name="itemProperties"></param>
        protected virtual void DrawProjectSetupStatusBar(ItemProperty[] itemProperties)
        {
            if (itemProperties == null && itemProperties.Length == 0)
            {
                return;
            }
            int maxCount = itemProperties.Length;
            int nonReadyCount = itemProperties.Where(t => !t.IsReady()).ToArray().Length;

            float value = Mathf.InverseLerp(maxCount, 0, nonReadyCount);

            GUILayout.BeginHorizontal();
            GUILayout.Space(3);
            Rect rect = GUILayoutUtility.GetRect(1, 18);
            EditorGUI.ProgressBar(rect, value, "");
            string valueLabel = value > 0 ? (value * 100).ToString("#") + "%" : 0 + "%";
            EditorGUI.LabelField(rect, "Complete: " + valueLabel, UEditorStyles.CenteredBoldLabel);
            GUILayout.Space(3);
            GUILayout.EndHorizontal();
        }
    }
}