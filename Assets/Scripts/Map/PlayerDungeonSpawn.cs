using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDungeonSpawn : MonoBehaviour
{
    public Transform playerSpawnPosition;

    PlayerCharacter player;
    private void Awake()
    {
        player = FindObjectOfType<PlayerCharacter>();
    }

    private void Start()
    {
        player.transform.position = playerSpawnPosition.position;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
