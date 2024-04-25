using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Health))]
public class CharacterStatHandler : StatHandlerBase<CharacterStat>
{
    [SerializeField]
    CharacterStatSO baseStatSO;

    [SerializeField]
    Health health;

    protected void Awake()
    {
        base.Awake();
        TryGetComponent(out health);
    }

    // Start is called before the first frame update
    protected void Start()
    {
        health.TakeDamage = TakeDamageWithDefense;
        health.MaxHealth = () => { return currentStat.maxHealth; };
        health.RegenHealth = () => { return currentStat.regenHealthPerSec; };
    }

    protected override void InitStat() 
    {
        if (baseStatSO != null) 
        {
            baseStat = (Instantiate(baseStatSO) as CharacterStatSO).characterStat;
        }
    }

    public void TakeDamageWithDefense(float damage)
    {
        if (health.curHealth == 0) return;

        health.OnTakeDamage?.Invoke();
        health.HealthChanged?.Invoke();

        health.curHealth = Mathf.Max(health.curHealth - (Mathf.Max(0, (damage - currentStat.defense) * currentStat.defenseRateMultiplyConverted)), 0);

        if (health.curHealth == 0)
            health.OnDie?.Invoke();
    }
}
