using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DefaultInventoryData", menuName = "new Data/PlayerInventory")]
public class InventoryData : ScriptableObject
{
    [Header("Ammo")]
    public List<AmmoType> maxAmmoType;
    public List<int> maxAmmoCount;

    public Dictionary<AmmoType, int> maxAmmo;

    public void init()
    {
        maxAmmo = new Dictionary<AmmoType, int>();

        for(int idx = 0; idx < maxAmmoType.Count; idx++)
        {
            maxAmmo.Add(maxAmmoType[idx], maxAmmoCount[idx]);
        }
    }
}
