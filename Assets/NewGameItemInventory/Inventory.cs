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

    public ItemSlot(int _index, ItemSlotUI _slotUI)
    {
        index = _index;
        slotUI = _slotUI;
        slotUI.Init(this);
    }
    public void OnClick()
    {
        //itemEquip
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
    public ItemSlotUI uiSlot; // ui 슬롯
    public ItemSlot[] slots; // item 슬롯
    public Transform slotParent;
    
    public InventoryData inventorySO;
    public GameObject inventoryWindow; // 인벤토리 켜기
    public Transform dropPosition; // 아이템 버리기 위치

    [Header("Selected Item")]
    private ItemSlot selectedItem; // 선택한 아이템
    private int selectedItemIndex; // 선택한 아이템 인덱스
    public TextMeshProUGUI selectedItemName; // 이름
    //public TextMeshProUGUI selectedItemDescription; // 설명
    //public TextMeshProUGUI selectedItemStatNames; // 스텟
    public Dictionary<string, TextMeshProUGUI> selectedItemStatValues; // 스텟 값
    //public GameObject equipButton; // 착용 버튼
    //public GameObject unEquipButton; // 해제 버튼
    //public GameObject dropButton; // 버림 버튼

    private Dictionary<AmmoType, int> inventoryAmmo;
    public TextMeshProUGUI rifleAmmoCountText;
    public TextMeshProUGUI pistolAmmoCountText;

    public WeaponSlotUI weaponSlotUI1;
    public WeaponSlotUI weaponSlotUI2;

    private int curEquipIndex; // ???
    public int itemSlotCount;

    private PlayerCharacter character;

    [Header("Events")]
    public UnityEvent onOpenInventory; // 인벤토리 오픈 이벤트
    public UnityEvent onCloseInventory; // 인벤토리 닫기 이벤트

    void Awake()
    {
        character = GetComponent<PlayerCharacter>();
        slots = new ItemSlot[itemSlotCount]; // 아이템 슬롯
        slotParent = inventoryWindow.transform.Find("Inventory/Scroll View/Viewport/Slots");

        inventorySO.init();
        inventoryAmmo = new Dictionary<AmmoType, int>();
        
        foreach (AmmoType ammo in Enum.GetValues(typeof(AmmoType)))
            inventoryAmmo.Add(ammo, 0);
    }

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++) // 슬롯 초기화
        {
            slots[i] = new ItemSlot(i, Instantiate(uiSlot, slotParent));
            slots[i].Clear();
        }
        foreach(AmmoType ammo in Enum.GetValues(typeof(AmmoType)))
        {
            inventoryAmmo[ammo] = inventorySO.maxAmmo[ammo];
        }

        GameObject itemName = inventoryWindow.transform.Find("Inventory/InfoBG/ItemName").gameObject;
        selectedItemName = itemName?.GetComponent<TextMeshProUGUI>();
        GameObject itemStatValues = inventoryWindow.transform.Find("Inventory/InfoBG/ItemStatValues").gameObject;
        selectedItemStatValues = new Dictionary<string, TextMeshProUGUI>();
        for(int i = 0; i < inventorySO.itemStatValues.Count; i++)
        {
            GameObject textObj = itemStatValues.transform.Find($"{inventorySO.statNames[i]}/StatValue").gameObject;
            selectedItemStatValues.Add(inventorySO.statNames[i], textObj?.GetComponent<TextMeshProUGUI>());
        }

        ClearSeletecItemWindow();
        inventoryWindow.SetActive(false); // 인벤토리 꺼두기
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
            if(character.primaryWeapon != null)
            {
                weaponSlotUI1.nameText.text = character.primaryWeapon.name;
            }
            if(character.secondaryWeapon != null)
            {
                weaponSlotUI2.nameText.text = character.secondaryWeapon.name;
            }
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
            if (slots[i].item != null) // 슬롯에 아이템이 있다면
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
        //selectedItemDescription.text = selectedItem.item.description;
        // 아이템 스텟 초기화
        //selectedItemStatNames.text = string.Empty;
        foreach (TextMeshProUGUI StatValueText in selectedItemStatValues.Values)
        {
            StatValueText.text = string.Empty;
        }
    }

    // 인벤토리창에서 아이템 선택 취소하기
    private void ClearSeletecItemWindow()
    {
        // 아이템 선택안함 표현
        selectedItem = null;
        selectedItemName.text = string.Empty;
        //selectedItemDescription.text = string.Empty;
        // 아이템 스탯 표시 제거
        //selectedItemStatNames.text = string.Empty;
        foreach(TextMeshProUGUI StatValueText in selectedItemStatValues.Values)
        {
            StatValueText.text = string.Empty;
        }
    }

    // 아이템 착용
    public void OnEquip()
    {
        
    }

    // 아이템 착용해제
    void UnEquip(int index)
    {

    }

    // 아이템 버림
    public void OnDrop()
    {
        ThrowItem(selectedItem.item); // 선택아이템 아이템 버림
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