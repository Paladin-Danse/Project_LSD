using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private GameObject destrutedBox;
    [SerializeField] private GameObject bulletBox;    
    Health health;   

    bool isDestroyed = false;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.OnDie += DestroyBox;
    }
    
    void DestroyBox()
    {
        isDestroyed = true;

        GameObject destroyBox = Instantiate(destrutedBox, transform.position, transform.rotation);

        Destroy(gameObject);

        Instantiate(bulletBox, transform.position, transform.rotation);

        Destroy(destroyBox, 3f);
    }
}
