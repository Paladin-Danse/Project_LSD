using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSensor : MonoBehaviour
{
    public Turret turret;
    private void Start()
    {
        turret.GetComponent<Rigidbody>().isKinematic = false;
        turret.GetComponent<CapsuleCollider>().enabled = true;
        Invoke("TriggerReady", 5f);
    }

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

    void TriggerReady()
    {
        turret.GetComponent<Rigidbody>().isKinematic = true;
        turret.GetComponent<CapsuleCollider>().enabled = false;
    }
}
