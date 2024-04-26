using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossBigWeapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public ParticleSystem boomSmoke1;
    public ParticleSystem boomSmoke2;
    public ParticleSystem boomSmoke3;
    public ParticleSystem boomSmoke4;
    public Transform muzzlePos1;
    public Transform muzzlePos2;
    public Transform muzzlePos3;
    public Transform muzzlePos4;
    public AudioSource audioSource;
    public AudioClip Cannon1;
    public AudioClip Cannon2;
    [HideInInspector] public float projectileSpeed;
    [HideInInspector] public float projectileDamage;
    [HideInInspector] public float projectileDistance;
    WaitForSeconds WFS;
    DesertBoss desertBoss;

    private void Awake()
    {
        desertBoss = GetComponent<DesertBoss>();
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = SoundManager.instance.UISound.outputAudioMixerGroup;
        projectileSpeed = desertBoss.WSData.weaponStat.attackStat.bulletSpeed;
        projectileDamage = desertBoss.WSData.weaponStat.attackStat.damage;
        projectileDistance = desertBoss.WSData.weaponStat.attackStat.range;
    }

    private void Start()
    {
        WFS = new WaitForSeconds(desertBoss.stateMachine.Enemy.RData.AttackRate);
    }
    public void FirstShot()
    {
        StartCoroutine("FShot");
    }

    public void SecondShot()
    {
        StartCoroutine("SShot");
    }

    IEnumerator FShot()
    {        
        BossProjectile instantProjectile1 = ObjectPoolManager.Instance.Pop(projectilePrefab).GetComponent<BossProjectile>();
        instantProjectile1.transform.position = muzzlePos1.position;
        instantProjectile1.transform.forward = muzzlePos1.forward;
        
        BossProjectile instantProjectile2 = ObjectPoolManager.Instance.Pop(projectilePrefab).GetComponent<BossProjectile>();
        instantProjectile2.transform.position = muzzlePos2.position;
        instantProjectile2.transform.forward = muzzlePos2.forward;

        Rigidbody projectileRigid1 = instantProjectile1.GetComponent<Rigidbody>();
        Rigidbody projectileRigid2 = instantProjectile2.GetComponent<Rigidbody>();

        projectileRigid1.velocity = muzzlePos1.forward * projectileSpeed;
        projectileRigid2.velocity = muzzlePos2.forward * projectileSpeed;

        desertBoss.BProjectile1 = instantProjectile1.GetComponent<BossProjectile>();
        desertBoss.BProjectile2 = instantProjectile2.GetComponent<BossProjectile>();
        
        desertBoss.BProjectile1.BInitProjectile(this);
        desertBoss.BProjectile2.BInitProjectile(this);
        yield return WFS;
    }

    IEnumerator SShot()
    {
        BossProjectile instantProjectile3 = ObjectPoolManager.Instance.Pop(projectilePrefab).GetComponent<BossProjectile>();
        instantProjectile3.transform.position = muzzlePos3.position;
        instantProjectile3.transform.forward = muzzlePos3.forward;

        BossProjectile instantProjectile4 = ObjectPoolManager.Instance.Pop(projectilePrefab).GetComponent<BossProjectile>();
        instantProjectile4.transform.position = muzzlePos4.position;
        instantProjectile4.transform.forward = muzzlePos4.forward;        
        
        Rigidbody projectileRigid3 = instantProjectile3.GetComponent<Rigidbody>();
        Rigidbody projectileRigid4 = instantProjectile4.GetComponent<Rigidbody>();

        projectileRigid3.velocity = muzzlePos3.forward * projectileSpeed;
        projectileRigid4.velocity = muzzlePos4.forward * projectileSpeed;

        desertBoss.BProjectile3 = instantProjectile3.GetComponent<BossProjectile>();
        desertBoss.BProjectile4 = instantProjectile4.GetComponent<BossProjectile>();

        desertBoss.BProjectile3.BInitProjectile(this);
        desertBoss.BProjectile4.BInitProjectile(this);
        yield return WFS;
    }

    public void FBoomSmoke()
    {
        boomSmoke1.Play();
        boomSmoke2.Play();
        audioSource.PlayOneShot(Cannon1);
        audioSource.PlayOneShot(Cannon2);
    }

    public void SBoomSmoke()
    {
        boomSmoke3.Play();
        boomSmoke4.Play();
        audioSource.PlayOneShot(Cannon1);
        audioSource.PlayOneShot(Cannon2);
    }
}
