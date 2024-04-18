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
using static UnityEditor.Progress;

public interface IObjectCrash
{
    void TakeAmmoItemColliderCrash(AmmoType ammoType, int count);
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
    //public PlayerCharacter character{ get; private set; }

    void Awake()
    {
        inventorySO.init();
        inventoryAmmo = new Dictionary<AmmoType, int>();
        _playerMaxMoney = inventorySO.maxMoney;

        foreach (AmmoType ammo in Enum.GetValues(typeof(AmmoType)))
            inventoryAmmo.Add(ammo, 0);

        //Init(inventoryUI);
    }

    public void Init(InventoryUI _inventoryUI)
    {
        Debug.Log(inventoryUI);
        inventoryUI.Init(this);

        dropPosition = Player.Instance.playerCharacter.transform.Find("DropPosition");
    }

    private void Start()
    {
        foreach (AmmoType ammo in Enum.GetValues(typeof(AmmoType)))
        {
            if(ammo != AmmoType.None)
                inventoryAmmo[ammo] = inventorySO.maxAmmo[ammo];
        }

        money += inventorySO.playerStartMoney;
        //ClearSeletecItemWindow();
        inventoryUI.gameObject.SetActive(false); // 인벤토리 꺼두기
    }

    [System.Obsolete]
    public void TakeAmmoItemColliderCrash(AmmoType ammoType, int count)
    {
        inventoryAmmo[ammoType] += count;
        if (inventoryAmmo[ammoType] > inventorySO.maxAmmo[ammoType]) inventoryAmmo[ammoType] = inventorySO.maxAmmo[ammoType];

        if (inventoryUI.gameObject.active) // 인벤토리를 열고있다면
        {
            inventoryUI.AmmoUI_Update();
        }
    }
    public int LostorUsedAmmo(AmmoType ammoType, int count)
    {
        int leftAmmo = inventoryAmmo[ammoType] < count ? inventoryAmmo[ammoType] : count;
        inventoryAmmo[ammoType] -= leftAmmo;

        if (inventoryUI.gameObject.activeSelf) // 인벤토리를 열고있다면
        {
            inventoryUI.AmmoUI_Update();
        }
        return leftAmmo;
    }
    public int InventoryAmmoCheck(AmmoType ammoType)
    {
        return inventoryAmmo[ammoType];
    }



    public void AddWeapon(Weapon weapon)
    {
        if (Player.Instance.playerCharacter.primaryWeapon == null || Player.Instance.playerCharacter.primaryWeapon == null)
        {
            Player.Instance.playerCharacter.InventoryWeaponEquip(weapon);
        }
        else
        {
            AddItem(weapon.itemData);
        }
    }

    // 인벤토리에 아이템 추가
    public void AddItem(ItemData item)
    {
        ItemSlot emptySlot = inventoryUI.GetEmptySlot(); // 비어있는 아이템 슬롯 찾기

        if (emptySlot != null) // 비어있는 슬롯이 있다면
        {
            emptySlot.data = item;
            inventoryUI.UpdateSlotUI();
            return;
        }

        ThrowItem(item); // 인벤토리가 꽉 찼다면 버리기
    }

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
    public void OnEquip()
    {

    }

    // 아이템 착용해제
    public void UnEquip(int index)
    {
        ItemData unEquipItem;

        switch (index)
        {
            case 1:
                unEquipItem = inventoryUI.weaponSlotUI1.weaponItemData;
                Player.Instance.playerCharacter.InventoryWeaponUnequip(true);
                break;
            case 2:
                unEquipItem = inventoryUI.weaponSlotUI2.weaponItemData;
                Player.Instance.playerCharacter.InventoryWeaponUnequip(false);
                break;
            default:
                unEquipItem = null;
                break;
        }

        if(unEquipItem)
        {
            AddItem(unEquipItem);
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