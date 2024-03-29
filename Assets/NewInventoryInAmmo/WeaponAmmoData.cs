using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Åº¾à Á¾·ù </summary>
[System.Serializable]
public enum AmmoType
{
    Rifle, // ¶óÀÌÇÃ
    Pistol, // ±ÇÃÑ
}
[CreateAssetMenu(fileName = "WeaponAmmoData", menuName = "New WeaponAmmoData")]
public class WeaponAmmoData : ScriptableObject
{
    [Header("AmmoType")]
    public AmmoType ammoType;
}
