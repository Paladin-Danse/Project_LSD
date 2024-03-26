using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyWeapon : EnemyProjectile
{
    public GameObject projectilePrefab;
    public Transform muzzlePos;
    public int projectileSpeed;    
    public Transform target;
    WaitForSeconds WFS;
    RangedEnemy rangedEnemy;
    private void Awake()
    {
        rangedEnemy = GetComponentInParent<RangedEnemy>();        
    }

    private void Start()
    {
        WFS = new WaitForSeconds(rangedEnemy.stateMachine.Enemy.Data.AttackRate);        
    }    
    public void StartShot()
    {
        StartCoroutine("Shot");
    }    

    IEnumerator Shot()
    {
        LookTarget();
        GameObject instantProjectile = Instantiate(projectilePrefab, muzzlePos.position, muzzlePos.rotation);
        Rigidbody projectileRigid = instantProjectile.GetComponent<Rigidbody>();
        projectileRigid.velocity = muzzlePos.forward * projectileSpeed;
        yield return WFS;               
    }    

    void LookTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);
    }
}
