using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultWeaponData", menuName = "Stats/Weapon/Default", order = 0)]
public class WeaponStatSO : ScriptableObject
{
    public GameObject weapon;
    public GameObject firePoint;
    public WeaponStat weaponStat;
}
