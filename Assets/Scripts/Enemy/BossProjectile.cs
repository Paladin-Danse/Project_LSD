using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private EnemyProjectileMovementTransform movement;
    float projectileDistance;
    float damage;
    //public Transform target;
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
        //LookTarget();
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

    //public void LookTarget()
    //{
    //    Vector3 to = new Vector3(target.position.x, 0, target.position.z);
    //    Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
    //    transform.rotation = Quaternion.LookRotation(to - from);
    //}

    private void OnTriggerEnter(Collider other)
    {
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    collision.gameObject.GetComponent<Health>().TakeDamageWithoutDefense(damage);
        //    Debug.Log(damage);
        //    Debug.Log("Player Hit");
        //    Destroy(gameObject);
        //}
        if(other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            ExplosionDamage(transform.position, explosionRadius, layerMask, damage);
            Destroy(gameObject);
            int per = Random.Range(0, 99);
            if(per < 33)
            {
                GameObject B1 = Instantiate(Boom1, transform.position, Quaternion.identity);
                Destroy(B1, 2f);
            }
            else if(per >= 33 && per < 66)
            {
                GameObject B2 = Instantiate(Boom2, transform.position, Quaternion.identity);
                Destroy(B2, 2f);
            }
            else if(per >= 66)
            {
                GameObject B3 = Instantiate(Boom3, transform.position, Quaternion.identity);
                Destroy(B3, 2f);
            }
            
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Health>().TakeDamageWithoutDefense(damage);
            ExplosionDamage(transform.position, explosionRadius, layerMask, damage);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, layerMask);

            foreach (Collider hitCollider in hitColliders)
            {
                // 여기에 데미지를 입히는 코드를 추가하세요.
                Health health = hitCollider.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    // 피해를 입히는 로직을 추가합니다.
                    health.TakeDamage(damage);
                }
            }

            Destroy(gameObject);
            int per = Random.Range(0, 99);
            if (per < 33)
            {
                GameObject B1 = Instantiate(Boom1, transform.position, Quaternion.identity);
                Destroy(B1, 2f);
            }
            else if (per >= 33 && per < 66)
            {
                GameObject B2 = Instantiate(Boom2, transform.position, Quaternion.identity);
                Destroy(B2, 2f);
            }
            else if (per >= 66)
            {
                GameObject B3 = Instantiate(Boom3, transform.position, Quaternion.identity);
                Destroy(B3, 2f);
            }

        }

        Destroy(gameObject, 3f);
    }

    public void InitProjectile(RangedEnemyWeapon weapon)
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
