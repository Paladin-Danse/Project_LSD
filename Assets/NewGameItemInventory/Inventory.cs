using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
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
    public ItemData item;
    public int index;

    public ItemSlot(int _index)
    {
        index = _index;
        slotUI = new ItemSlotUI(_index);
    }
}

// 인벤토리를 표현하기 위한 클래스
public class Inventory : MonoBehaviour, IObjectCrash
{
    public ItemSlotUI[] uiSlots; // ui 슬롯
    public ItemSlot[] slots; // item 슬롯

    public InventoryData inventorySO;
    public GameObject inventoryWindow; // 인벤토리 켜기
    public Transform dropPosition; // 아이템 버리기 위치

    [Header("Selected Item")]
    private ItemSlot selectedItem; // 선택한 아이템
    private int selectedItemIndex; // 선택한 아이템 인덱스
    public TextMeshProUGUI selectedItemName; // 이름
    public TextMeshProUGUI selectedItemDescription; // 설명
    public TextMeshProUGUI selectedItemStatNames; // 스텟
    public TextMeshProUGUI selectedItemStatValues; // 스텟 값
    public GameObject equipButton; // 착용 버튼
    public GameObject unEquipButton; // 해제 버튼
    public GameObject dropButton; // 버림 버튼

    private Dictionary<AmmoType, int> inventoryAmmo;
    public TextMeshProUGUI rifleAmmoCountText;
    public TextMeshProUGUI pistolAmmoCountText;

    public WeaponSlotUI weaponSlotUI1;
    public WeaponSlotUI weaponSlotUI2;

    private int curEquipIndex; // ???

    private PlayerCharacter character;

    [Header("Events")]
    public UnityEvent onOpenInventory; // 인벤토리 오픈 이벤트
    public UnityEvent onCloseInventory; // 인벤토리 닫기 이벤트

    void Awake()
    {
        inventoryWindow = new Inventory().gameObject;
        character = GetComponent<PlayerCharacter>();
        slots = new ItemSlot[uiSlots.Length]; // 아이템 슬롯
        inventorySO.init();
        inventoryAmmo = new Dictionary<AmmoType, int>();

        foreach (AmmoType ammo in Enum.GetValues(typeof(AmmoType)))
            inventoryAmmo.Add(ammo, 0);
    }

    private void Start()
    {
        inventoryWindow.SetActive(false); // 인벤토리 꺼두기
        

        for (int i = 0; i < slots.Length; i++) // 슬롯 초기화
        {
            slots[i] = new ItemSlot(i);
            uiSlots[i] = new ItemSlotUI(i);
            uiSlots[i].Clear();
        }
        foreach(AmmoType ammo in Enum.GetValues(typeof(AmmoType)))
        {
            inventoryAmmo[ammo] = inventorySO.maxAmmo[ammo];
        }
        ClearSeletecItemWindow();
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
        if (inventoryWindow.activeInHierarchy) // 하이어라키상에서 켜져있나?
        {
            inventoryWindow.SetActive(false); // 인벤토리창을 끈다
            onCloseInventory?.Invoke(); // 인벤토리창 끄기 인보크
            //controller.ToggleCursor(false); // 컨트롤러.커서 잠금
            Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
        }
        else
        {
            UpdateAmmoUI();
            weaponSlotUI1.nameText.text = character.primaryWeapon.name;
            weaponSlotUI2.nameText.text = character.secondaryWeapon.name;
            inventoryWindow.SetActive(true);
            onOpenInventory?.Invoke();
            //controller.ToggleCursor(true);
            Cursor.lockState = CursorLockMode.Confined; // 커서 잠금해제
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    [System.Obsolete]
    public void TakeAmmoItemColliderCrash(AmmoType ammoType, int count)
    {
        inventoryAmmo[ammoType] += count;
        if (inventoryAmmo[ammoType] > inventorySO.maxAmmo[ammoType]) inventoryAmmo[ammoType] = inventorySO.maxAmmo[ammoType];

        if (inventoryWindow.active) // 인벤토리를 열고있다면
        {
            UpdateAmmoUI();
        }
    }
    public int LostorUsedAmmo(AmmoType ammoType, int count)
    {
        int leftAmmo = inventoryAmmo[ammoType] < count ? inventoryAmmo[ammoType] : count;
        inventoryAmmo[ammoType] -= leftAmmo;

        if (inventoryWindow.activeSelf) // 인벤토리를 열고있다면
        {
            UpdateAmmoUI();
        }
        return leftAmmo;
    }
    public int InventoryAmmoCheck(AmmoType ammoType)
    {
        return inventoryAmmo[ammoType];
    }

    void UpdateAmmoUI()
    {
        rifleAmmoCountText.text = inventoryAmmo[AmmoType.Rifle].ToString();
        pistolAmmoCountText.text = inventoryAmmo[AmmoType.Pistol].ToString();
    }

    public void AddWeapon(Weapon weapon)
    {
        character.primaryWeapon = weapon;
    }

    // 인벤토리에 아이템 추가
    public void AddItem(ItemData item)
    {
        ItemSlot emptySlot = GetEmptySlot(); // 비어있는 아이템 슬롯 찾기

        if (emptySlot != null) // 비어있는 슬롯이 있다면
        {
            emptySlot.item = item;
            UpdateUI();
            return;
        }

        ThrowItem(item); // 인벤토리가 꽉 찼다면 버리기
    }

    // 아이템 버림
    void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360f));
    }

    // 인벤토리창 업데이트
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null) // 슬롯에 아이템이 있다면
                uiSlots[i].Set(slots[i]); // 세팅
            else
                uiSlots[i].Clear(); // 없다면 클리어
        }
    }

    // 비어있는 아이템 슬롯 가져오기
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }

        return null;
    }

    // 인덱스로 인벤토리창에서 아이템 가져오기
    public void SelectItem(int index)
    {
        if (slots[index].item == null)
            return;
        // 선택한 아이템 변수에 대입
        selectedItem = slots[index];
        selectedItemIndex = index;
        // 아이템 이름, 정보 표시
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
        // 아이템 스텟 초기화
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        
        // 버튼 표시
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    // 인벤토리창에서 아이템 선택 취소하기
    private void ClearSeletecItemWindow()
    {
        // 아이템 선택안함 표현
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        // 아이템 스탯 표시 제거
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        // 버튼 표시 끄기
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // 아이템 착용 버튼
    public void OnEquipButton()
    {

    }

    // 아이템 착용해제
    void UnEquip(int index)
    {

    }

    // 아이템 착용해제 버튼
    public void OnUnEquipButton()
    {

    }

    // 아이템 버림 버튼
    public void OnDropButton()
    {
        ThrowItem(selectedItem.item); // 선택아이템 아이템 버림
        RemoveSelectedItem(); // 선택아이템 인벤토리에서 삭제
    }

    // 선택한 아이템 삭제
    private void RemoveSelectedItem()
    {
        if (uiSlots[selectedItemIndex].equipped)
        {
            UnEquip(selectedItemIndex);
        }

        selectedItem.item = null;
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