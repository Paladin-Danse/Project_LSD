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
    public bool isShotable;
    public bool isSwap;
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

    //IEnumerator
    public IEnumerator ShotCoroutine = null;
    public IEnumerator RecoilCoroutine = null;
    public IEnumerator ReloadCoroutine = null;
    public IEnumerator TakeCoroutine = null;
    public List<Mod> mods { get; private set; }
    

    [Header("Projectile")]
    public GameObject projectilePrefab;//InGame Projectile
    public Transform firePos;

    [Header("Audio")]
    protected AudioSource audioSource;
    public AudioClip shot_AudioClip;
    public AudioClip dry_AudioClip;
    public AudioClip cock_AudioClip;
    public AudioClip reload_start_AudioClip;
    public AudioClip reload_end_AudioClip;
    public AudioClip takeOut_AudioClip;

    public float shot_Volume;
    public float dry_Volume;
    public float cock_Volume;
    public float reload_Volume;
    public float takeOut_Volume;

    [Header("Animation")]
    public Animator animator;
    public WeaponAnimationData animationData;

    public void Init(PlayerCharacter playerCharacter)
    {
        playerCharacter_ = playerCharacter;
        
        animationData = new WeaponAnimationData();
        animationData.Initialize();

        if (baseStatSO != null)
        {
            baseStat = Instantiate(baseStatSO).weaponStat;
        }
        baseStat = Instantiate(baseStatSO).weaponStat;
        GetWeaponStat = () => { return baseStat; };
        mods = new List<Mod>();
        WeaponSet();
    }

    protected void Awake()
    {
        if (!TryGetComponent<Animator>(out animator)) Debug.Log("Weapon(animator) : Animator is not Found!");
        stateMachine = new GunStateMachine(this);
        if (!TryGetComponent<AudioSource>(out audioSource)) Debug.Log("this Weapon is not Found AudioSource Component!!");
        if(GetComponentInChildren<FirePos>() != null) firePos = GetComponentInChildren<FirePos>().transform;

        isShotable = true;
        isSwap = false;
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
        //RandomSpread * Recoil
        Vector3 randomSpreadCircle = UnityEngine.Random.insideUnitCircle * math.min((defaultSpread + (curRecoil * 0.01f)), maxSpread);
        return randomSpreadCircle;
    }
    public Vector3 GetRaycastHitPosition()
    {
        Camera curCam = playerCharacter_.FPCamera;
        float rayDistance = curWeaponStat.attackStat.range;
        Vector3 centerPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, math.abs(firePos.position.z - curCam.transform.position.z));
        Vector3 centerWorldPos = curCam.ScreenToWorldPoint(centerPos);

        Ray shotRay = new Ray(centerWorldPos, curCam.transform.forward + RandomSpread());
        Debug.DrawRay(shotRay.origin, curCam.transform.forward + RandomSpread(), Color.red, rayDistance);

        //return shotRay.GetPoint(rayDistance);
        
        RaycastHit hit;
        Vector3 hitPos;

        if (Physics.Raycast(shotRay, out hit, rayDistance, LayerMask.GetMask("Projectile")))
        {
            hitPos = hit.point;
        }
        else
        {
            hitPos = shotRay.GetPoint(rayDistance);
        }
        return hitPos;
    }
    public void WeaponSet()
    {
        maxMagazine = curWeaponStat.magazine;
        curMagazine = math.max(0, maxMagazine);//zero is remaining ammo to Inventory & soon develop(math.max -> math.min)
        maxRecoil = curWeaponStat.recoil * 2f;
        defaultSpread = curWeaponStat.spread * 0.01f;
        maxSpread = defaultSpread * 2f;
    }
    
    public void CurrentWeaponEquip()
    {
        input_ = playerCharacter_.input;
        playerCharacter_.playerUIEventInvoke();
        stateMachine.ChangeState(stateMachine.EnterState);
        //stateMachine.currentState.AddInputActionsCallbacks();
    }
    public void CurrentWeaponUnEquip()
    {
        stateMachine.ChangeState(stateMachine.ExitState);
    }
    public void PlayClip(AudioClip newClip, float volume)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(newClip);
    }

    public void ShotCoroutinePlay(IEnumerator shotOrDryfire)
    {
        if(ShotCoroutine == null)
        {
            ShotCoroutine = shotOrDryfire;
            StartCoroutine(ShotCoroutine);
        }
    }
    public void TakeInCoroutinePlay()
    {
        if (TakeCoroutine == null)
        {
            TakeCoroutine = TakeIn();
            StartCoroutine(TakeCoroutine);
        }
    }
    public void TakeOutCoroutinePlay()
    {
        if(TakeCoroutine == null)
        {
            TakeCoroutine = TakeOut();
            StartCoroutine(TakeCoroutine);
        }
    }

    public IEnumerator Shot()
    {
        curMagazine--;
        PlayClip(shot_AudioClip, shot_Volume);
        playerCharacter_.playerUIEventInvoke();

        //Projectile Create
        AmmoProjectile ammoProjectile = ObjectPoolManager.Instance.Pop(projectilePrefab).GetComponent<AmmoProjectile>();
        ammoProjectile.transform.position = firePos.position;
        ammoProjectile.transform.transform.LookAt(GetRaycastHitPosition());
        ammoProjectile.OnInit(stateMachine.gun);

        //Recoil
        if (RecoilCoroutine != null) StopCoroutine(RecoilCoroutine);
        RecoilCoroutine = OnRecoil();
        StartCoroutine(RecoilCoroutine);

        //Empty Check
        if (isEmpty) stateMachine.ChangeState(stateMachine.EmptyState);

        //shoot CoolTime
        yield return YieldCacher.WaitForSeconds(curWeaponStat.fireDelay);
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

            yield return YieldCacher.WaitForSeconds(0.03f);
        }
        cnt = 0;
        while (curRecoil > 0.1f)
        {
            if (isFiring) curRecoil = math.lerp(curRecoil, 0, 0.1f);
            else curRecoil = math.lerp(curRecoil, 0, 0.4f);
            cnt++;
            if (cnt > 100) break;

            yield return YieldCacher.WaitForSeconds(0.03f);
        }
        RecoilCoroutine = null;
        Debug.Log("Coroutine end");
        yield break;
    }
    public IEnumerator Reload()
    {
        PlayClip(reload_start_AudioClip, reload_Volume);
        animator.SetInteger(animationData.reloadParameterHash, 1);
        while(!animator.GetCurrentAnimatorStateInfo(0).IsTag("Reload"))
        {
            yield return null;
        }
        float reloadAnimTime = animator.GetCurrentAnimatorStateInfo(0).length;
        animator.speed = (reloadAnimTime / baseStatSO.weaponStat.reloadDelay) * 0.9f;//0.9f is For the naturalness of animation

        animator.SetInteger(animationData.reloadParameterHash, -1);

        yield return YieldCacher.WaitForSeconds(curWeaponStat.reloadDelay);
        PlayClip(reload_end_AudioClip, reload_Volume);
        animator.speed = 1;
        curMagazine = maxMagazine;
        playerCharacter_.playerUIEventInvoke();
        ReloadCoroutine = null;
        stateMachine.ChangeState(stateMachine.ReadyState);
    }
    
    public IEnumerator TakeIn()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        TakeCoroutine = null;
        stateMachine.ChangeState(stateMachine.ReadyState);
    }
    public IEnumerator TakeOut()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        TakeCoroutine = null;
        gameObject.SetActive(false);
    }
}
