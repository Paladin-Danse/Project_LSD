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

// ������ ���� Ŭ����
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
        //�����ϸ� �ش� �������� �κ��丮�� ������� ��񽽷Կ� ����.
    }
}

// �κ��丮�� ǥ���ϱ� ���� Ŭ����
public class Inventory : MonoBehaviour, IObjectCrash
{
    public InventoryUI inventoryUI;

    public InventoryData inventorySO;
    public Transform dropPosition; // ������ ������ ��ġ

    public Dictionary<AmmoType, int> inventoryAmmo { get; private set; }

    [Header("Selected Item")]
    private ItemSlot selectedItem = null; // ������ ������
    private int selectedItemIndex; // ������ ������ �ε���

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
        if (inventoryUI.gameObject.active) // �κ��丮�� �����ִٸ�
        {
            inventoryUI.Inventory_AmmoUI_Update();
        }
    }
    public int LostorUsedAmmo(AmmoType ammoType, int count)
    {
        int leftAmmo = inventoryAmmo[ammoType] < count ? inventoryAmmo[ammoType] : count;
        inventoryAmmo[ammoType] -= leftAmmo;

        OnAmmoChanged?.Invoke();
        if (inventoryUI.gameObject.activeSelf) // �κ��丮�� �����ִٸ�
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

    // �κ��丮�� ������ �߰�
    public void AddItem(ItemData itemdata, WeaponStat _weaponStat)
    {
        ItemSlotUI emptySlot = inventoryUI.GetEmptySlot(); // ����ִ� ������ ���� ã��

        if (emptySlot != null) // ����ִ� ������ �ִٸ�
        {
            emptySlot.Set(itemdata, _weaponStat);
            inventoryUI.UpdateSlotUI();
            return;
        }

        ThrowItem(itemdata); // �κ��丮�� �� á�ٸ� ������
    }
    //public void AddItem(ItemData item, Equipment equipment)

    // ������ ����
    void ThrowItem(ItemData item)
    {
        Poolable dropItem = ObjectPoolManager.Instance.Pop(item.dropPrefab);
        dropItem.transform.position = dropPosition.position;
        dropItem.transform.rotation = Quaternion.identity;
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360f));
    }

    // �κ��丮â���� ������ ���� ����ϱ�
    private void ClearSeletecItemWindow()
    {
        // ������ ���þ��� ǥ��
        selectedItem = null;
        inventoryUI.SelectedItemUI_Update(selectedItem);
    }

    // ������ ����
    public bool OnEquip(WeaponStat weaponStat)
    {
        return Player.Instance.playerCharacter.InventoryWeaponEquip(weaponStat);
    }

    // ������ ��������
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

    // ������ ����
    public void OnDrop()
    {
        ThrowItem(selectedItem.data); // ���þ����� ������ ����
        RemoveSelectedItem(); // ���þ����� �κ��丮���� ����
    }


    // ������ ������ ����
    private void RemoveSelectedItem()
    {
        /* ����ϰ� �ִ� �������� �κ��丮 ���� ������ �� �����̶� �ش� �ڵ�� ����.
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
    // ������ ����
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