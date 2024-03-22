using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{        
    private EnemyProjectileMovementTransform movement;
    float projectileDistance = 25f;
    [HideInInspector] public int damage;

    public void Setup(Vector3 position)
    {
        movement = GetComponent<EnemyProjectileMovementTransform>();

        StartCoroutine("OnMove", position);
    }

    private IEnumerator OnMove(Vector3 targetPosition)
    {
        Vector3 start =transform.position;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamageWithoutDefense(damage);
            Debug.Log("Player Hit");
            Destroy(gameObject);
        }
    }

    public void SetAttack(int damage)
    {
        this.damage = damage;
    }
}
