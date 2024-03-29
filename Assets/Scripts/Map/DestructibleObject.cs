using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private GameObject destrutedBox;
    [SerializeField] private GameObject bulletBox;
    [SerializeField] private GameObject firstAidKit;
    Health health;   

    bool isDestroyed = false;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.OnDie += DestroyBox;
    }
    
    void DestroyBox()
    {
        int per = Random.Range(0, 99);
        isDestroyed = true;

        GameObject destroyBox = Instantiate(destrutedBox, transform.position, transform.rotation);

        Destroy(gameObject);
        if(per >= 50)
        {
            Instantiate(bulletBox, transform.position, transform.rotation);
        }
        else if(per < 50)
        {
            Instantiate(firstAidKit, transform.position, transform.rotation);
        }        

        Destroy(destroyBox, 3f);
    }
}
