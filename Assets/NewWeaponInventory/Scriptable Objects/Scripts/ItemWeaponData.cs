using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ��ũ���ͺ� ������Ʈ ����ϱ�(ScriptableObject)
[CreateAssetMenu(fileName = "Item", menuName = "New ItemWeaponData")]
public class ItemWeaponData : ScriptableObject
{
    [Header("Info")] // ����
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("WeaponStatSO")] // ����
    public WeaponStatSO weaponStatSO;

}