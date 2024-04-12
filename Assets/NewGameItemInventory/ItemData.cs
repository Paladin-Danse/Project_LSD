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

// ȸ���Ǵ� ���� �� ��
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
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
}