using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    public int damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("InteractableObject"))
        {
            collision.gameObject.GetComponent<DestructibleObject>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private Vector3 direction;

    public void Shoot(Vector3 dir)
    {
        direction = dir;
        Invoke("DestroyBullet", 5f);
    }

    private void DestroyBullet()
    {
        ObjectPool.ReturnObject(this);
    }

    private void Update()
    {
        transform.Translate(direction);
    }
}
