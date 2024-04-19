using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossAccessSensor : MonoBehaviour
{
    DesertBossStateMachine desertBossStateMachine;
    public DesertBoss desertBoss;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            desertBossStateMachine.Target = other.gameObject.GetComponent<Health>();
            desertBoss.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            desertBossStateMachine.Target = null;
            desertBoss.enabled = false;
        }
    }
}
