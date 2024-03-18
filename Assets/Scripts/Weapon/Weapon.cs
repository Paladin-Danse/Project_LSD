using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : StatHandlerBase<WeaponStat>
{
    public GunStateMachine stateMachine { get; private set; }
    public PlayerInput input_ { get; private set; }
    [SerializeField]
    WeaponStatSO baseStatSO;
    [SerializeField]
    public bool isAuto;
    public bool isFiring;
    List<Mod> mods;
    public List<AmmoProjectile> weaponProjectile_List;
    public AmmoProjectile ammoProjectile;
    public Transform firePos;
    
    protected override void InitStat()
    {
        base.InitStat();
        //currentStat.OverlapStats(baseStatSO.weaponStat);//currentStat을 그냥 삭제해버림.
        //currentStat.OverlapStat(baseStatSO.weaponStat);//currentStat에 값이 덧씌워지지 않음.

        stateMachine = new GunStateMachine(this);
        input_ = GetComponentInParent<PlayerInput>();
        weaponProjectile_List = new List<AmmoProjectile>();

    }

    protected void Start()
    {
        stateMachine.ChangeState(stateMachine.ReadyState);
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
    protected void WeaponSet()
    {
        stateMachine.weaponAttackDelay = new WaitForSeconds(stateMachine.Gun.baseStatSO.weaponStat.fireDelay);
    }
    public AmmoProjectile CreateObject(List<AmmoProjectile> pooling_List, AmmoProjectile obj)
    {
        AmmoProjectile newProjectile = Instantiate(obj, firePos.position, Quaternion.LookRotation(-firePos.forward)).GetComponent<AmmoProjectile>();
        pooling_List.Add(newProjectile);
        return newProjectile;
    }
}
