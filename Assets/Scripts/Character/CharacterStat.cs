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

[Serializable]
public class CharacterStat
{
    public StatModifyType statModifyType;

    public float maxHealth;
    public float defense;
    [SerializeField]
    private float _defenseRate;
    public float regenHealthPerSec;
    public float moveSpeed;
    public float criticalDamageRate;

    public float defenseRate { get { return 100f / (100f + defenseRate); } private set { _defenseRate = value; } }

    public void Add(CharacterStat other) 
    {
        maxHealth += other.maxHealth;
        defense += other.defense;
        defenseRate += other.defenseRate;
        regenHealthPerSec += other.regenHealthPerSec;
        moveSpeed += other.moveSpeed;
        criticalDamageRate += other.criticalDamageRate;
    }

    public void Multiply(CharacterStat other)
    {
        maxHealth *= other.maxHealth;
        defense *= other.defense;
        defenseRate *= other.defenseRate;
        regenHealthPerSec *= other.regenHealthPerSec;
        moveSpeed *= other.moveSpeed;
        criticalDamageRate *= other.criticalDamageRate;
    }

    public void Override(CharacterStat other)
    {
        maxHealth = other.maxHealth;
        defense = other.defense;
        defenseRate = other.defenseRate;
        regenHealthPerSec = other.regenHealthPerSec;
        moveSpeed = other.moveSpeed;
        criticalDamageRate = other.criticalDamageRate;
    }
}