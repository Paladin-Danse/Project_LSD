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

    [Header("Effect")]
    public ParticleSystem hitEffect;

    private void Awake()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }

    public void OnInit(Weapon curWeapon)
    {
        //ź ����ü�� �����ǰų� SetActive�� True�� �Ǿ��� �� ���� Ŭ������ �Ű������� �޾ƿ� ź���� ����.
        //���� Weapon���� currentStat�� �޾ƿ��� �۾��� ����ǰ� ���� �ʴ�. �ʱ�ȭ�� ���־������� �ʱ�ȭ�� �� �� ����̴�.
        EnablePos = transform.position;
        ProjectileVelocity = curWeapon.curWeaponStat.attackStat.bulletSpeed;
        ProjectileMaxDistance = curWeapon.curWeaponStat.attackStat.range;
        ProjectileDamage = curWeapon.curWeaponStat.attackStat.damage;
        gameObject.SetActive(true);
        Move();
    }

    private void FixedUpdate()
    {
        CollisionCheck();
        DisableProjectile();
    }

    private void Move()
    {
        rigidbody_.velocity = transform.TransformDirection(Vector3.forward) * ProjectileVelocity;
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * ProjectileMaxDistance, Color.blue, 2.0f);
    }

    private void CollisionCheck()
    {
        if (rigidbody_.SweepTest(transform.TransformDirection(Vector3.forward), out hit, ProjectileVelocity * ProjectileSweepCheckModifier))
        {
            int objectLayer = 1 << hit.transform.gameObject.layer;

            if ((TargetLayer & objectLayer) != 0)
            {
                if (hit.transform.TryGetComponent<Health>(out Health hit_Object))
                {
                    //Debug.Log("Target Hit & Damaged!!");
                    hit_Object.TakeDamage(ProjectileDamage);
                    DungeonTracker.Instance.totalDamage += ProjectileDamage;
                }
                else
                {
                    //Debug.Log("Target Hit!!");
                }
            }
            ParticleSystem hitParticle = ObjectPoolManager.Instance.Pop(hitEffect.gameObject).GetComponent<ParticleSystem>();
            hitParticle.transform.position = hit.point;
            hitParticle.gameObject.SetActive(true);
            hitParticle.Play();
            
            rigidbody_.velocity = Vector3.zero;
            DestroyProjectile();
        }
    }
    
    private void DisableProjectile()
    {
        if (Vector3.Distance(transform.position, EnablePos) >= ProjectileMaxDistance)
        {
            DestroyProjectile();
        }
    }
    
    private void DestroyProjectile()
    {
        ObjectPoolManager.Instance.TryPush(this.gameObject);
    }

}
