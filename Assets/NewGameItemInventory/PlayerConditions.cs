using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 대미지 처리를 위한 인터페이스
public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

// 유니티에서 접근할 수 있도록 직렬화
[System.Serializable]
public class Condition
{
    [HideInInspector]
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public float decayRate;
    public Image uiBar;

    // 덧셈
    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    // 뺼셈
    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }

}


// IDamagable 사용하기 위해서 상속
public class PlayerConditions : MonoBehaviour, IDamagable
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    public float noHungerHealthDecay;

    public UnityEvent onTakeDamage; // 대미지를 받을때 받아올 이벤트

    void Start()
    {
        health.curValue = health.startValue;
        hunger.curValue = hunger.startValue;
        stamina.curValue = stamina.startValue;
    }

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.decayRate * Time.deltaTime); // 포만감 소모
        stamina.Add(stamina.regenRate * Time.deltaTime); // 스테미나 회복

        if (hunger.curValue == 0.0f) // 포만감 0일시 체력소모
            health.Subtract(noHungerHealthDecay * Time.deltaTime);

        if (health.curValue == 0.0f) // 체력 0일시 죽음
            Die();

        // uiBar에 fillAmount 값을 조절해 게이지 표현
        health.uiBar.fillAmount = health.GetPercentage();
        hunger.uiBar.fillAmount = hunger.GetPercentage();
        stamina.uiBar.fillAmount = stamina.GetPercentage();
    }

    // 체력 회복
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // 포만감 회복
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    // 스테미나 소모
    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
            return false;

        stamina.Subtract(amount);
        return true;
    }

    // 죽음
    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }

    // 대미지 처리
    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}