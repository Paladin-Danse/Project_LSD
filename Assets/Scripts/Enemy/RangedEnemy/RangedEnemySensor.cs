using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemySensor : MonoBehaviour
{
    public RangedEnemy rangedEnemy;
    RangedEnemyStateMachine rangedEnemyStateMachine;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<Health>(out rangedEnemy.stateMachine.Target))
            {
                rangedEnemy.enabled = true;
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
            }
        }
    }
}
