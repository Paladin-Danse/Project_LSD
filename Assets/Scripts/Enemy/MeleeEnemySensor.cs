using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MeleeEnemySensor : MonoBehaviour
{    
    EnemyStateMachine enemyStateMachine;
    public Enemy meleeEnemy;

    private void Awake()
    {
        meleeEnemy.GetComponent<Rigidbody>().isKinematic = false;
        meleeEnemy.GetComponent<CapsuleCollider>().enabled = true;
    }
    private void Start()
    {        
        Invoke("TriggerReady", 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<Health>(out meleeEnemy.stateMachine.Target))
            {
                meleeEnemy.enabled = true;
                meleeEnemy.GetComponent<Rigidbody>().isKinematic = false;
                meleeEnemy.GetComponent<CapsuleCollider>().enabled = true;
            }                        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<Health>(out meleeEnemy.stateMachine.Target))
            {
                meleeEnemy.enabled = false;
                meleeEnemy.GetComponent<Rigidbody>().isKinematic = true;
                meleeEnemy.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }

    void TriggerReady()
    {
        meleeEnemy.GetComponent<Rigidbody>().isKinematic = true;
        meleeEnemy.GetComponent<CapsuleCollider>().enabled = false;
    }
}
