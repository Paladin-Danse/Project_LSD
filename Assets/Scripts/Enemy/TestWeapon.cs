using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private int damage;
    private float knockback;

    //private List<Collider> alreadyColliderWith = new List<Collider>();

    //private void OnEnable()
    //{
    //    alreadyColliderWith.Clear();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other == myCollider) return;
    //    if (alreadyColliderWith.Contains(other)) return;

    //    alreadyColliderWith.Add(other);

    //    if (other.TryGetComponent(out TestHealth health))
    //    {
    //        health.TakeDamage(damage);
    //    }

    //    //if (other.TryGetComponent(out ForceReceiver forceReceiver))
    //    //{
    //    //    Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
    //    //    forceReceiver.AddForce(direction * knockback);
    //    //}


    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<TestHealth>().TakeDamage(damage);
        }        
    }

    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
