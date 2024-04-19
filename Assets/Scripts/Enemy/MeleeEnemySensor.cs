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
            enemyStateMachine.Target = other.gameObject.GetComponent<Health>();
            meleeEnemy.enabled = true;            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            enemyStateMachine.Target = null;
            meleeEnemy.enabled = false;
        }
    }
}
