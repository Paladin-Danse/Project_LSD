using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 스크립터블 오브젝트 상속하기(ScriptableObject)
[CreateAssetMenu(fileName = "Item", menuName = "New ItemWeaponData")]
public class ItemWeaponData : ScriptableObject
{
    [Header("Info")] // 설명
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("WeaponStatSO")] // 설명
    public WeaponStatSO weaponStatSO;

}