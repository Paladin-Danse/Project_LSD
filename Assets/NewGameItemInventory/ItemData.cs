using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ����
public enum ItemType
{
    Weapon, // ����
    Equipable // ���
}

// ���� ����
public enum ConsumableType
{
    Health // ü��
}

// ��ũ���ͺ� ������Ʈ ����ϱ�(ScriptableObject)
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] // ����
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