using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private int damage;        

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamageWithoutDefense(damage);
        }        
    }

    public void SetAttack(int damage)
    {
        this.damage = damage;        
    }
}
