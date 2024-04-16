using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFire : MonoBehaviour
{
    float damage = 2f;
    float damageRate = 1f;
    private float currentDamageRate;
    private float durationTime = 2f;
    private float currentDurationTime;
    private bool isFire = true;

    private void Start()
    {
        currentDurationTime = durationTime;        
    }

    void Update()
    {
        if (isFire)
        {
            ElapseTime();
        }
    }

    private void ElapseTime()
    {
        currentDurationTime -= Time.deltaTime;

        if (currentDurationTime <= 0)
            isFire = false;


        if (currentDamageRate > 0)
            currentDamageRate -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (currentDamageRate <= 0)
            {
                other.gameObject.GetComponent<Health>().TakeDamage(damage);
                currentDamageRate = damageRate;
            }
        }
    }
}
