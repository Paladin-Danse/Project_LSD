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

namespace UnrealFPS.Editor
{
	public class UCreateMenuItems
	{
		[MenuItem(UEditorPaths.DEFAULT + "Create/Player", false, 21)]
		public static void CreatePlayer()
		{
			UMenuItemsProperties itemsProperties = UEditorResourcesHelper.GetMenuItemsProperties();
			if (itemsProperties == null)
			{
				UDisplayDialogs.ErrorCreateItemPropNull("Player");
				return;
			}
			if (itemsProperties.GetPlayerProperties().player == null)
			{
				UDisplayDialogs.ErrorCreateItemObjNull("Player");
				return;
			}
			Object.Instantiate(itemsProperties.GetPlayerProperties().player, itemsProperties.GetPlayerProperties().position, Quaternion.Euler(itemsProperties.GetPlayerProperties().rotation));
		}

		[MenuItem(UEditorPaths.DEFAULT + "Create/Weapon", false, 22)]
		public static void CreateWeapon()
		{
			UMenuItemsProperties itemsProperties = UEditorResourcesHelper.GetMenuItemsProperties();
			if (itemsProperties == null)
			{
				UDisplayDialogs.ErrorCreateItemPropNull("Weapon");
				return;
			}

			if (itemsProperties.GetWeaponProperties().weapon == null)
			{
				UDisplayDialogs.ErrorCreateItemObjNull("Weapon");
				return;
			}
			Object.Instantiate(itemsProperties.GetWeaponProperties().weapon, itemsProperties.GetWeaponProperties().position, Quaternion.Euler(itemsProperties.GetWeaponProperties().rotation));
		}

		[MenuItem(UEditorPaths.DEFAULT + "Create/Ammo", false, 23)]
		public static void CreateWeaponAmmo()
		{
			UMenuItemsProperties itemsProperties = UEditorResourcesHelper.GetMenuItemsProperties();
			if (itemsProperties == null)
			{
				UDisplayDialogs.ErrorCreateItemPropNull("Ammo");
				return;
			}

			if (itemsProperties.GetWeaponAmmoProperties().weaponAmmo == null)
			{
				UDisplayDialogs.ErrorCreateItemObjNull("Ammo");
				return;
			}
			Object.Instantiate(itemsProperties.GetWeaponAmmoProperties().weaponAmmo, itemsProperties.GetWeaponAmmoProperties().position, Quaternion.Euler(itemsProperties.GetWeaponAmmoProperties().rotation));
		}

		[MenuItem(UEditorPaths.DEFAULT + "Create/AI", false, 24)]
		public static void CreateAI()
		{
			UMenuItemsProperties itemsProperties = UEditorResourcesHelper.GetMenuItemsProperties();
			if (itemsProperties == null)
			{
				UDisplayDialogs.ErrorCreateItemPropNull("AI");
				return;
			}

			if (itemsProperties.GetAIProperties().ai == null)
			{
				UDisplayDialogs.ErrorCreateItemObjNull("AI");
				return;
			}
			Object.Instantiate(itemsProperties.GetAIProperties().ai, itemsProperties.GetAIProperties().position, Quaternion.Euler(itemsProperties.GetAIProperties().rotation));
		}

		[MenuItem(UEditorPaths.DEFAULT + "Create/Spawn Zone", false, 25)]
		public static void CreateSpawnZone()
		{
			UMenuItemsProperties itemsProperties = UEditorResourcesHelper.GetMenuItemsProperties();
			if (itemsProperties == null)
			{
				UDisplayDialogs.ErrorCreateItemPropNull("Spawn Zone");
				return;
			}

			if (itemsProperties.GetSpawnZoneProperties().spawnZone == null)
			{
				UDisplayDialogs.ErrorCreateItemObjNull("Spawn Zone");
				return;
			}
			Object.Instantiate(itemsProperties.GetSpawnZoneProperties().spawnZone, itemsProperties.GetSpawnZoneProperties().position, Quaternion.Euler(itemsProperties.GetSpawnZoneProperties().rotation));
		}
	}
}