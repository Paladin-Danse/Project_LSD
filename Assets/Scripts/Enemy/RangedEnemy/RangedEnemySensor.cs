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
            rangedEnemyStateMachine.Target = other.gameObject.GetComponent<Health>();
            rangedEnemy.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rangedEnemyStateMachine.Target = null;
            rangedEnemy.enabled = false;
        }
    }
}
