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
            if (other.TryGetComponent<Health>(out desertBoss.stateMachine.Target))
            {
                desertBoss.enabled = true;
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<Health>(out desertBoss.stateMachine.Target))
            {
                desertBoss.enabled = false;
            }
        }
    }
}
