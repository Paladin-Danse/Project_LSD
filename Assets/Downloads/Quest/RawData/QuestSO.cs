using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(AssetPath = "Resources/DB", ExcelName = "QuestDataSheet")]
public class QuestSO : ScriptableObject
{
	public List<QuestData> Entities; // Replace 'EntityType' to an actual type that is serializable.
}
