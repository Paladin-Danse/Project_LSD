using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOnSensor : MonoBehaviour
{
    public GameObject shield;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            shield.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            shield.SetActive(false);
        }
    }
}
