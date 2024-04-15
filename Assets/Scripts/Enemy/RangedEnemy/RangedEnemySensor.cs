using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemySensor : MonoBehaviour
{
    public RangedEnemy rangedEnemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rangedEnemy.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rangedEnemy.enabled = false;
        }
    }
}
