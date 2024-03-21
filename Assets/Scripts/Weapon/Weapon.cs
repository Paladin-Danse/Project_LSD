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
        stateMachine.curMagazine = math.max(0, stateMachine.maxMagazine);//현재는 0으로 되어있는 값을 나중에 인벤토리 내에 가지고 있는 탄약을 가져와 넣을 것.
        stateMachine.maxRecoil = currentStat.recoil * 2f;
        stateMachine.recoveryRecoil = currentStat.recoil * 0.25f;
    }

    public void CurrentWeaponEquip()
    {
        gameObject.SetActive(true);
        stateMachine.ChangeState(stateMachine.ReadyState);
    }

    public AmmoProjectile CreateObject(List<AmmoProjectile> pooling_List, AmmoProjectile obj)
    {
        AmmoProjectile newProjectile = Instantiate(obj, firePos.position, Quaternion.LookRotation(-firePos.forward)).GetComponent<AmmoProjectile>();
        pooling_List.Add(newProjectile);
        return newProjectile;
    }
    public void GetStateMachine(PlayerStateMachine stateMachine)
    {
        this.stateMachine.playerStateMachine_ = stateMachine;
    }
}
