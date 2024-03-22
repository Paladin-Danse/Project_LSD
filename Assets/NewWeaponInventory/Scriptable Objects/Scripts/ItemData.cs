using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 종류
public enum ItemType
{
    Resource, // 재료
    Equipable, // 장비
    Consumable // 소모품(체력)
}

// 스텟 종류
public enum ConsumableType
{
    Health // 체력
}

// 회복되는 스텟 과 값
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

// 스크립터블 오브젝트 상속하기(ScriptableObject)
[CreateAssetMenu(fileName = "Item", menuName = "New ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Info")] // 설명
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")] // 스택
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")] // 소모품
    public ItemDataConsumable[] consumables;

}