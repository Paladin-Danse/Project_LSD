using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public enum StatModifyType 
{
    Add, Multiply, Override
}

public class Stat 
{
    public StatModifyType statModifyType;

    public static int UpdateStat(Func<int, int, int> operation, int one, int other)
    {
        if (other != 0) return operation(one, other);
        else return one;
    }

    public static float UpdateStat(Func<float, float, float> operation, float one, float other)
    {
        if (other != 0f) return operation(one, other);
        else return one;
    }
}

[Serializable]
public class CharacterStat : Stat
{
    [Header("Character Stat")]
    public float maxHealth;
    public float defense;
    [SerializeField]
    private float _defenseRate;
    public float regenHealthPerSec;
    public float moveSpeed;
    public float criticalDamageRate;

    public float defenseRate { get { return 100f / (100f + _defenseRate); } private set { _defenseRate = value; } }

    public void Add(CharacterStat other) 
    {
        Func<float, float, float> op = (o1, o2)=> o1 + o2;
        maxHealth = UpdateStat(op, maxHealth, other.maxHealth);
        defense = UpdateStat(op, defense, other.defense);
        defenseRate = UpdateStat(op, defenseRate, other.defenseRate);
        regenHealthPerSec = UpdateStat(op, regenHealthPerSec, other.regenHealthPerSec);
        moveSpeed = UpdateStat(op, moveSpeed, other.moveSpeed);
        criticalDamageRate = UpdateStat(op, criticalDamageRate, other.criticalDamageRate);
    }

    public void Multiply(CharacterStat other)
    {
        Func<float, float, float> op = (o1, o2) => o1 * o2;
        maxHealth = UpdateStat(op, maxHealth, other.maxHealth);
        defense = UpdateStat(op, defense, other.defense);
        defenseRate = UpdateStat(op, defenseRate, other.defenseRate);
        regenHealthPerSec = UpdateStat(op, regenHealthPerSec, other.regenHealthPerSec);
        moveSpeed = UpdateStat(op, moveSpeed, other.moveSpeed);
        criticalDamageRate = UpdateStat(op, criticalDamageRate, other.criticalDamageRate);
    }

    public void Override(CharacterStat other)
    {
        Func<float, float, float> op = (o1, o2) => o2;
        maxHealth = UpdateStat(op, maxHealth, other.maxHealth);
        defense = UpdateStat(op, defense, other.defense);
        defenseRate = UpdateStat(op, defenseRate, other.defenseRate);
        regenHealthPerSec = UpdateStat(op, regenHealthPerSec, other.regenHealthPerSec);
        moveSpeed = UpdateStat(op, moveSpeed, other.moveSpeed);
        criticalDamageRate = UpdateStat(op, criticalDamageRate, other.criticalDamageRate);
    }
}