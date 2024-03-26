using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public PlayerCharacter playerCharacter_ { get; private set; }

    public GunStateMachine stateMachine { get; private set; }
    public PlayerInput input_;
    [SerializeField]
    WeaponStatSO baseStatSO;
    public WeaponStat baseStat;

    public WeaponStat curWeaponStat { get { return GetWeaponStat(); } }
    public Func<WeaponStat> GetWeaponStat;

    [Header("Weapon")]
    public string WeaponName;
    public bool isAuto;
    public bool isFiring;
    public int maxMagazine;
    public string maxMagazineText { get { return maxMagazine.ToString(); } }
    public int curMagazine;
    public string curMagazineText { get { return curMagazine.ToString(); } }
    public bool isEmpty => curMagazine <= 0;
    private float targetRecoil = 0f;
    public float curRecoil { get; private set; } = 0f;
    public float maxRecoil;
    public float defaultSpread = 0f;
    public float maxSpread;

    public Quaternion weaponTargetRotation { get; private set; }

    public WaitForSeconds weaponAttackDelay;
    public WaitForSeconds weaponReloadDelay;
    public WaitForSeconds whileRestTimeSeconds;

    public IEnumerator ShotCoroutine = null;
    public IEnumerator RecoilCoroutine = null;
    public IEnumerator ReloadCoroutine = null;

    public List<Mod> mods { get; private set; }
    [Header("Projectile")]
    public List<AmmoProjectile> weaponProjectile_List;//poolingList
    protected GameObject projectiles;//InGame Projectile Parent Object
    public AmmoProjectile ammoProjectile;//InGame Projectile
    public Transform firePos;

    [Header("Audio")]
    protected AudioSource audioSource;
    public AudioClip shot_AudioClip;
    public AudioClip dry_AudioClip;
    public AudioClip cock_AudioClip;
    public AudioClip reload_start_AudioClip;
    public AudioClip reload_end_AudioClip;

    public float shot_Volume;
    public float dry_Volume;
    public float cock_Volume;
    public float reload_Volume;

    [Header("Animation")]
    public Animator animator;
    public WeaponAnimationData animationData;

    public void Init(PlayerCharacter playerCharacter)
    {
        playerCharacter_ = playerCharacter;
        input_ = playerCharacter_.input;
        WeaponSet();
        CurrentWeaponEquip();
        stateMachine.currentState.AddInputActionsCallbacks();
    }

    protected void Awake()
    {
        if (baseStatSO != null)
        {
            baseStat = Instantiate(baseStatSO).weaponStat;
        }

        if (!TryGetComponent<Animator>(out animator)) Debug.Log("Weapon(animator) : Animator is not Found!");
        animationData = new WeaponAnimationData();

        stateMachine = new GunStateMachine(this);
        if (!TryGetComponent<AudioSource>(out audioSource)) Debug.Log("this Weapon is not Found AudioSource Component!!");
        weaponProjectile_List = new List<AmmoProjectile>();
        projectiles = new GameObject("Projectiles");
        mods = new List<Mod>();

        baseStat = Instantiate(baseStatSO).weaponStat;
        GetWeaponStat = () => { return baseStat; };
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


    public void AddMod(Mod mod) 
    {
        mods.Add(mod);
    }

    public void RemoveMod(Mod mod)
    {
        mods.Remove(mod);
    }
    public Vector3 RandomSpread()
    {
        /*
        Camera curCam = Camera.main;
        float rayDistance = curWeaponStat.attackStat.range;
        Vector3 centerPos = new Vector3(curCam.pixelWidth * 0.5f, curCam.pixelHeight * 0.5f, rayDistance);
        //RandomSpread * Recoil
        Vector3 randomSpreadCircle = new Vector3(UnityEngine.Random.insideUnitCircle.normalized.x, UnityEngine.Random.insideUnitCircle.normalized.y, 0) * stateMachine.defaultSpread * (stateMachine.curRecoil * 0.01f);
        Ray shotRay = curCam.ScreenPointToRay(centerPos + randomSpreadCircle);
        RaycastHit hit;
        Vector3 hitPos;

        Debug.DrawRay(shotRay.origin, shotRay.direction, Color.red, rayDistance);

        if(Physics.Raycast(shotRay, out hit, rayDistance, ammoProjectile.TargetLayer))
        {
            hitPos = hit.point;
        }
        else
        {
            hitPos = curCam.ScreenToWorldPoint(shotRay.origin);
            hitPos.z += rayDistance;
        }
        
        return hitPos;
        */
        
        //RandomSpread * Recoil
        Vector3 randomSpreadCircle = new Vector3(UnityEngine.Random.insideUnitCircle.normalized.x, UnityEngine.Random.insideUnitCircle.normalized.y, 0) * defaultSpread * (curRecoil * 0.01f);
        return -firePos.forward + randomSpreadCircle;
        
    }
    public void WeaponSet()
    {
        weaponAttackDelay = new WaitForSeconds(baseStatSO.weaponStat.fireDelay);
        weaponReloadDelay = new WaitForSeconds(baseStatSO.weaponStat.reloadDelay);
        whileRestTimeSeconds = new WaitForSeconds(0.03f);
        maxMagazine = curWeaponStat.magazine;
        curMagazine = math.max(0, maxMagazine);//����� 0���� �Ǿ��ִ� ���� ���߿� �κ��丮 ���� ������ �ִ� ź���� ������ ���� ��.
        maxRecoil = curWeaponStat.recoil * 2f;
        defaultSpread = curWeaponStat.spread * 0.01f;
        maxSpread = defaultSpread * 2f;

        if (weaponProjectile_List.Find(x => x == ammoProjectile) == null)
        {
            for (int i = 0; i < maxMagazine; i++)
                CreateObject(weaponProjectile_List, ammoProjectile).gameObject.SetActive(false);
        }
    }

    public void CurrentWeaponEquip()
    {
        gameObject.SetActive(true);
        WeaponSet();
        PlayClip(cock_AudioClip, cock_Volume);
        stateMachine.ChangeState(stateMachine.ReadyState);

        //transform.rotation = WeaponForward();
    }

    public Quaternion WeaponForward()
    {
        Ray ray = playerCharacter_.FPCamera.ScreenPointToRay(new Vector3(playerCharacter_.FPCamera.pixelWidth * 0.5f, playerCharacter_.FPCamera.pixelWidth * 0.5f, 0));
        return Quaternion.LookRotation(ray.GetPoint(curWeaponStat.attackStat.range));
    }

    public AmmoProjectile CreateObject(List<AmmoProjectile> pooling_List, AmmoProjectile obj)
    {
        AmmoProjectile newProjectile = Instantiate(obj, firePos.position, Quaternion.LookRotation(-firePos.forward)).GetComponent<AmmoProjectile>();

        if(projectiles == null) projectiles = new GameObject("Projectiles");
        newProjectile.transform.parent = projectiles.transform;
        pooling_List.Add(newProjectile);
        return newProjectile;
    }
    public void PlayClip(AudioClip newClip, float volume)
    {
        audioSource.clip = newClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    protected void ProjectilePooling(AmmoProjectile projectile)
    {
        AmmoProjectile newProjectile;
        if (weaponProjectile_List.Exists(x => x.gameObject.activeSelf == false))
            newProjectile = weaponProjectile_List.Find(x => x.gameObject.activeSelf == false);
        else
            newProjectile = CreateObject(weaponProjectile_List, projectile);

        newProjectile.transform.position = firePos.position;
        newProjectile.transform.rotation = Quaternion.LookRotation(RandomSpread());
        newProjectile.OnInit(stateMachine.Gun);
    }

    public void ShotCoroutinePlay(IEnumerator shotOrDryfire)
    {
        if(ShotCoroutine == null)
        {
            ShotCoroutine = shotOrDryfire;
            StartCoroutine(ShotCoroutine);
        }
    }

    public IEnumerator Shot()
    {
        curMagazine--;
        PlayClip(shot_AudioClip, shot_Volume);
        playerCharacter_.playerUIEventInvoke();

        //Projectile Create
        ProjectilePooling(ammoProjectile);
        //Recoil
        if (RecoilCoroutine != null) StopCoroutine(RecoilCoroutine);
        RecoilCoroutine = OnRecoil();
        StartCoroutine(RecoilCoroutine);
        //Empty Check
        if (isEmpty) stateMachine.ChangeState(stateMachine.EmptyState);
        //shoot CoolTime
        yield return weaponAttackDelay;
        ShotCoroutine = null;
    }

    public IEnumerator DryFire()
    {
        PlayClip(dry_AudioClip, dry_Volume);
        yield return null;
        ShotCoroutine = null;
    }

    public IEnumerator OnRecoil()
    {
        targetRecoil = math.min(curRecoil + curWeaponStat.recoil, maxRecoil);

        int cnt = 0;
        while (curRecoil < targetRecoil - 0.1f)
        {
            curRecoil = math.lerp(curRecoil, targetRecoil, 0.4f);
            cnt++;
            if (cnt > 100) break;

            yield return whileRestTimeSeconds;
        }
        cnt = 0;
        while (curRecoil > 0.1f)
        {
            if (isFiring) curRecoil = math.lerp(curRecoil, 0, 0.1f);
            else curRecoil = math.lerp(curRecoil, 0, 0.4f);
            cnt++;
            if (cnt > 100) break;

            yield return whileRestTimeSeconds;
        }
        RecoilCoroutine = null;
        Debug.Log("Coroutine end");
        yield break;
    }
    public IEnumerator Reload()
    {
        PlayClip(reload_start_AudioClip, reload_Volume);
        animator.SetInteger(animationData.reloadParameterHash, 1);

        yield return weaponReloadDelay;
        PlayClip(reload_end_AudioClip, reload_Volume);
        animator.SetInteger(animationData.reloadParameterHash, -1);
        curMagazine = maxMagazine;
        playerCharacter_.playerUIEventInvoke();
        ReloadCoroutine = null;
        stateMachine.ChangeState(stateMachine.ReadyState);
    }
}
