using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform turretBody = null;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float spinSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float t_ProjectileSpeed;
    public float t_ProjectileDamage;    
    [SerializeField] GameObject turretProjectile;
    [SerializeField] Transform t_MuzzlePos1;
    [SerializeField] Transform t_MuzzlePos2;
    [SerializeField] Transform t_MuzzlePos3;
    [SerializeField] Transform t_MuzzlePos4;
    [SerializeField] WeaponStatSO turretStat;
    [SerializeField] GameObject destroyPre;
    [SerializeField] GameObject muzzleShotPar;

    float curFireRate;

    [HideInInspector] public Transform target = null;
    Animator anim;
    public Animator dieAnim;
    AudioSource audioSource;
    public AudioClip t_ShotSound;
    public AudioClip dieSound;
    public AudioClip upSound;
    public AudioClip downSound;
    Health health;
    public Health _Target;

    public TurretProjectile t_Projectile1 { get; set; }
    public TurretProjectile t_Projectile2 { get; set; }
    public TurretProjectile t_Projectile3 { get; set; }
    public TurretProjectile t_Projectile4 { get; set; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        attackRange = turretStat.weaponStat.attackStat.range;
        fireRate = turretStat.weaponStat.fireDelay;
        t_ProjectileSpeed = turretStat.weaponStat.attackStat.bulletSpeed;
        t_ProjectileDamage = turretStat.weaponStat.attackStat.damage;
        anim = GetComponent<Animator>();        
        health = GetComponent<Health>();
        health.OnDie += TOnDie;
    }

    void Start()
    {
        dieAnim.enabled = false;
        curFireRate = fireRate;        
        InvokeRepeating("SearchEnemy", 0f, 0.5f);
    }
    
    void Update()
    {
        if(target == null)
        {                        
            anim.SetBool("Attack", false);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            anim.SetBool("Attack", true);
            Quaternion lookRotation = Quaternion.LookRotation(target.position - turretBody.position);
            Vector3 euler = Quaternion.RotateTowards(turretBody.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
            turretBody.rotation = Quaternion.Euler(0, euler.y, 0);            
            gameObject.GetComponent<CapsuleCollider>().enabled = true;

            Quaternion fireRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            if(Quaternion.Angle(turretBody.rotation, fireRotation) < 5f)
            {
                curFireRate -= Time.deltaTime;
                if(curFireRate <= 0f)
                {
                    curFireRate = fireRate;
                    Invoke("TurretShot", 1.5f);
                }
            }
        }
    }

    void SearchEnemy()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, attackRange, layerMask);
        Transform shortestTarget = null;

        if (cols.Length > 0)
        {
            float shortestDistance = Mathf.Infinity;
            foreach (Collider colTarget in cols)
            {
                float distance = Vector3.SqrMagnitude(transform.position - colTarget.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    shortestTarget = colTarget.transform;
                }
            }
        }

        target = shortestTarget;
    }

    void TurretShot()
    {
        StartCoroutine("TShot");
    }

    void STurretShot()
    {
        StopCoroutine("TShot");
    }

    IEnumerator TShot()
    {
        TurretProjectile instantProjectile1 = ObjectPoolManager.Instance.Pop(turretProjectile).GetComponent<TurretProjectile>();
        instantProjectile1.transform.position = t_MuzzlePos1.position;
        instantProjectile1.transform.forward = t_MuzzlePos1.forward;

        TurretProjectile instantProjectile2 = ObjectPoolManager.Instance.Pop(turretProjectile).GetComponent<TurretProjectile>();
        instantProjectile2.transform.position = t_MuzzlePos2.position;
        instantProjectile2.transform.forward = t_MuzzlePos2.forward;

        TurretProjectile instantProjectile3 = ObjectPoolManager.Instance.Pop(turretProjectile).GetComponent<TurretProjectile>();
        instantProjectile3.transform.position = t_MuzzlePos3.position;
        instantProjectile3.transform.forward = t_MuzzlePos3.forward;

        TurretProjectile instantProjectile4 = ObjectPoolManager.Instance.Pop(turretProjectile).GetComponent<TurretProjectile>();
        instantProjectile4.transform.position = t_MuzzlePos4.position;
        instantProjectile4.transform.forward = t_MuzzlePos4.forward;        

        GameObject particle1 = Instantiate(muzzleShotPar, t_MuzzlePos1.position, t_MuzzlePos1.rotation);
        GameObject particle2 = Instantiate(muzzleShotPar, t_MuzzlePos2.position, t_MuzzlePos2.rotation);
        GameObject particle3 = Instantiate(muzzleShotPar, t_MuzzlePos3.position, t_MuzzlePos3.rotation);
        GameObject particle4 = Instantiate(muzzleShotPar, t_MuzzlePos4.position, t_MuzzlePos4.rotation);

        Rigidbody projectileRigid1 = instantProjectile1.GetComponent<Rigidbody>();
        Rigidbody projectileRigid2 = instantProjectile2.GetComponent<Rigidbody>();
        Rigidbody projectileRigid3 = instantProjectile3.GetComponent<Rigidbody>();
        Rigidbody projectileRigid4 = instantProjectile4.GetComponent<Rigidbody>();

        projectileRigid1.velocity = t_MuzzlePos1.forward * t_ProjectileSpeed;
        projectileRigid2.velocity = t_MuzzlePos2.forward * t_ProjectileSpeed;
        projectileRigid3.velocity = t_MuzzlePos3.forward * t_ProjectileSpeed;
        projectileRigid4.velocity = t_MuzzlePos4.forward * t_ProjectileSpeed;

        t_Projectile1 = instantProjectile1.GetComponent<TurretProjectile>();
        t_Projectile2 = instantProjectile2.GetComponent<TurretProjectile>();
        t_Projectile3 = instantProjectile3.GetComponent<TurretProjectile>();
        t_Projectile4 = instantProjectile4.GetComponent<TurretProjectile>();

        audioSource.PlayOneShot(t_ShotSound);
        audioSource.PlayOneShot(t_ShotSound);
        audioSource.PlayOneShot(t_ShotSound);
        audioSource.PlayOneShot(t_ShotSound);

        Destroy(particle1, 0.5f);
        Destroy(particle2, 0.5f);
        Destroy(particle3, 0.5f);
        Destroy(particle4, 0.5f);

        t_Projectile1.TInitProjectile(this);
        t_Projectile2.TInitProjectile(this);
        t_Projectile3.TInitProjectile(this);
        t_Projectile4.TInitProjectile(this);

        yield return null;
    }

    void TOnDie()
    {
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        
        STurretShot();

        enabled = false;
        anim.enabled = false;
        dieAnim.enabled = true;

        audioSource.PlayOneShot(dieSound);
        DungeonTracker.Instance.killedEnemies += 1;
        GameObject dP = Instantiate(destroyPre, transform.position + new Vector3(2.5f,0.5f,0), Quaternion.identity);
        GameObject dP2 = Instantiate(destroyPre, transform.position + new Vector3(-2.5f, 0.5f, 0), Quaternion.identity);
        GameObject dP3 = Instantiate(destroyPre, transform.position + new Vector3(0, 0.5f, 2.5f), Quaternion.identity);
        GameObject dP4 = Instantiate(destroyPre, transform.position + new Vector3(0, 0.5f, -2.5f), Quaternion.identity);
        Destroy(dP, 2f);
        Destroy(dP2, 2f);
        Destroy(dP3, 2f);
        Destroy(dP4, 2f);
        Destroy(gameObject, 3f);
    }   
    
    void OnUpSound()
    {
        audioSource.PlayOneShot(upSound);
    }

    void OnDownSound()
    {
        audioSource.PlayOneShot(downSound);
    }
}
