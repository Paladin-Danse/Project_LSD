using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField]
    private GameObject poolingObjectPrefab;

    private Queue<TestBullet> poolingObjectQueue = new Queue<TestBullet>();

    private void Awake()
    {
        instance = this;

        Initialize(10);
    }

    TestBullet CreatNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab, transform).GetComponent<TestBullet>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private void Initialize(int count)
    {
        for(int i = 0; i < count; i++)
        {
            poolingObjectQueue.Enqueue(CreatNewObject());
        }        
    }

    public static TestBullet GetObject()
    {
        if(instance.poolingObjectQueue.Count > 0)
        {
            var obj = instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreatNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public static void ReturnObject(TestBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(instance.transform);
        instance.poolingObjectQueue.Enqueue(bullet);
    }
}
