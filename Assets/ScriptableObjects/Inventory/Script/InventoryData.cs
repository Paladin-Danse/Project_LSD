using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DefaultInventoryData", menuName = "new Data/PlayerInventory")]
public class InventoryData : ScriptableObject
{
    [Header("Ammo")]
    public List<AmmoType> maxAmmoTypes;
    public List<int> maxAmmoCounts;

    public Dictionary<AmmoType, int> maxAmmo;
    [Header("Item")]
    public List<string> statNames;

    public Dictionary<string, int> itemStatValues;
    public void init()
    {
        maxAmmo = new Dictionary<AmmoType, int>();
        itemStatValues = new Dictionary<string, int>();

        for(int idx = 0; idx < maxAmmoTypes.Count; idx++)
        {
            maxAmmo.Add(maxAmmoTypes[idx], maxAmmoCounts[idx]);
        }
        for (int idx = 0; idx < statNames.Count; idx++)
        {
            itemStatValues.Add(statNames[idx], 0);
        }
    }
}
