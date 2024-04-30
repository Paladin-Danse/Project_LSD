using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemySensor : MonoBehaviour
{
    public RangedEnemy rangedEnemy;
    RangedEnemyStateMachine rangedEnemyStateMachine;
    private void Start()
    {
        rangedEnemy.GetComponent<Rigidbody>().isKinematic = false;
        rangedEnemy.GetComponent<CapsuleCollider>().enabled = true;
        Invoke("TriggerReady", 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<Health>(out rangedEnemy.stateMachine.Target))
            {
                rangedEnemy.enabled = true;
                rangedEnemy.GetComponent<Rigidbody>().isKinematic = false;
                rangedEnemy.GetComponent<CapsuleCollider>().enabled = true;
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<Health>(out rangedEnemy.stateMachine.Target))
            {
                rangedEnemy.enabled = false;
                rangedEnemy.GetComponent<Rigidbody>().isKinematic = true;
                rangedEnemy.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }

    void TriggerReady()
    {
        rangedEnemy.GetComponent<Rigidbody>().isKinematic = true;
        rangedEnemy.GetComponent<CapsuleCollider>().enabled = false;
    }
}
