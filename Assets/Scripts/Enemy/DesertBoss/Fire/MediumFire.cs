using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumFire : MonoBehaviour
{
    float damage = 3f;
    float damageRate = 1f;
    private float currentDamageRate;
    private float durationTime = 3f;
    private float currentDurationTime;
    private bool isFire = true;
    public GameObject smallFire;
    ParticleSystem Sfire;

    private void Start()
    {
        currentDurationTime = durationTime;
        Invoke("SFire", 2.9f);
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

    void SFire()
    {
        GameObject SF = Instantiate(smallFire, transform.position + new Vector3(0, 0.15f, 0), Quaternion.identity);
        ParticleSystem f_Effect = SF.GetComponent<ParticleSystem>();
        this.Sfire = f_Effect;
        Destroy(SF, 2f);

        Sfire.Play();
    }
}
