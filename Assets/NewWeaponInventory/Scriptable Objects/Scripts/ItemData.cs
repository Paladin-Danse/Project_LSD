using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ����
public enum ItemType
{
    Resource, // ���
    Equipable, // ���
    Consumable // �Ҹ�ǰ(ü��)
}

// ���� ����
public enum ConsumableType
{
    Health // ü��
}

// ȸ���Ǵ� ���� �� ��
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

// ��ũ���ͺ� ������Ʈ ����ϱ�(ScriptableObject)
[CreateAssetMenu(fileName = "Item", menuName = "New ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Info")] // ����
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")] // ����
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")] // �Ҹ�ǰ
    public ItemDataConsumable[] consumables;

}