using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmallProjectile : MonoBehaviour
{
    private EnemyProjectileMovementTransform movement;
    float projectileDistance;
    public float damage;        
    public GameObject firePrefab;        
    public LayerMask layerMask;
    public float explosionRadius;
    private GameObject FireInstance;        

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

    //bool check = false;
    private void OnTriggerEnter(Collider other)
    {
        //if(check)
        //{
        //    return;
        //}
        //check = true;        
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {

            FireInstance = Instantiate(firePrefab, transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
            //CFire();

            Destroy(gameObject, 2f);

        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(damage);
            //DungeonManager.Instance.receivedDamage += damage;

            Destroy(gameObject, 2f);
        }
        //else
        //{
        //    FireInstance = Instantiate(firePrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
        //    Destroy(gameObject, 2f);
        //}

        Destroy(gameObject, 3f);
    }    

    public void SInitProjectile(DesertBossSmallWeapon weapon)
    {
        this.damage = weapon.projectileDamage;
        this.projectileDistance = weapon.projectileDistance;
        this.gameObject.SetActive(true);
    }    
}
