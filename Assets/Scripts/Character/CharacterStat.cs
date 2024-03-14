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

    public virtual void OverlapStats(Stat other){ }
}


[Serializable]
public class CharacterStat : Stat
{
    [Flags]
    [Serializable]
    public enum CharacterStatFlag
    {
        NONE = 0,
        MAX_HEALTH = 1 << 0,
        DEFENSE = 1 << 1,
        DEFENSE_RATE = 1 << 2,
        REGEN_HEALTH = 1 << 3,
        MOVE_SPEED = 1 << 4,
        CRIT_DAMAGE = 1 << 5,
    }

    public CharacterStatFlag characterStatFlag;

    [Header("Character Stat")]
    public float maxHealth;
    public float defense;
    [SerializeField]
    public float defenseRate;
    public float regenHealthPerSec;
    public float moveSpeed;
    public float criticalDamageRate;

    public float defenseRateMultiplyConverted { get { return 100f / (100f + defenseRate); } }

    public override void OverlapStats(Stat other)
    {
        if (other is CharacterStat)
            this.OverlapStat(other as CharacterStat);
    }

    public void OverlapStat(CharacterStat other) 
    {
        Func<float, float, float> op = (o1, o2) => o1;
        
        if(other.statModifyType == StatModifyType.Add)
            op = (o1, o2) => o1 + o2;
        else if(other.statModifyType == StatModifyType.Multiply)
            op = (o1, o2) => o1 * o2;
        else if(other.statModifyType == StatModifyType.Override)
            op = (o1, o2) => o2;

        if ((other.characterStatFlag & CharacterStatFlag.MAX_HEALTH) != 0)
            maxHealth = op(maxHealth, other.maxHealth);
        if ((other.characterStatFlag & CharacterStatFlag.DEFENSE) != 0)
            defense = op(defense, other.defense);
        if ((other.characterStatFlag & CharacterStatFlag.DEFENSE_RATE) != 0)
            defenseRate = op(defenseRate, other.defenseRate);
        if ((other.characterStatFlag & CharacterStatFlag.REGEN_HEALTH) != 0)
            regenHealthPerSec = op(regenHealthPerSec, other.regenHealthPerSec);
        if ((other.characterStatFlag & CharacterStatFlag.MOVE_SPEED) != 0)
            moveSpeed = op(moveSpeed, other.moveSpeed);
        if ((other.characterStatFlag & CharacterStatFlag.CRIT_DAMAGE) != 0)
            criticalDamageRate = op(criticalDamageRate, other.criticalDamageRate);
    }
}