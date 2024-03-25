using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDungeonSpawn : MonoBehaviour
{
    public Transform dungeonSpawnPosition;

    PlayerCharacter player;
    private void Awake()
    {
        player = FindObjectOfType<PlayerCharacter>();
    }

    private void Start()
    {
        player.transform.position = dungeonSpawnPosition.position;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
