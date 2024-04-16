using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public EnemySO Data { get; private set; }
    [field: SerializeField] public EnemyWeapon Weapon { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }

    [SerializeField] private GameObject bulletBox;
    [SerializeField] private GameObject firstAidKit;    

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }        
    public Health health { get; private set; }

    private EnemyStateMachine stateMachine;

    void Awake()
    {
        AnimationData.Initialize();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();                
        health = GetComponent<Health>();

        stateMachine = new EnemyStateMachine(this);
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
            ObjectPoolManager.Instance.Pop(bulletBox);
            bulletBox.transform.position = transform.position;
            bulletBox.transform.rotation = transform.rotation;
        }
        else if (per < 50)
        {
            ObjectPoolManager.Instance.Pop(firstAidKit);
            firstAidKit.transform.position = transform.position;
            firstAidKit.transform.rotation = transform.rotation;
        }

        DungeonManager.Instance.killedEneies += 1;

        float gper = Random.Range(0, 99);
        if(gper >= 50)
        {
            float goldPosX = Random.Range(0, 1f);
            float goldPosZ = Random.Range(0, 1f);
            float goldRot = Random.Range(0, 180f);
            Instantiate(DungeonManager.Instance.goldPrefab, transform.position + new Vector3(goldPosX, 1f, goldPosZ), Quaternion.Euler(0, goldRot, 0));
        }        

        enabled = false;
        //Destroy(gameObject, 2f);
    }

    void OnHit()
    {
        Animator.SetTrigger("Hit");        
    }
}
