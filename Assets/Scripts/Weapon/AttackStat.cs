using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static WeaponStat;

[Serializable]
public enum AttackType
{
    Melee, Bullet, Explosion
}

[Serializable]
public class AttackStat : Stat
{
    [Flags]
    [Serializable]
    public enum AttackStatFlag
    {
        None = 0,
        Damage = 1 << 0,
        Range = 1 << 1,
        BulletSpeed = 1 << 2,
        ExplosionRange = 1 << 3,
        MaxExplosionDamageRange = 1 << 4,
        IsKnockBackEnable = 1 << 5,
        KnockBackPower = 1 << 6,
        KnockBackTime = 1 << 7,
    }

    public AttackType AttackType;
    public AttackStatFlag attackStatFlag;

    public float damage;
    public float range;
    public float bulletSpeed;

    [Header("Stat for Launcher")]
    public float explosionRange;
    [Range(0f, 1f)]
    public float maxExplosionDamageRange;

    [Header("KnockBack Stat")]
    public bool IsKnockbackEnable;
    public float knockBackPower;
    public float knockBackTime;

    public void OverlapStats(AttackStat other)
    {
        Func<float, float, float> op = (o1, o2) => o1;

        if (other.statModifyType == StatModifyType.Add)
            op = (o1, o2) => o1 + o2;
        else if (other.statModifyType == StatModifyType.Multiply)
            op = (o1, o2) => o1 * o2;
        else if (other.statModifyType == StatModifyType.Override)
            op = (o1, o2) => o2;

        if ((other.attackStatFlag & AttackStatFlag.Damage) != 0)
            damage = op(damage, other.damage);
        if ((other.attackStatFlag & AttackStatFlag.Range) != 0)
            range = op(range, other.range);
        if ((other.attackStatFlag & AttackStatFlag.BulletSpeed) != 0)
            bulletSpeed = op(bulletSpeed, other.bulletSpeed);

        if ((other.attackStatFlag & AttackStatFlag.ExplosionRange) != 0)
            explosionRange = op(explosionRange, other.explosionRange);
        if ((other.attackStatFlag & AttackStatFlag.MaxExplosionDamageRange) != 0)
            maxExplosionDamageRange = op(maxExplosionDamageRange, other.maxExplosionDamageRange);

        if ((other.attackStatFlag & AttackStatFlag.KnockBackPower) != 0)
            knockBackPower = op(knockBackPower, other.knockBackPower);
        if ((other.attackStatFlag & AttackStatFlag.KnockBackTime) != 0)
            knockBackTime = op(knockBackTime, other.knockBackTime);

        Func<bool, bool, bool> op2 = (o1, o2) => o1;

        if (other.statModifyType == StatModifyType.Add)
            op2 = (o1, o2) => o1 | o2;
        else if (other.statModifyType == StatModifyType.Multiply)
            op2 = (o1, o2) => o1 & o2;
        else if (other.statModifyType == StatModifyType.Override)
            op2 = (o1, o2) => o2;

        if ((other.attackStatFlag & AttackStatFlag.IsKnockBackEnable) != 0)
            IsKnockbackEnable = op2(IsKnockbackEnable, other.IsKnockbackEnable);
    }
}
