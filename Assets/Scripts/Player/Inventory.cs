using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Reflection;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;

public interface IObjectCrash
{
    void TakeAmmoItem(int percent);
}

// 아이템 슬롯 클래스
public class ItemSlot
{
    public ItemData data;
    public GameObject itemObject;
    public int index;
    public bool isEmpty
    {
        get { return data == null; }
    }
    
    public void Init(int _index)
    {
        index = _index;
    }

    public void Set(ItemData _data, GameObject _itemObject)
    {
        data = _data;
        itemObject = _itemObject;
    }
    public void Clear()
    {
        data = null;
        itemObject = null;
    }
    public void Equip()
    {
        //장착하면 해당 아이템은 인벤토리에 사라지고 장비슬롯에 등장.
    }
}

// 인벤토리를 표현하기 위한 클래스
public class Inventory : MonoBehaviour, IObjectCrash
{
    public InventoryUI inventoryUI;

    public InventoryData inventorySO;
    public Transform dropPosition; // 아이템 버리기 위치

    public Dictionary<AmmoType, int> inventoryAmmo { get; private set; }

    [Header("Selected Item")]
    private ItemSlot selectedItem = null; // 선택한 아이템
    private int selectedItemIndex; // 선택한 아이템 인덱스

    private int _playerMoney;
    private int _playerMaxMoney;

    public event Action OnAmmoChanged;

    public int money
    {
        get { return _playerMoney; }
        set
        {
            _playerMoney = value;
            _playerMoney = math.clamp(_playerMoney, 0, _playerMaxMoney);
            inventoryUI?.MoneyUI_Update();
        }
    }
    
    public void Init(InventoryUI _inventoryUI)
    {
        inventoryUI = _inventoryUI;

        if (inventorySO == null)
        {
            inventorySO = Addressables.LoadAssetAsync<InventoryData>("DefaultInventoryData").WaitForCompletion();
        }
        inventorySO.init();
        inventoryAmmo = new Dictionary<AmmoType, int>();
        _playerMaxMoney = inventorySO.maxMoney;

        foreach (AmmoType ammo in Enum.GetValues(typeof(AmmoType)))
        {
            if (ammo == AmmoType.None) continue;
            else if (!inventoryAmmo.ContainsKey(ammo))
                inventoryAmmo.Add(ammo, inventorySO.maxAmmo[ammo]);
        }
        money += inventorySO.playerStartMoney;

        inventoryUI.Init(this);

        dropPosition = Player.Instance.playerCharacter.transform.Find("DropPosition");
    }

    public void TakeAmmoItem(int percent)
    {
        AmmoType primaryWeaponAmmo = Player.Instance.playerCharacter.primaryWeapon.baseStat.e_useAmmo;
        AmmoType secondaryWeaponAmmo = Player.Instance.playerCharacter.secondaryWeapon.baseStat.e_useAmmo;

        inventoryAmmo[primaryWeaponAmmo] = math.min(inventoryAmmo[primaryWeaponAmmo] + (int)(inventorySO.maxAmmo[primaryWeaponAmmo] * (percent * 0.01f)), inventorySO.maxAmmo[primaryWeaponAmmo]);
        inventoryAmmo[secondaryWeaponAmmo] = math.min(inventoryAmmo[secondaryWeaponAmmo] + (int)(inventorySO.maxAmmo[secondaryWeaponAmmo] * (percent * 0.01f)), inventorySO.maxAmmo[secondaryWeaponAmmo]);

        OnAmmoChanged?.Invoke();
        if (inventoryUI.gameObject.active) // 인벤토리를 열고있다면
        {
            inventoryUI.Inventory_AmmoUI_Update();
        }
    }
    public int LostorUsedAmmo(AmmoType ammoType, int count)
    {
        int leftAmmo = inventoryAmmo[ammoType] < count ? inventoryAmmo[ammoType] : count;
        inventoryAmmo[ammoType] -= leftAmmo;

        OnAmmoChanged?.Invoke();
        if (inventoryUI.gameObject.activeSelf) // 인벤토리를 열고있다면
        {
            inventoryUI.Inventory_AmmoUI_Update();
        }

        return leftAmmo;
    }
    public int InventoryAmmoCheck(AmmoType ammoType)
    {
        return inventoryAmmo[ammoType];
    }

    public void AddWeapon(ItemData itemData, WeaponStat weaponStat)
    {
        if (!Player.Instance.playerCharacter.InventoryWeaponEquip(weaponStat))
            AddItem(itemData, weaponStat);
    }

    // 인벤토리에 아이템 추가
    public void AddItem(ItemData itemdata, WeaponStat _weaponStat)
    {
        ItemSlotUI emptySlot = inventoryUI.GetEmptySlot(); // 비어있는 아이템 슬롯 찾기

        if (emptySlot != null) // 비어있는 슬롯이 있다면
        {
            emptySlot.Set(itemdata, _weaponStat);
            inventoryUI.UpdateSlotUI();
            return;
        }

        ThrowItem(itemdata); // 인벤토리가 꽉 찼다면 버리기
    }
    //public void AddItem(ItemData item, Equipment equipment)

    // 아이템 버림
    void ThrowItem(ItemData item)
    {
        Poolable dropItem = ObjectPoolManager.Instance.Pop(item.dropPrefab);
        dropItem.transform.position = dropPosition.position;
        dropItem.transform.rotation = Quaternion.identity;
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360f));
    }

    // 인벤토리창에서 아이템 선택 취소하기
    private void ClearSeletecItemWindow()
    {
        // 아이템 선택안함 표현
        selectedItem = null;
        inventoryUI.SelectedItemUI_Update(selectedItem);
    }

    // 아이템 착용
    public bool OnEquip(WeaponStat weaponStat)
    {
        return Player.Instance.playerCharacter.InventoryWeaponEquip(weaponStat);
    }

    // 아이템 착용해제
    public void UnEquip(int index)
    {
        WeaponSlotUI unEquipItem;

        switch (index)
        {
            case 1:
                unEquipItem = inventoryUI.weaponSlotUI1;
                Player.Instance.playerCharacter.InventoryWeaponUnequip(true);
                break;
            case 2:
                unEquipItem = inventoryUI.weaponSlotUI2;
                Player.Instance.playerCharacter.InventoryWeaponUnequip(false);
                break;
            default:
                unEquipItem = null;
                break;
        }

        if(unEquipItem)
        {
            AddItem(unEquipItem.weaponItemData, unEquipItem.weaponData.baseStat);
        }
    }

    // 아이템 버림
    public void OnDrop()
    {
        ThrowItem(selectedItem.data); // 선택아이템 아이템 버림
        RemoveSelectedItem(); // 선택아이템 인벤토리에서 삭제
    }


    // 선택한 아이템 삭제
    private void RemoveSelectedItem()
    {
        /* 장비하고 있는 아이템은 인벤토리 내에 없도록 할 예정이라 해당 코드는 보류.
        if (uiSlots[selectedItemIndex].equipped)
        {
            UnEquip(selectedItemIndex);
        }
        */
        selectedItem.data.displayName = null;
        /*
        foreach (string key in selectedItem.data.itemStatValues.Keys)
            selectedItem.data.itemStatValues[key] = 0;
        */
        ClearSeletecItemWindow();

        inventoryUI.UpdateSlotUI();
    }
    // 아이템 삭제
    public void RemoveItem(ItemData item)
    {

    }

    public bool HasItems(ItemData item, int quantity)
    {
        return false;
    }
    public void InventoryWindowClear()
    {

    }
}