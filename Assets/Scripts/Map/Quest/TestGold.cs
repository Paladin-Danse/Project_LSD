using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGold : MonoBehaviour
{
    public static int plusGold;
    public static int goldCount = 1;

    private void Start()
    {
        plusGold = Random.Range(1, 200);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //playerGold += plusGold;
            Destroy(other.gameObject);
        }
    }
}
