using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSensor : MonoBehaviour
{
    public Turret turret;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<Health>(out turret._Target))
            {
                turret.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<Health>(out turret._Target))
            {
                turret.enabled = false;
            }
        }
    }
}
