using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesetBossBigWeapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform muzzlePos1;
    public Transform muzzlePos2;
    public Transform muzzlePos3;
    public Transform muzzlePos4;
    [HideInInspector] public float projectileSpeed;
    [HideInInspector] public float projectileDamage;
    [HideInInspector] public float projectileDistance;
    WaitForSeconds WFS;
    DesertBoss desertBoss;

    private void Awake()
    {
        desertBoss = GetComponent<DesertBoss>();
        projectileSpeed = desertBoss.WSData.weaponStat.attackStat.bulletSpeed;
        projectileDamage = desertBoss.WSData.weaponStat.attackStat.damage;
        projectileDistance = desertBoss.WSData.weaponStat.attackStat.range;
    }

    private void Start()
    {
        WFS = new WaitForSeconds(desertBoss.stateMachine.Enemy.RData.AttackRate);
    }
    public void FirstShot()
    {
        StartCoroutine("FShot");
    }

    public void SecondShot()
    {
        StartCoroutine("SShot");
    }

    IEnumerator FShot()
    {
        GameObject instantProjectile1 = Instantiate(projectilePrefab, muzzlePos1.position, muzzlePos1.rotation);
        GameObject instantProjectile2 = Instantiate(projectilePrefab, muzzlePos2.position, muzzlePos2.rotation);
        
        Rigidbody projectileRigid1 = instantProjectile1.GetComponent<Rigidbody>();
        Rigidbody projectileRigid2 = instantProjectile2.GetComponent<Rigidbody>();

        projectileRigid1.velocity = muzzlePos1.forward * projectileSpeed;
        projectileRigid2.velocity = muzzlePos2.forward * projectileSpeed;

        desertBoss.Projectile = instantProjectile1.GetComponent<EnemyProjectile>();
        desertBoss.Projectile = instantProjectile2.GetComponent<EnemyProjectile>();
        
        //desertBoss.Projectile.InitProjectile(this);
        yield return WFS;
    }

    IEnumerator SShot()
    {        
        GameObject instantProjectile3 = Instantiate(projectilePrefab, muzzlePos3.position, muzzlePos3.rotation);
        GameObject instantProjectile4 = Instantiate(projectilePrefab, muzzlePos4.position, muzzlePos4.rotation);
        
        Rigidbody projectileRigid3 = instantProjectile3.GetComponent<Rigidbody>();
        Rigidbody projectileRigid4 = instantProjectile4.GetComponent<Rigidbody>();

        projectileRigid3.velocity = muzzlePos3.forward * projectileSpeed;
        projectileRigid4.velocity = muzzlePos4.forward * projectileSpeed;

        desertBoss.Projectile = instantProjectile3.GetComponent<EnemyProjectile>();
        desertBoss.Projectile = instantProjectile4.GetComponent<EnemyProjectile>();
        //desertBoss.Projectile.InitProjectile(this);
        yield return WFS;
    }
}
