using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum AttackType
{
    Melee, Bullet, Explosion
}

[Serializable]
public class AttackStat : Stat
{
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

    public void Add(AttackStat other)
    {
        Func<float, float, float> op = (o1, o2) => o1 + o2;
        damage = UpdateStat(op, damage, other.damage);
        range = UpdateStat(op, range, other.range);
        bulletSpeed = UpdateStat(op, bulletSpeed, other.bulletSpeed);
        explosionRange = UpdateStat(op, explosionRange, other.explosionRange);
        maxExplosionDamageRange = UpdateStat(op, maxExplosionDamageRange, other.maxExplosionDamageRange);

        knockBackPower = UpdateStat(op, knockBackPower, other.knockBackPower);
        knockBackTime = UpdateStat(op, knockBackTime, other.knockBackTime);
    }

    public void Multiply(AttackStat other)
    {
        Func<float, float, float> op = (o1, o2) => o1 * o2;
        damage = UpdateStat(op, damage, other.damage);
        range = UpdateStat(op, range, other.range);
        bulletSpeed = UpdateStat(op, bulletSpeed, other.bulletSpeed);
        explosionRange = UpdateStat(op, explosionRange, other.explosionRange);
        maxExplosionDamageRange = UpdateStat(op, maxExplosionDamageRange, other.maxExplosionDamageRange);

        knockBackPower = UpdateStat(op, knockBackPower, other.knockBackPower);
        knockBackTime = UpdateStat(op, knockBackTime, other.knockBackTime);
    }

    public void Override(AttackStat other)
    {
        Func<float, float, float> op = (o1, o2) => o2;
        damage = UpdateStat(op, damage, other.damage);
        range = UpdateStat(op, range, other.range);
        bulletSpeed = UpdateStat(op, bulletSpeed, other.bulletSpeed);
        explosionRange = UpdateStat(op, explosionRange, other.explosionRange);
        maxExplosionDamageRange = UpdateStat(op, maxExplosionDamageRange, other.maxExplosionDamageRange);

        knockBackPower = UpdateStat(op, knockBackPower, other.knockBackPower);
        knockBackTime = UpdateStat(op, knockBackTime, other.knockBackTime);
    }
}
