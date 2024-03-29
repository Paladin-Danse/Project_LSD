using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField]
    float _maxHealth;
    public float maxHealth { get { return MaxHealth(); } }
    public float curHealth;

    [SerializeField]
    float _regenHealth;
    public float regenHealthPerSec {  get { return RegenHealth(); } }

    public bool IsDead => curHealth == 0;

    public Action OnDie;
    public Action OnHeal;
    public Action OnTakeDamage;

    public Action<float> TakeDamage;
    public Func<float> MaxHealth;
    public Func<float> RegenHealth;

    private void Awake()
    {
        TakeDamage = TakeDamageWithoutDefense;
        MaxHealth = () => { return _maxHealth; };
        RegenHealth = () => { return _regenHealth; };
    }

    private void Start()
    {
        curHealth = maxHealth;
        InvokeRepeating("RegenHealthPerSec", 0f, 1.0f);
    }    

    public void TakeDamageWithoutDefense(float damage)
    {
        if (curHealth == 0) return;

        OnTakeDamage?.Invoke();        

        curHealth = Mathf.Max(curHealth - damage, 0);

        if (curHealth == 0)
            OnDie?.Invoke();
    }

    public void TakeHeal(float addHealth)
    {
        OnHeal?.Invoke();
        curHealth = MathF.Min(maxHealth, curHealth + addHealth);
    }

    private void RegenHealthPerSec()
    {
        curHealth = MathF.Min(maxHealth, curHealth + regenHealthPerSec);
    }
}
