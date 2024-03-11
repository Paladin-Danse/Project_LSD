using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponStat
{
    public StatModifyType statModifyType;

    [Header("Default Weapon Stat")]
    public float spread;
    public float recoil;
    public float preFireDelay;
    public float fireDelay;
    public float reloadDelay;
    public int bulletPerFire;
    public AttackStat attackStat;

    public void Add(WeaponStat other)
    {
        spread += other.spread;
        recoil += other.recoil;
        preFireDelay += other.preFireDelay;
        fireDelay += other.fireDelay;
        reloadDelay += other.reloadDelay;
        bulletPerFire += other.bulletPerFire;
    }

    public void Multiply(WeaponStat other)
    {
        spread *= other.spread;
        recoil *= other.recoil;
        preFireDelay *= other.preFireDelay;
        fireDelay *= other.fireDelay;
        reloadDelay *= other.reloadDelay;
        bulletPerFire *= other.bulletPerFire;
    }

    public void Override(WeaponStat other)
    {
        spread = other.spread;
        recoil = other.recoil;
        preFireDelay = other.preFireDelay;
        fireDelay = other.fireDelay;
        reloadDelay = other.reloadDelay;
        bulletPerFire *= other.bulletPerFire;
    }
}
