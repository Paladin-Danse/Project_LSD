using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoProjectile : MonoBehaviour
{
    Rigidbody rigidbody_;
    private float ProjectileVelocity;
    [SerializeField] private Vector3 EnablePos;
    private float ProjectileMaxDistance;
    RaycastHit hit;

    private void Awake()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }

    public void OnInit(Weapon curWeapon)
    {
        //ź ����ü�� �����ǰų� SetActive�� True�� �Ǿ��� �� ���� Ŭ������ �Ű������� �޾ƿ� ź���� ����.
        //���� Weapon���� currentStat�� �޾ƿ��� �۾��� ����ǰ� ���� �ʴ�. �ʱ�ȭ�� ���־������� �ʱ�ȭ�� �� �� ����̴�.
        ProjectileVelocity = curWeapon.currentStat.attackStat.bulletSpeed;
        EnablePos = transform.position;
        ProjectileMaxDistance = curWeapon.currentStat.attackStat.range;
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
        if(rigidbody_.SweepTest(transform.forward, out hit, ProjectileVelocity * 0.1f))
        {
            Debug.Log("Hit!!");
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
