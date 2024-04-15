using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyWeapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform muzzlePos;
    [HideInInspector] public float projectileSpeed;
    [HideInInspector] public float projectileDamage;
    [HideInInspector] public float projectileDistance;
    WaitForSeconds WFS;
    RangedEnemy rangedEnemy;
    
    private void Awake()
    {
        rangedEnemy = GetComponent<RangedEnemy>();
        projectileSpeed = rangedEnemy.WSData.weaponStat.attackStat.bulletSpeed;
        projectileDamage = rangedEnemy.WSData.weaponStat.attackStat.damage;
        projectileDistance = rangedEnemy.WSData.weaponStat.attackStat.range;
    }

    private void Start()
    {
        WFS = YieldCacher.WaitForSeconds(rangedEnemy.stateMachine.Enemy.RData.AttackRate);
    }    
    public void StartShot()
    {
        StartCoroutine("Shot");
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
}
