using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BigFire : MonoBehaviour
{
    float damage = 5f;
    float damageRate = 1f;
    private float currentDamageRate;
    private float durationTime = 5f;
    private float currentDurationTime;
    private bool isFire = true;
    public GameObject mediumFire;
    ParticleSystem Mfire;

    private void Start()
    {
        currentDurationTime = durationTime;
        Invoke("MFire", 4.9f);
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
                other.gameObject.GetComponent<Health>().TakeDamageWithoutDefense(damage);
                currentDamageRate = damageRate;
            }                                    
        }        
    }    

    void MFire()
    {
        GameObject MF = Instantiate(mediumFire, transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
        ParticleSystem f_Effect = MF.GetComponent<ParticleSystem>();
        this.Mfire = f_Effect;
        Destroy(MF, 3f);

        Mfire.Play();
    }
}
