using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGold : MonoBehaviour
{
    int gold;
    private void Awake()
    {
        gold = Random.Range(10, 200);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //PlayerGold += gold;
            DungeonManager.Instance.earnGold += gold;
            Destroy(gameObject);
        }
    }
}
