using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBoss : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public EnemySO RData { get; private set; }
    [field: SerializeField] public WeaponStatSO WSData { get; private set; }    
    public BossProjectile BProjectile { get; set; }
    public BossSmallProjectile SProjectile { get; set; }

    [field: Header("Animations")]
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public Health health { get; private set; }

    [SerializeField] private GameObject bulletBox;
    [SerializeField] private GameObject firstAidKit;
    [SerializeField] private GameObject GoldPrefab;

    public DesertBossStateMachine stateMachine;
    public AudioSource audioSource;
    public AudioClip dieSound;

    void Awake()
    {
        AnimationData.Initialize();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();

        stateMachine = new DesertBossStateMachine(this);

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
        health.OnDie += OnDie;        
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
        if (per >= 50)
        {
            Instantiate(bulletBox, transform.position, transform.rotation);
        }
        else if (per < 50)
        {
            Instantiate(firstAidKit, transform.position, transform.rotation);
        }

        audioSource.PlayOneShot(dieSound);
        QuestManager.Instance.DQuestUpdate(1009, 1);

        for(int i = 0; i < TestGold.goldCount;  i++)
        {
            int posX = Random.Range(0, 5);
            int posZ = Random.Range(0, 5);
            int rot = Random.Range(0, 180);
            Instantiate(GoldPrefab, transform.position + new Vector3(posX, 0, posZ), transform.rotation * new Quaternion(0,rot,0,0));
            DungeonManager.amountGold -= TestGold.plusGold;
            TestGold.goldCount += 1;
            if(DungeonManager.amountGold <= 0)
            {
                TestGold.goldCount -= 1;
            }
        }

        Destroy(gameObject, 2f);
    }    
}
