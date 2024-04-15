using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public class WeaponStat : Stat
{
    [Flags]
    [Serializable]
    public enum WeaponStatFlag
    {
        None = 0,
        Magazine = 1 << 0,
        Spread = 1 << 1,
        Recoil = 1 << 2,
        PreFireDelay = 1 << 3,
        FireDelay = 1 << 4,
        ReloadDelay = 1 << 5,
        BulletPerFire = 1 << 6,
        AttackStat = 1 << 7,
    }

    public WeaponStatFlag weaponStatFlag;

    [Header("Default Weapon Stat")]
    public AmmoType e_useAmmo;
    public int magazine;
    public float spread;
    public float recoil;
    public float preFireDelay;
    public float fireDelay;
    public float reloadDelay;
    public int bulletPerFire;
    public AttackStat attackStat;

    public override void OverlapStats(Stat other)
    {
        if(other is  WeaponStat) 
            OverlapStat(other as WeaponStat);
    }
    public void OverlapStat(WeaponStat other)
    {
        Func<float, float, float> op = (o1, o2) => o1;

        if (other.statModifyType == StatModifyType.Add)
            op = (o1, o2) => o1 + o2;
        else if (other.statModifyType == StatModifyType.Multiply)
            op = (o1, o2) => o1 * o2;
        else if (other.statModifyType == StatModifyType.Override)
            op = (o1, o2) => o2;

        if ((other.weaponStatFlag & WeaponStatFlag.Spread) != 0)
            spread = op(spread, other.spread);
        if ((other.weaponStatFlag & WeaponStatFlag.Recoil) != 0)
            recoil = op(recoil, other.recoil);
        if ((other.weaponStatFlag & WeaponStatFlag.PreFireDelay) != 0)
            preFireDelay = op(preFireDelay, other.preFireDelay);
        if ((other.weaponStatFlag & WeaponStatFlag.FireDelay) != 0)
            fireDelay = op(fireDelay, other.fireDelay);
        if ((other.weaponStatFlag & WeaponStatFlag.ReloadDelay) != 0)
            reloadDelay = op(reloadDelay, other.reloadDelay);

        Func<int, int, int> op2 = (o1, o2) => o1;

        if (other.statModifyType == StatModifyType.Add)
            op2 = (o1, o2) => o1 + o2;
        else if (other.statModifyType == StatModifyType.Multiply)
            op2 = (o1, o2) => o1 * o2;
        else if (other.statModifyType == StatModifyType.Override)
            op2 = (o1, o2) => o2;

        if ((other.weaponStatFlag & WeaponStatFlag.Magazine) != 0)
            magazine = op2(magazine, other.magazine);
        if ((other.weaponStatFlag & WeaponStatFlag.BulletPerFire) != 0)
            bulletPerFire = op2(bulletPerFire, other.bulletPerFire);

        if ((other.weaponStatFlag & WeaponStatFlag.AttackStat) != 0)
            attackStat.OverlapStats(other.attackStat);
    }
}
