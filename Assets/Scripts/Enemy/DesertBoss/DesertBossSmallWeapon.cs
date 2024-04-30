using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossSmallWeapon : MonoBehaviour
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
        WFS = YieldCacher.WaitForSeconds(desertBoss.stateMachine.Enemy.RData.AttackRate);
    }
    public void SFirstShot()
    {
        StartCoroutine("FShot");
    }

    public void SSecondShot()
    {
        StartCoroutine("SShot");
    }

    IEnumerator FShot()
    {
        BossSmallProjectile instantProjectile1 = ObjectPoolManager.Instance.Pop(projectilePrefab).GetComponent<BossSmallProjectile>();
        instantProjectile1.transform.position = muzzlePos1.position;
        instantProjectile1.transform.forward = muzzlePos1.forward;

        BossSmallProjectile instantProjectile2 = ObjectPoolManager.Instance.Pop(projectilePrefab).GetComponent<BossSmallProjectile>();
        instantProjectile2.transform.position = muzzlePos2.position;
        instantProjectile2.transform.forward = muzzlePos2.forward;

        Rigidbody projectileRigid1 = instantProjectile1.GetComponent<Rigidbody>();
        Rigidbody projectileRigid2 = instantProjectile2.GetComponent<Rigidbody>();

        projectileRigid1.velocity = muzzlePos1.forward * projectileSpeed;
        projectileRigid2.velocity = muzzlePos2.forward * projectileSpeed;

        desertBoss.SProjectile1 = instantProjectile1.GetComponent<BossSmallProjectile>();
        desertBoss.SProjectile2 = instantProjectile2.GetComponent<BossSmallProjectile>();

        desertBoss.SProjectile1.SInitProjectile(this);
        desertBoss.SProjectile2.SInitProjectile(this);
        yield return WFS;
    }

    IEnumerator SShot()
    {
        BossSmallProjectile instantProjectile3 = ObjectPoolManager.Instance.Pop(projectilePrefab).GetComponent<BossSmallProjectile>();
        instantProjectile3.transform.position = muzzlePos3.position;
        instantProjectile3.transform.forward = muzzlePos3.forward;

        BossSmallProjectile instantProjectile4 = ObjectPoolManager.Instance.Pop(projectilePrefab).GetComponent<BossSmallProjectile>();
        instantProjectile4.transform.position = muzzlePos4.position;
        instantProjectile4.transform.forward = muzzlePos4.forward;

        Rigidbody projectileRigid3 = instantProjectile3.GetComponent<Rigidbody>();
        Rigidbody projectileRigid4 = instantProjectile4.GetComponent<Rigidbody>();

        projectileRigid3.velocity = muzzlePos3.forward * projectileSpeed;
        projectileRigid4.velocity = muzzlePos4.forward * projectileSpeed;

        desertBoss.SProjectile3 = instantProjectile3.GetComponent<BossSmallProjectile>();
        desertBoss.SProjectile4 = instantProjectile4.GetComponent<BossSmallProjectile>();

        desertBoss.SProjectile3.SInitProjectile(this);
        desertBoss.SProjectile4.SInitProjectile(this);
        yield return WFS;
    }
}
