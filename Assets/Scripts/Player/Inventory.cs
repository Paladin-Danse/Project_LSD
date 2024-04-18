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
        inventoryUI.gameObject.SetActive(false); // �κ��丮 ���α�
    }

    [System.Obsolete]
    public void TakeAmmoItemColliderCrash(AmmoType ammoType, int count)
    {
        inventoryAmmo[ammoType] += count;
        if (inventoryAmmo[ammoType] > inventorySO.maxAmmo[ammoType]) inventoryAmmo[ammoType] = inventorySO.maxAmmo[ammoType];

        if (inventoryUI.gameObject.active) // �κ��丮�� �����ִٸ�
        {
            inventoryUI.AmmoUI_Update();
        }
    }
    public int LostorUsedAmmo(AmmoType ammoType, int count)
    {
        int leftAmmo = inventoryAmmo[ammoType] < count ? inventoryAmmo[ammoType] : count;
        inventoryAmmo[ammoType] -= leftAmmo;

        if (inventoryUI.gameObject.activeSelf) // �κ��丮�� �����ִٸ�
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

    // �κ��丮�� ������ �߰�
    public void AddItem(ItemData item)
    {
        ItemSlot emptySlot = inventoryUI.GetEmptySlot(); // ����ִ� ������ ���� ã��

        if (emptySlot != null) // ����ִ� ������ �ִٸ�
        {
            emptySlot.data = item;
            inventoryUI.UpdateSlotUI();
            return;
        }

        ThrowItem(item); // �κ��丮�� �� á�ٸ� ������
    }

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
    public void OnEquip()
    {

    }

    // ������ ��������
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