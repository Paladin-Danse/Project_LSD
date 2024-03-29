using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ź�� ���� </summary>
[System.Serializable]
public enum AmmoType
{
    Rifle, // ������
    Pistol, // ����
}
[CreateAssetMenu(fileName = "WeaponAmmoData", menuName = "New WeaponAmmoData")]
public class WeaponAmmoData : ScriptableObject
{
    [Header("AmmoType")]
    public AmmoType ammoType;
}
