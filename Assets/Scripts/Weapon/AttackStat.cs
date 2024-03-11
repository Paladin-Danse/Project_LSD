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
public class AttackStat
{
    public StatModifyType statModifyType;

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
        damage += other.damage;
        range += other.range;
        bulletSpeed += other.bulletSpeed;
        explosionRange += other.explosionRange;
        maxExplosionDamageRange += other.maxExplosionDamageRange;

        knockBackPower += other.knockBackPower;
        knockBackTime += other.knockBackTime;
    }

    public void Multiply(AttackStat other)
    {
        damage *= other.damage;
        range *= other.range;
        bulletSpeed *= other.bulletSpeed;
        explosionRange *= other.explosionRange;
        maxExplosionDamageRange *= other.maxExplosionDamageRange;

        knockBackPower *= other.knockBackPower;
        knockBackTime *= other.knockBackTime;
    }

    public void Override(AttackStat other)
    {
        damage = other.damage;
        range = other.range;
        bulletSpeed = other.bulletSpeed;
        explosionRange = other.explosionRange;
        maxExplosionDamageRange = other.maxExplosionDamageRange;

        knockBackPower = other.knockBackPower;
        knockBackTime = other.knockBackTime;
    }
}
