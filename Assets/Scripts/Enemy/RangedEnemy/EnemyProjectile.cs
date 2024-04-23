using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{            
    float projectileDistance;
    float damage;              

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            //DungeonManager.Instance.receivedDamage += damage;
            Debug.Log("Ranged - Player Hit" + damage);
            Destroy(gameObject);
        }

        Destroy(gameObject, 3f);
    }
    
    public void InitProjectile(RangedEnemyWeapon weapon)
    {
        this.damage = weapon.projectileDamage;
        this.projectileDistance = weapon.projectileDistance;
        this.gameObject.SetActive(true);
    }
}
