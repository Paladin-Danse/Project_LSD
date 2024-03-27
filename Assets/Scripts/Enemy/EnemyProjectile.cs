using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{        
    private EnemyProjectileMovementTransform movement;
    float projectileDistance;
    float damage;
    public Transform target;
    
    public void Setup(Vector3 position)
    {
        movement = GetComponent<EnemyProjectileMovementTransform>();
        
        StartCoroutine("OnMove", position);
    }

    private IEnumerator OnMove(Vector3 targetPosition)
    {
        LookTarget();
        Vector3 start = transform.position;
        
        movement.MoveTo((targetPosition-transform.position).normalized);

        while (true)
        {
            if(Vector3.Distance(transform.position, start) >= projectileDistance)
            {
                Destroy(gameObject);

                yield break;
            }

            yield return null;
        }
    }

    public void LookTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamageWithoutDefense(damage);
            Debug.Log(damage);
            Debug.Log("Player Hit");
            Destroy(gameObject);
        }
    }
    
    public void InitProjectile(RangedEnemyWeapon weapon)
    {
        this.damage = weapon.projectileDamage;
        this.projectileDistance = weapon.projectileDistance;
    }
}
