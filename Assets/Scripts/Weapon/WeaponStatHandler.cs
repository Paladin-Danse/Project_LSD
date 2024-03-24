using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatHandler : StatHandlerBase<WeaponStat>
{
    Weapon curWeapon;

    protected void Awake()
    {
        base.Awake();
    }

    public void EquipWeapon(Weapon weapon) 
    {
        curWeapon = weapon;
        baseStat = curWeapon.baseStat;
        baseStat.weaponStatFlag = (WeaponStat.WeaponStatFlag) int.MaxValue;
        foreach (Mod mod in weapon.mods) 
        {
            statModifiers.Add(mod.modStat);
        }
        UpdateStats();
        weapon.GetWeaponStat = () => { return currentStat; };
    }

    public void UnequipWeapon(Weapon weapon) 
    {
        foreach (Mod mod in weapon.mods)
        {
            statModifiers.Remove(mod.modStat);
        }
        weapon.GetWeaponStat = () => { return weapon.baseStat; };
        curWeapon = null;
    }
}
