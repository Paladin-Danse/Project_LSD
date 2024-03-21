using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoProjectile : MonoBehaviour
{
    Rigidbody rigidbody_;
    [SerializeField] private Vector3 EnablePos;
    private float ProjectileVelocity;
    [SerializeField] private float ProjectileSweepCheckModifier = 0.1f;
    private float ProjectileMaxDistance;
    private float ProjectileDamage;

    RaycastHit hit;
    public LayerMask TargetLayer;

    private void Awake()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }

    public void OnInit(Weapon curWeapon)
    {
        //탄 투사체가 생성되거나 SetActive가 True가 되었을 때 무기 클래스를 매개변수로 받아와 탄속을 저장.
        //현재 Weapon에서 currentStat을 받아오는 작업이 진행되고 있지 않다. 초기화를 해주었음에도 초기화가 안 된 모습이다.
        EnablePos = transform.position;
        ProjectileVelocity = curWeapon.currentStat.attackStat.bulletSpeed;
        ProjectileMaxDistance = curWeapon.currentStat.attackStat.range;
        ProjectileDamage = curWeapon.currentStat.attackStat.damage;
        gameObject.SetActive(true);
        Move();
    }

    void Update()
    {
    }
    private void FixedUpdate()
    {
        CollisionCheck();
        DisableProjectile();
    }

    private void Move()
    {
        rigidbody_.velocity = transform.forward * ProjectileVelocity;
    }
    private void CollisionCheck()
    {
        if(rigidbody_.SweepTest(transform.forward, out hit, ProjectileVelocity * ProjectileSweepCheckModifier))
        {
            int objectLayer = 1 << hit.transform.gameObject.layer;
            
            if ((TargetLayer & objectLayer) != 0)
            {
                if (hit.transform.TryGetComponent<Health>(out Health hit_Object))
                {
                    Debug.Log("Target Hit & Damaged!!");
                    hit_Object.TakeDamage(ProjectileDamage);
                }
                else
                {
                    Debug.Log("Target Hit!!");
                }
            }
            rigidbody_.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
    
    private void DisableProjectile()
    {
        if (Vector3.Distance(transform.position, EnablePos) >= ProjectileMaxDistance)
        {
            rigidbody_.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

}
