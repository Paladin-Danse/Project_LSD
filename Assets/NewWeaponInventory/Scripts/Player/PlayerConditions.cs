using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ����� ó���� ���� �������̽�
public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

// ����Ƽ���� ������ �� �ֵ��� ����ȭ
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

    // ����
    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    // �E��
    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }

}


// IDamagable ����ϱ� ���ؼ� ���
public class PlayerConditions : MonoBehaviour, IDamagable
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    public float noHungerHealthDecay;

    public UnityEvent onTakeDamage; // ������� ������ �޾ƿ� �̺�Ʈ

    void Start()
    {
        health.curValue = health.startValue;
        hunger.curValue = hunger.startValue;
        stamina.curValue = stamina.startValue;
    }

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.decayRate * Time.deltaTime); // ������ �Ҹ�
        stamina.Add(stamina.regenRate * Time.deltaTime); // ���׹̳� ȸ��

        if (hunger.curValue == 0.0f) // ������ 0�Ͻ� ü�¼Ҹ�
            health.Subtract(noHungerHealthDecay * Time.deltaTime);

        if (health.curValue == 0.0f) // ü�� 0�Ͻ� ����
            Die();

        // uiBar�� fillAmount ���� ������ ������ ǥ��
        health.uiBar.fillAmount = health.GetPercentage();
        hunger.uiBar.fillAmount = hunger.GetPercentage();
        stamina.uiBar.fillAmount = stamina.GetPercentage();
    }

    // ü�� ȸ��
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // ������ ȸ��
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    // ���׹̳� �Ҹ�
    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
            return false;

        stamina.Subtract(amount);
        return true;
    }

    // ����
    public void Die()
    {
        Debug.Log("�÷��̾ �׾���.");
    }

    // ����� ó��
    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}