using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class RangedEnemyWeapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform muzzlePos;
    [HideInInspector] public float projectileSpeed;
    [HideInInspector] public float projectileDamage;
    [HideInInspector] public float projectileDistance;
    WaitForSeconds WFS;
    RangedEnemy rangedEnemy;
    public LayerMask layerMask;
    float attackRange;
    private Transform target;    

    private void Awake()
    {
        rangedEnemy = GetComponent<RangedEnemy>();
        projectileSpeed = rangedEnemy.WSData.weaponStat.attackStat.bulletSpeed;
        projectileDamage = rangedEnemy.WSData.weaponStat.attackStat.damage;
        projectileDistance = rangedEnemy.WSData.weaponStat.attackStat.range;
        attackRange = rangedEnemy.RData.AttackRange;
    }

    private void Start()
    {
        WFS = YieldCacher.WaitForSeconds(rangedEnemy.stateMachine.Enemy.RData.AttackRate);
        InvokeRepeating("UpdateTarget", 0, 0.25f);
    }    
    public void StartShot()
    {
        StartCoroutine("Shot");
    }

    private void Update()
    {
        if (target != null)
        {
            muzzlePos.transform.LookAt(target, muzzlePos.transform.forward);            
        }

    }    

    IEnumerator Shot()
    {        
        GameObject instantProjectile = Instantiate(projectilePrefab, muzzlePos.position, muzzlePos.rotation);
        Rigidbody projectileRigid = instantProjectile.GetComponent<Rigidbody>();
        projectileRigid.velocity = muzzlePos.forward * projectileSpeed;        
        rangedEnemy.Projectile = instantProjectile.GetComponent<EnemyProjectile>();
        rangedEnemy.Projectile.InitProjectile(this);
        yield return WFS;               
    }

    void UpdateTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, attackRange, layerMask);

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    target = cols[i].gameObject.transform;
                }
            }
        }
        else
        {
            target = null;
        }
    }
}
