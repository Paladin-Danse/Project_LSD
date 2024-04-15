using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DefaultInventoryData", menuName = "new Data/PlayerInventory")]
public class InventoryData : ScriptableObject
{
    public int itemSlotCount;
    [Header("Ammo")]
    public List<AmmoType> maxAmmoTypes;
    public List<int> maxAmmoCounts;

    public Dictionary<AmmoType, int> maxAmmo;
    [Header("Item")]
    public List<string> statNames;

    public Dictionary<string, int> itemStatValues;

    [Header("Money")]
    public int playerStartMoney;
    [field: SerializeField]
    public int maxMoney { get; private set; }

    public void init()
    {
        maxAmmo = new Dictionary<AmmoType, int>();
        itemStatValues = new Dictionary<string, int>();

        for (int idx = 0; idx < maxAmmoTypes.Count; idx++)
        {
            maxAmmo.Add(maxAmmoTypes[idx], maxAmmoCounts[idx]);
        }
        for (int idx = 0; idx < statNames.Count; idx++)
        {
            itemStatValues.Add(statNames[idx], 0);
        }
    }
}
