using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    float projectileDistance;
    float damage;
    private ParticleSystem Boom;
    public GameObject Boom1;
    public GameObject Boom2;
    public GameObject Boom3;
    public GameObject Boom4;
    public LayerMask layerMask;
    public float explosionRadius;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            ExplosionDamage(transform.position, explosionRadius, layerMask, damage);
            Destroy(gameObject);
            int per = Random.Range(0, 99);
            if (per < 33)
            {
                GameObject B1 = Instantiate(Boom1, transform.position, Quaternion.identity);
                ParticleSystem B1_Effect = B1.GetComponent<ParticleSystem>();
                this.Boom = B1_Effect;
                Destroy(B1, 2f);

            }
            else if (per >= 33 && per < 66)
            {
                GameObject B2 = Instantiate(Boom2, transform.position, Quaternion.identity);
                ParticleSystem B2_Effect = B2.GetComponent<ParticleSystem>();
                this.Boom = B2_Effect;
                Destroy(B2, 2f);
            }
            else if (per >= 66)
            {
                GameObject B3 = Instantiate(Boom3, transform.position, Quaternion.identity);
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
            if (per < 25)
            {
                GameObject B1 = Instantiate(Boom1, transform.position + new Vector3(-2.5f, 0, -2.5f), Quaternion.identity);
                ParticleSystem B1_Effect = B1.GetComponent<ParticleSystem>();
                this.Boom = B1_Effect;
                Destroy(B1, 2f);
            }
            else if (per >= 25 && per < 50)
            {
                GameObject B2 = Instantiate(Boom2, transform.position + new Vector3(-2.5f, 0, -2.5f), Quaternion.identity);
                ParticleSystem B2_Effect = B2.GetComponent<ParticleSystem>();
                this.Boom = B2_Effect;
                Destroy(B2, 2f);
            }
            else if (per >= 50 && per < 75)
            {
                GameObject B3 = Instantiate(Boom3, transform.position + new Vector3(-2.5f, 0, -2.5f), Quaternion.identity);
                ParticleSystem B3_Effect = B3.GetComponent<ParticleSystem>();
                this.Boom = B3_Effect;
                Destroy(B3, 2f);
            }
            else if (per >= 75)
            {
                GameObject B4 = Instantiate(Boom4, transform.position + new Vector3(-2.5f, 0, -2.5f), Quaternion.identity);
                ParticleSystem B4_Effect = B4.GetComponent<ParticleSystem>();
                this.Boom = B4_Effect;
                Destroy(B4, 2f);
            }
            Boom.Play();
        }

        Destroy(gameObject, 3f);
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
                Debug.Log("ÅÍ·¿ Æø¹ß µ¥¹ÌÁö" + damage);
                //DungeonManager.Instance.receivedDamage += damage;
            }
        }

    }

    public void TInitProjectile(Turret weapon)
    {
        this.damage = weapon.t_ProjectileDamage;        
    }
}
