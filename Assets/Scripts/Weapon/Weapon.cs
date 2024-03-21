using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : StatHandlerBase<WeaponStat>
{
    public GunStateMachine stateMachine { get; private set; }
    public PlayerInput input_ { get; private set; }
    [SerializeField]
    WeaponStatSO baseStatSO;

    public bool isAuto;
    public bool isFiring;
    List<Mod> mods;
    public List<AmmoProjectile> weaponProjectile_List;
    protected GameObject projectiles;
    public AmmoProjectile ammoProjectile;
    public Transform firePos;
    
    protected override void InitStat()
    {
        base.InitStat();

        if (baseStatSO != null)
        {
            baseStat = baseStatSO.weaponStat;
        }

        stateMachine = new GunStateMachine(this);
        input_ = GetComponentInParent<PlayerInput>();
        weaponProjectile_List = new List<AmmoProjectile>();
        projectiles = new GameObject("Projectiles");
    }

    private void Start()
    {
        WeaponSet();
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
        AddStatModifier(mod.modStat);
    }

    public void RemoveMod(Mod mod)
    {
        mods.Remove(mod);
        RemoveStatModifier(mod.modStat);
    }
    public void WeaponSet()
    {
        stateMachine.weaponAttackDelay = new WaitForSeconds(stateMachine.Gun.baseStatSO.weaponStat.fireDelay);
        stateMachine.weaponReloadDelay = new WaitForSeconds(stateMachine.Gun.baseStatSO.weaponStat.reloadDelay);
        stateMachine.whileRestTimeSeconds = new WaitForSeconds(0.03f);
        stateMachine.maxMagazine = currentStat.magazine;
        stateMachine.curMagazine = math.max(0, stateMachine.maxMagazine);//����� 0���� �Ǿ��ִ� ���� ���߿� �κ��丮 ���� ������ �ִ� ź���� ������ ���� ��.
        stateMachine.maxRecoil = currentStat.recoil * 2f;
        stateMachine.defaultSpread = currentStat.spread;
        stateMachine.maxSpread = currentStat.spread * 2f;

        for (int i = 0; i < stateMachine.maxMagazine; i++)
            CreateObject(weaponProjectile_List, ammoProjectile).gameObject.SetActive(false);
    }

    public void CurrentWeaponEquip()
    {
        gameObject.SetActive(true);
        stateMachine.ChangeState(stateMachine.ReadyState);
    }

    public AmmoProjectile CreateObject(List<AmmoProjectile> pooling_List, AmmoProjectile obj)
    {
        AmmoProjectile newProjectile = Instantiate(obj, firePos.position, Quaternion.LookRotation(-firePos.forward)).GetComponent<AmmoProjectile>();

        if(projectiles == null) projectiles = new GameObject("Projectiles");
        newProjectile.transform.parent = projectiles.transform;
        pooling_List.Add(newProjectile);
        return newProjectile;
    }
    public void GetStateMachine(PlayerStateMachine stateMachine)
    {
        this.stateMachine.playerStateMachine_ = stateMachine;
    }
}
