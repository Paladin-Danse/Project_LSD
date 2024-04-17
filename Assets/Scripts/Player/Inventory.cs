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
        //장착하면 해당 아이템은 인벤토리에 사라지고 장비슬롯에 등장.
        slotUI.Clear();
    }
}

// 인벤토리를 표현하기 위한 클래스
public class Inventory : MonoBehaviour, IObjectCrash
{
    public InventoryUI inventoryUI;

    public ItemSlot[] slots; // item 슬롯
    public Transform slotParent;

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

        slots = new ItemSlot[inventorySO.itemSlotCount]; // 아이템 슬롯
        slotParent = inventoryUI.inventoryWindow.transform.Find("Scroll View/Viewport/Slots");
    }

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++) // 슬롯 초기화
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
        inventoryUI.inventoryWindow.SetActive(false); // 인벤토리 꺼두기
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            // Toggle();
        }
    }

    // 인벤토리 열고 닫기 토글
    public void Toggle(InputAction.CallbackContext callbackContext)
    {
        if (inventoryUI.inventoryWindow.activeInHierarchy) // 하이어라키상에서 켜져있나?
        {
            inventoryUI.inventoryWindow.SetActive(false); // 인벤토리창을 끈다
            //controller.ToggleCursor(false); // 컨트롤러.커서 잠금
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

        if (inventoryUI.inventoryWindow.active) // 인벤토리를 열고있다면
        {
            inventoryUI.AmmoUI_Update();
        }
    }
    public int LostorUsedAmmo(AmmoType ammoType, int count)
    {
        int leftAmmo = inventoryAmmo[ammoType] < count ? inventoryAmmo[ammoType] : count;
        inventoryAmmo[ammoType] -= leftAmmo;

        if (inventoryUI.inventoryWindow.activeSelf) // 인벤토리를 열고있다면
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
        ItemSlot emptySlot = GetEmptySlot(); // 비어있는 아이템 슬롯 찾기

        if (emptySlot != null) // 비어있는 슬롯이 있다면
        {
            emptySlot.data = item;
            UpdateUI();
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

    // 인벤토리창 업데이트
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data != null) // 슬롯에 아이템이 있다면
                slots[i].Set(); // 세팅
            else
                slots[i].Clear(); // 없다면 클리어
        }
    }

    // 비어있는 아이템 슬롯 가져오기
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isEmpty)
                return slots[i];
        }

        return null;
    }

    // 인덱스로 인벤토리창에서 아이템 가져오기
    public void SelectItem(int index)
    {
        if (slots[index].isEmpty)
            return;
        // 선택한 아이템 변수에 대입
        selectedItem = slots[index];
        selectedItemIndex = index;
        // 아이템 이름, 정보 표시
        inventoryUI.SelectedItemUI_Update(selectedItem);
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

        UpdateUI();
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