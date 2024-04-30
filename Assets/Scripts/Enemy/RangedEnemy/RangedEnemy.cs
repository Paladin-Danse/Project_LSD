using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private AudioSource hitSoundAudio;
    [SerializeField] private AudioClip hitSound;
    

    public RangedEnemyStateMachine stateMachine;

    void Awake()
    {
        AnimationData.Initialize();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();        
        health = GetComponent<Health>();

        stateMachine = new RangedEnemyStateMachine(this);

        health.OnDie += OnDie;
        health.OnTakeDamage += OnHit;
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);                        
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

        if (per >= 30)
        {
            GameObject bulletObject = ObjectPoolManager.Instance.Pop(bulletBox).gameObject;
            bulletObject.transform.position = transform.position;
            bulletObject.transform.rotation = transform.rotation;
            bulletObject.SetActive(true);
        }
        else if (per < 30)
        {
            GameObject firstAidObject = ObjectPoolManager.Instance.Pop(firstAidKit).gameObject;
            firstAidObject.transform.position = transform.position;
            firstAidObject.transform.rotation = transform.rotation;
            firstAidObject.SetActive(true);
        }

        DungeonTracker.Instance.killedEnemies += 1;

        float gper = Random.Range(0, 99);
        if (gper >= 50)
        {
            float goldPosX = Random.Range(0, 1f);
            float goldPosZ = Random.Range(0, 1f);
            float goldRot = Random.Range(0, 180f);

            GameObject gold = ObjectPoolManager.Instance.Pop("Gold").gameObject;
            gold.transform.position = transform.position + new Vector3(goldPosX, 0f, goldPosZ);
            gold.transform.rotation = Quaternion.Euler(0, goldRot, 0);
            gold.SetActive(true);
        }

        enabled = false;
        Destroy(gameObject, 2f);
    }

    void OnHit()
    {
        Animator.SetTrigger("Hit");
        hitSoundAudio.PlayOneShot(hitSound);
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * -10f;
    }
}
