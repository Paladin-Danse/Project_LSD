using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossAccessSensor : MonoBehaviour
{
    DesertBossStateMachine desertBossStateMachine;
    public DesertBoss desertBoss;
    private void Start()
    {
        desertBoss.GetComponent<Rigidbody>().isKinematic = false;
        desertBoss.GetComponent<CapsuleCollider>().enabled = true;
        Invoke("TriggerReady", 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<Health>(out desertBoss.stateMachine.Target))
            {
                desertBoss.enabled = true;
                desertBoss.GetComponent<Rigidbody>().isKinematic = false;
                desertBoss.GetComponent<CapsuleCollider>().enabled = true;

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
                desertBoss.GetComponent<Rigidbody>().isKinematic = true;
                desertBoss.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }

    void TriggerReady()
    {
        desertBoss.GetComponent<Rigidbody>().isKinematic = true;
        desertBoss.GetComponent<CapsuleCollider>().enabled = false;
    }
}
