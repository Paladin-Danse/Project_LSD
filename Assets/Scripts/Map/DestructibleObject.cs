using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private GameObject destrutedBox;
    [SerializeField] private GameObject bulletBox;
    [SerializeField] private int maxHP = 100;
    private int currentHP;

    bool isDestroyed = false;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if(currentHP <= 0 && isDestroyed == false)
        {
            isDestroyed = true;

            GameObject destroyBox = Instantiate(destrutedBox, transform.position, transform.rotation);

            Destroy(gameObject);

            Instantiate(bulletBox, transform.position, transform.rotation);

            Destroy(destroyBox, 3f);
        }
    }
}
