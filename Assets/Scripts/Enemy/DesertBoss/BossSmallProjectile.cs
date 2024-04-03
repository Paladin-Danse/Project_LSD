using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmallProjectile : MonoBehaviour
{
    private EnemyProjectileMovementTransform movement;
    float projectileDistance;
    public float damage;    
    private ParticleSystem Fire;
    public GameObject Fire1;    
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {                        
            GameObject B1 = Instantiate(Fire1, transform.position, Quaternion.identity);
            ParticleSystem B1_Effect = B1.GetComponent<ParticleSystem>();
            this.Fire = B1_Effect;
            Destroy(B1, 5f);

            Fire.Play();

            Destroy(gameObject, 6f);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Health>().TakeDamageWithoutDefense(damage);                        

            GameObject B1 = Instantiate(Fire1, transform.position, Quaternion.identity);
            ParticleSystem B1_Effect = B1.GetComponent<ParticleSystem>();
            this.Fire = B1_Effect;
            Destroy(B1, 5f);

            Fire.Play();

            Destroy(gameObject, 6f);
        }

        Destroy(gameObject, 3f);
    }    

    public void SInitProjectile(DesertBossSmallWeapon weapon)
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
                health.TakeDamageWithoutDefense(damage);
            }
        }

    }
}
