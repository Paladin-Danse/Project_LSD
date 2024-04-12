using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 종류
public enum ItemType
{
    Weapon, // 무기
    Equipable // 장비
}

// 스텟 종류
public enum ConsumableType
{
    Health // 체력
}

// 스크립터블 오브젝트 상속하기(ScriptableObject)
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] // 설명
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;
    public Dictionary<string, int> itemStatValues;

    public void Init(InventoryData inventorySO)
    {
        itemStatValues = new Dictionary<string, int>();
        foreach (string key in inventorySO.itemStatValues.Keys)
            itemStatValues.Add(key, 0);
    }
    public void Init()
    {
        itemStatValues = new Dictionary<string, int>();
    }
    public void AddStat(string name, int value)
    {
        itemStatValues.Add(name, value);
    }
}