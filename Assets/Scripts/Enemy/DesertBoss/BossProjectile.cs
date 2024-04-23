using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private EnemyProjectileMovementTransform movement;
    float projectileDistance;
    float damage;    
    private ParticleSystem Boom;
    public GameObject Boom1;
    public GameObject Boom2;
    public GameObject Boom3;
    public LayerMask layerMask;
    public float explosionRadius;    
    
    public void Setup(Vector3 position)
    {
        movement = GetComponent<EnemyProjectileMovementTransform>();

        StartCoroutine("OnMove", position);

    }

    private IEnumerator OnMove(Vector3 targetPosition)
    {        
        Vector3 start = transform.position;

        movement.MoveTo((targetPosition - transform.position).normalized);

        while (true)
        {
            if (Vector3.Distance(transform.position, start) >= projectileDistance)
            {
                Destroy(gameObject);

                yield break;
            }

            yield return null;
        }
    }    

    private void OnTriggerEnter(Collider other)
    {        
        if(other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            ExplosionDamage(transform.position, explosionRadius, layerMask, damage);
            Destroy(gameObject);
            int per = Random.Range(0, 99);
            if(per < 33)
            {                                
                GameObject B1 = Instantiate(Boom1, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                ParticleSystem B1_Effect = B1.GetComponent<ParticleSystem>();
                this.Boom = B1_Effect;
                Destroy(B1, 2f);
                
            }
            else if(per >= 33 && per < 66)
            {
                GameObject B2 = Instantiate(Boom2, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                ParticleSystem B2_Effect = B2.GetComponent<ParticleSystem>();
                this.Boom = B2_Effect;                
                Destroy(B2, 2f);
            }
            else if(per >= 66)
            {
                GameObject B3 = Instantiate(Boom3, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                ParticleSystem B3_Effect = B3.GetComponent<ParticleSystem>();
                this.Boom = B3_Effect;                
                Destroy(B3, 2f);
            }
            Boom.Play();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(damage);
            ExplosionDamage(transform.position, explosionRadius, layerMask, damage);            

            Destroy(gameObject);
            int per = Random.Range(0, 99);
            if (per < 33)
            {
                GameObject B1 = Instantiate(Boom1, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                ParticleSystem B1_Effect = B1.GetComponent<ParticleSystem>();
                this.Boom = B1_Effect;
                Destroy(B1, 2f);                
            }
            else if (per >= 33 && per < 66)
            {
                GameObject B2 = Instantiate(Boom2, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                ParticleSystem B2_Effect = B2.GetComponent<ParticleSystem>();
                this.Boom = B2_Effect;                
                Destroy(B2, 2f);
            }
            else if (per >= 66)
            {
                GameObject B3 = Instantiate(Boom3, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                ParticleSystem B3_Effect = B3.GetComponent<ParticleSystem>();
                this.Boom = B3_Effect;                
                Destroy(B3, 2f);
            }
            Boom.Play();
        }
                
        Destroy(gameObject, 3f);
    }

    public void BInitProjectile(DesertBossBigWeapon weapon)
    {
        this.damage = weapon.projectileDamage;
        this.projectileDistance = weapon.projectileDistance;
    }    

    void ExplosionDamage(Vector3 center, float radius, LayerMask layerMask, float damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, layerMask);

        foreach (Collider hitCollider in hitColliders)
        {            
            Health health = hitCollider.gameObject.GetComponent<Health>();
            if (health != null)
            {                
                health.TakeDamage(damage);
                DungeonTracker.Instance.receivedDamage += damage;
            }
        }

    }
}
