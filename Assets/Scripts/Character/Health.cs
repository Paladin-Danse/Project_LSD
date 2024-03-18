using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TakeDamage(float damageTaken);
public delegate float MaxHealth();

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField]
    float _maxHealth;
    public float maxHealth { get { return MaxHealth(); } }
    public float curHealth;
    public bool IsDead => curHealth == 0;

    public Action OnDie;
    public Action OnHeal;
    public Action OnTakeDamage;

    public TakeDamage TakeDamage;
    public MaxHealth MaxHealth;

    private void Awake()
    {
        TakeDamage = TakeDamageWithoutDefense;
        MaxHealth = () => { return _maxHealth; };
    }

    private void Start()
    {
        curHealth = maxHealth;
    }

    private void Update()
    {

    }

    public void TakeDamageWithoutDefense(float damage)
    {
        OnTakeDamage?.Invoke();

        if (curHealth == 0) return;

        curHealth = Mathf.Max(curHealth - damage, 0);

        if (curHealth == 0)
            OnDie?.Invoke();
    }

    public void TakeHeal(float addHealth)
    {
        OnHeal?.Invoke();
        curHealth = MathF.Min(maxHealth, curHealth + addHealth);
    }

    private void RegenHealth(float health)
    {
        curHealth = MathF.Min(maxHealth, curHealth + health);
    }
}
