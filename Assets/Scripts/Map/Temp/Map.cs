using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [field:SerializeField]
    public PlayerSpawnPoint spawnPoint { get; private set; }

    // For Dungeon Map
    // [field: SerializeField]
    // public GameObject clearObject { get; private set; }


    private void Awake()
    {
        FindSpawnPoint();
    }

    void FindSpawnPoint() 
    {
        if (spawnPoint == null)
        {
            spawnPoint = GetComponentInChildren<PlayerSpawnPoint>();
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Map에 PlayerSpawnPoint가 없습니다!");
        }
    }
}
