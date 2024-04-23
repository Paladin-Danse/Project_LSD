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
    [SerializeField] private GameObject SmallShip;        

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
        Destroy(gameObject, 2f);
        if (per >= 50)
        {
            Instantiate(bulletBox, transform.position, transform.rotation);
        }
        else if (per < 50)
        {
            Instantiate(firstAidKit, transform.position, transform.rotation);
        }

        audioSource.PlayOneShot(dieSound);

        DungeonTracker.Instance.killedEnemies += 1;
        DQU();

        int gCount = Random.Range(3, 10);
        for(int i = 0; i < gCount;  i++)
        {
            float goldPosX = Random.Range(0, 2f);
            float goldPosZ = Random.Range(0, 2f);
            float goldRot = Random.Range(0, 180f);
            Instantiate(DungeonManager.Instance.goldPrefab, transform.position + new Vector3(goldPosX, 0f, goldPosZ), Quaternion.Euler(0, goldRot, 0));
        }        

        SmallShip.SetActive(true);        
    }    

    void DQU()
    {
        QuestManager.Instance.DQuestUpdate(SelectedDungeonContext.Instance.selectedDungeonData.QuestID, 1);
    }
}
