using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDungeonSpawn : MonoBehaviour
{
    public Transform dungeonSpawnPosition;

    Player player;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        player.transform.position = dungeonSpawnPosition.position;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
