using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }

    public Vector3 GetSpawnPosition()
    {
        return transform.position;
    }
}
