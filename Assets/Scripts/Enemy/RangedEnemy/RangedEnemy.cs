using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public EnemySO RData { get; private set; }
    [field: SerializeField] public WeaponStatSO WSData { get; private set; }
    //[field: SerializeField] public RangedEnemyWeapon Weapon { get; private set; }
    [field: SerializeField] public EnemyProjectile Projectile { get; set; }

    [field: Header("Animations")]
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }    
    public Health health { get; private set; }

    [SerializeField] private GameObject bulletBox;
    [SerializeField] private GameObject firstAidKit;

    public RangedEnemyStateMachine stateMachine;

    void Awake()
    {
        AnimationData.Initialize();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();        
        health = GetComponent<Health>();

        stateMachine = new RangedEnemyStateMachine(this);        
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
        health.OnDie += OnDie;
        health.OnTakeDamage += OnHit;                
    }

    private void Update()
    {
        stateMachine.HandleInput();

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    void OnDie()
    {
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        int per = Random.Range(0, 99);
        Animator.SetTrigger("Die");
        
        if (per >= 50)
        {
            Instantiate(bulletBox, transform.position, transform.rotation);
        }
        else if (per < 50)
        {
            Instantiate(firstAidKit, transform.position, transform.rotation);
        }

        DungeonManager.Instance.killedEneies += 1;
        
        float goldPosX = Random.Range(0, 5);
        float goldPosZ = Random.Range(0, 5);
        float goldRot = Random.Range(0, 180);
        Instantiate(DungeonManager.Instance.goldPrefab, transform.position + new Vector3(goldPosX, 5f, goldPosZ), Quaternion.Euler(0, goldRot, 0));

        enabled = false;
        Destroy(gameObject, 2f);
    }

    void OnHit()
    {
        Animator.SetTrigger("Hit");        
    }
}
