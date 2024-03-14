using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent (typeof(CharacterStatHandler))]
public class Health : MonoBehaviour
{
    private CharacterStatHandler characterStatHandler;

    public float maxHealth { get { return characterStatHandler.currentStat.maxHealth; } }
    public float curHealth;
    public bool IsDead => curHealth == 0;

    public event Action OnDie;
    public event Action OnHeal;
    public event Action OnTakeDamage;

    private void Awake()
    {
        characterStatHandler = GetComponent<CharacterStatHandler>();
    }

    private void Start()
    {
        curHealth = maxHealth;
    }

    private void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        OnTakeDamage?.Invoke();

        if (curHealth == 0) return;

        curHealth = Mathf.Max(curHealth - ((damage - characterStatHandler.currentStat.defense) * characterStatHandler.currentStat.defenseRateMultiplyConverted), 0);

        if (curHealth == 0)
            OnDie?.Invoke();
    }

    public void TakeHeal(float addHealth)
    {
        OnHeal?.Invoke();
        curHealth = MathF.Min(maxHealth, curHealth + addHealth);
    }

    private void RegenHealth()
    {
        curHealth = MathF.Min(maxHealth, curHealth + characterStatHandler.currentStat.regenHealthPerSec);
    }
}
