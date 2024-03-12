using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public class WeaponStat : Stat
{
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
        Func<float, float, float> op = (o1, o2) => o1 + o2;
        spread = UpdateStat(op, spread, other.spread);
        recoil = UpdateStat(op, recoil, other.recoil);
        preFireDelay = UpdateStat(op, preFireDelay, other.preFireDelay);
        fireDelay = UpdateStat(op, fireDelay, other.fireDelay);
        reloadDelay = UpdateStat(op, reloadDelay, other.reloadDelay);

        Func<int, int, int> op2 = (o1, o2) => o1 + o2;
        bulletPerFire = UpdateStat(op2, bulletPerFire, other.bulletPerFire);
    }

    public void Multiply(WeaponStat other)
    {
        Func<float, float, float> op = (o1, o2) => o1 * o2;
        spread = UpdateStat(op, spread, other.spread);
        recoil = UpdateStat(op, recoil, other.recoil);
        preFireDelay = UpdateStat(op, preFireDelay, other.preFireDelay);
        fireDelay = UpdateStat(op, fireDelay, other.fireDelay);
        reloadDelay = UpdateStat(op, reloadDelay, other.reloadDelay);

        Func<int, int, int> op2 = (o1, o2) => o1 * o2;
        bulletPerFire = UpdateStat(op2, bulletPerFire, other.bulletPerFire);
    }

    public void Override(WeaponStat other)
    {
        Func<float, float, float> op = (o1, o2) => o2;
        spread = UpdateStat(op, spread, other.spread);
        recoil = UpdateStat(op, recoil, other.recoil);
        preFireDelay = UpdateStat(op, preFireDelay, other.preFireDelay);
        fireDelay = UpdateStat(op, fireDelay, other.fireDelay);
        reloadDelay = UpdateStat(op, reloadDelay, other.reloadDelay);

        Func<int, int, int> op2 = (o1, o2) => o2;
        bulletPerFire = UpdateStat(op2, bulletPerFire, other.bulletPerFire);
    }
}
