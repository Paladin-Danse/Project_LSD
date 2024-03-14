using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Weapon : StatHandlerBase<WeaponStat>
{
    [SerializeField]
    WeaponStatSO baseStatSO;

    List<Mod> mods;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void InitStat()
    {
        base.InitStat();
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

}
