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
    public ItemSlotUI slotUI;
    public ItemData data;
    public GameObject itemObject;
    public int index;
    public bool isEmpty
    {
        get { return data.displayName == null; }
    }
    public ItemSlot(int _index, ItemSlotUI _slotUI)
    {
        index = _index;
        slotUI = _slotUI;
        slotUI.Init(this);
        data = new ItemData();
        //data.itemStatValues = new Dictionary<string, int>();
    }
    public void OnClick()
    {
        Weapon weapon;
        //Equipment
        
        switch (data.type)
        {
            case ItemType.Weapon:
                Player.Instance.playerCharacter.InventoryWeaponEquip(new Weapon(data));
                break;
            case ItemType.Equipable:
                break;
            default:
                break;
        }
        
    }
    public void Set()
    {
        slotUI.Set(this);
    }
    public void Clear()
    {
        slotUI.Clear();
    }
    public void Equip()
    {
        //�����ϸ� �ش� �������� �κ��丮�� ������� ��񽽷Կ� ����.
        slotUI.Clear();
    }
}

// �κ��丮�� ǥ���ϱ� ���� Ŭ����
public class Inventory : MonoBehaviour, IObjectCrash
{
    public InventoryUI inventoryUI;

    public ItemSlot[] slots; // item ����
    public Transform slotParent;

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
            inventoryUI.MoneyUI_Update();
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

        inventoryUI = GetComponent<InventoryUI>();
        Debug.Log(inventoryUI);
        inventoryUI.Init(this);

        slots = new ItemSlot[inventorySO.itemSlotCount]; // ������ ����
        slotParent = inventoryUI.inventoryWindow.transform.Find("Scroll View/Viewport/Slots");
    }

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++) // ���� �ʱ�ȭ
        {
            slots[i] = new ItemSlot(i, Instantiate(inventoryUI.uiSlot, slotParent));
            slots[i].data.Init(inventorySO);
            slots[i].Clear();
        }
        foreach (AmmoType ammo in Enum.GetValues(typeof(AmmoType)))
        {
            if(ammo != AmmoType.None)
                inventoryAmmo[ammo] = inventorySO.maxAmmo[ammo];
        }

        money += inventorySO.playerStartMoney;
        //ClearSeletecItemWindow();
        inventoryUI.inventoryWindow.SetActive(false); // �κ��丮 ���α�
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            // Toggle();
        }
    }

    // �κ��丮 ���� �ݱ� ���
    public void Toggle(InputAction.CallbackContext callbackContext)
    {
        if (inventoryUI.inventoryWindow.activeInHierarchy) // ���̾��Ű�󿡼� �����ֳ�?
        {
            inventoryUI.inventoryWindow.SetActive(false); // �κ��丮â�� ����
            //controller.ToggleCursor(false); // ��Ʈ�ѷ�.Ŀ�� ���
            Player.Instance.playerCharacter.input.SetCursorLock(true);
        }
        else
        {

            inventoryUI.inventoryWindow.SetActive(true);
            inventoryUI.UI_All_Update?.Invoke();
            //controller.ToggleCursor(true);
            Player.Instance.playerCharacter.input.SetCursorLock(false);
        }
    }

    public bool IsOpen()
    {
        return inventoryUI.inventoryWindow.activeInHierarchy;
    }

    [System.Obsolete]
    public void TakeAmmoItemColliderCrash(AmmoType ammoType, int count)
    {
        inventoryAmmo[ammoType] += count;
        if (inventoryAmmo[ammoType] > inventorySO.maxAmmo[ammoType]) inventoryAmmo[ammoType] = inventorySO.maxAmmo[ammoType];

        if (inventoryUI.inventoryWindow.active) // �κ��丮�� �����ִٸ�
        {
            inventoryUI.AmmoUI_Update();
        }
    }
    public int LostorUsedAmmo(AmmoType ammoType, int count)
    {
        int leftAmmo = inventoryAmmo[ammoType] < count ? inventoryAmmo[ammoType] : count;
        inventoryAmmo[ammoType] -= leftAmmo;

        if (inventoryUI.inventoryWindow.activeSelf) // �κ��丮�� �����ִٸ�
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
        ItemSlot emptySlot = GetEmptySlot(); // ����ִ� ������ ���� ã��

        if (emptySlot != null) // ����ִ� ������ �ִٸ�
        {
            emptySlot.data = item;
            UpdateUI();
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

    // �κ��丮â ������Ʈ
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data != null) // ���Կ� �������� �ִٸ�
                slots[i].Set(); // ����
            else
                slots[i].Clear(); // ���ٸ� Ŭ����
        }
    }

    // ����ִ� ������ ���� ��������
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isEmpty)
                return slots[i];
        }

        return null;
    }

    // �ε����� �κ��丮â���� ������ ��������
    public void SelectItem(int index)
    {
        if (slots[index].isEmpty)
            return;
        // ������ ������ ������ ����
        selectedItem = slots[index];
        selectedItemIndex = index;
        // ������ �̸�, ���� ǥ��
        inventoryUI.SelectedItemUI_Update(selectedItem);
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

        UpdateUI();
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