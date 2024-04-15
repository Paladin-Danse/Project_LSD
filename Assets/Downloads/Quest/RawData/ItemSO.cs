using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(AssetPath = "Resources/DB", ExcelName = "ItemSheet")]
public class ItemSO : ScriptableObject
{
	public List<ItemData> Entities; // Replace 'EntityType' to an actual type that is serializable.
}
