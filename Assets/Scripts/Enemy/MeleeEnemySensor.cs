using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MeleeEnemySensor : MonoBehaviour
{    
    EnemyStateMachine enemyStateMachine;
    public Enemy meleeEnemy;    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<Health>(out meleeEnemy.stateMachine.Target))
            {
                meleeEnemy.enabled = true;
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
            }
        }
    }
}
