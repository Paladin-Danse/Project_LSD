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
        enabled = false;
        Destroy(gameObject, 2f);
        if (per >= 50)
        {
            Instantiate(bulletBox, transform.position, transform.rotation);
        }
        else if (per < 50)
        {
            Instantiate(firstAidKit, transform.position, transform.rotation);
        }

        DungeonManager.amountGold += TestGold.plusGold;
    }

    void OnHit()
    {
        Animator.SetTrigger("Hit");        
    }
}
