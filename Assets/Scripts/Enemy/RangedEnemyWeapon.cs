using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyWeapon : EnemyProjectile
{
    public GameObject projectilePrefab;
    public Transform muzzlePos;
    public int projectileSpeed;        
    WaitForSeconds WFS;
    RangedEnemy rangedEnemy;
    
    private void Awake()
    {
        rangedEnemy = GetComponentInParent<RangedEnemy>();        
    }

    private void Start()
    {
        WFS = new WaitForSeconds(rangedEnemy.stateMachine.Enemy.RData.AttackRate);        
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
        yield return WFS;               
    }        
}
