/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using static UnrealFPS.Editor.ProjectSetupAssistant;

namespace UnrealFPS.Editor
{
	public class PSATreeView : TreeView
	{
		private TreeViewItem root;
		private List<TreeViewItem> allItems;

		public PSATreeView(TreeViewState treeViewState, ItemProperty[] itemProperties) : base(treeViewState)
		{
			InitializeTreeViewItems(itemProperties);
			SetupParentsAndChildrenFromDepths(root, allItems);
			Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			if (root == null)
				root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
			return root;
		}

		protected virtual void InitializeTreeViewItems(ItemProperty[] itemProperties)
		{
			root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
			allItems = new List<TreeViewItem>();

			string[] types = Enum.GetNames(typeof(ItemType));

			for (int i = 0; i < types.Length; i++)
			{
				ItemProperty[] items = itemProperties.Where(t => t.GetItemType().ToString() == types[i]).OrderBy(t => Math.Abs(t.GetIndex())).ToArray();
				List<TreeViewItem> itemsTreeView = new List<TreeViewItem>()
				{
					new TreeViewItem { id = -(i + 1), depth = 0, displayName = types[i] }
				};
				for (int j = 0; j < items.Length; j++)
				{
					itemsTreeView.Add(new TreeViewItem { id = items[j].GetID(), depth = 1, displayName = items[j].GetName() });
				}
				allItems.AddRange(itemsTreeView);
			}
		}
	}
}