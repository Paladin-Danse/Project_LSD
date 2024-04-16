using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossAccessSensor : MonoBehaviour
{
    public DesertBoss desertBoss;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            desertBoss.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            desertBoss.enabled = false;
        }
    }
}
