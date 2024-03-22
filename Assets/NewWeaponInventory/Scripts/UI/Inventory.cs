using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

// 아이템 슬롯 클래스
public class ItemSlot
{
    public ItemData item;
    public int quantity;
}

// 무기 슬롯 클래스
public class WeaponSlot
{
    public ItemWeaponData item;
}

// 인벤토리를 표현하기 위한 클래스
public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots; // ui 슬롯
    public ItemSlot[] slots; // item 슬롯

    public WeaponSlotUI[] weaponsUiSlots; // ui 슬롯
    public WeaponSlot[] weaponsSlots; // weapon 슬롯

    public GameObject inventoryWindow; // 인벤토리 켜기
    public Transform dropPosition; // 아이템 버리기 위치

    [Header("Selected Item")]
    private ItemSlot selectedItem; // 선택한 아이템
    private int selectedItemIndex; // 선택한 아이템 인덱스

    [Header("Selected Weapons")]
    private WeaponSlot selectedWeapon; // 선택한 아이템
    private int selectedWeaponsIndex; // 선택한 아이템 인덱스

    [Header("Selected Info")]
    public TextMeshProUGUI selectedItemName; // 이름
    public TextMeshProUGUI selectedItemDescription; // 설명
    public TextMeshProUGUI selectedItemStatNames; // 스텟
    public TextMeshProUGUI selectedItemStatValues; // 스텟 값
    public GameObject useButton; // 사용 버튼
    public GameObject equipButton; // 착용 버튼
    public GameObject unEquipButton; // 해제 버튼
    public GameObject dropButton; // 버림 버튼

    private int curEquipIndex; // ???

    private PlayerController controller; // 플레이어 컨트롤러
    private PlayerConditions condition; // 플레이어 상황

    [Header("Events")]
    public UnityEvent onOpenInventory; // 인벤토리 오픈 이벤트
    public UnityEvent onCloseInventory; // 인벤토리 닫기 이벤트

    public static Inventory instance; // 인벤토리 싱글톤 패턴
    void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerConditions>();
    }

    private void Start()
    {
        inventoryWindow.SetActive(false); // 인벤토리 꺼두기
        slots = new ItemSlot[uiSlots.Length]; // 아이템 슬롯
        weaponsSlots = new WeaponSlot[weaponsUiSlots.Length];

        for (int i = 0; i < slots.Length; i++) // 슬롯 초기화
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i;
            uiSlots[i].Clear();
        }
        for (int i = 0; i < weaponsSlots.Length; i++)
        {
            weaponsSlots[i] = new WeaponSlot();
            weaponsUiSlots[i].index = i;
            weaponsUiSlots[i].Clear();
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

    // 인벤토리에 아이템 추가
    public void AddItem(ItemData item)
    {
        if (item.canStack)
        {
            ItemSlot slotToStackTo = GetItemStack(item);
            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot(); // 비어있는 아이템 슬롯 찾기

        if (emptySlot != null) // 비어있는 슬롯이 있다면
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        ThrowItem(item); // 인벤토리가 꽉 찼다면 버리기
    }

    // 무기 줍기
    public void PickUpItem(ItemWeaponData item)
    {
        WeaponSlot emptySlot = GetEmptyWeaponSlot(); // 비어있는 무기 슬롯 찾기

        if (emptySlot != null) // 비어있는 슬롯이 있다면
        {
            emptySlot.item = item;
            UpdateUI();
            return;
        }

        ThrowWeapon(item);
    }

    // 아이템 버림
    void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    // 무기 버림
    void ThrowWeapon(ItemWeaponData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
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

        for (int i = 0; i < weaponsSlots.Length; i++)
        {
            if (weaponsSlots[i].item != null) // 슬롯에 아이템이 있다면
                weaponsUiSlots[i].Set(weaponsSlots[i]); // 세팅
            else
                weaponsUiSlots[i].Clear(); // 없다면 클리어
        }
    }

    // 해당 아이템 개수 세기
    ItemSlot GetItemStack(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount)
                return slots[i];
        }

        return null;
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


    // 비어있는 아이템 슬롯 가져오기
    WeaponSlot GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weaponsSlots.Length; i++)
        {
            if (weaponsSlots[i].item == null)
                return weaponsSlots[i];
        }

        return null;
    }

    // 인덱스로 인벤토리창에서 아이템 가져오기
    public void SelectItem(int index)
    {
        if (slots[index] == null)
            return;
        selectedWeapon = null;
        // 선택한 아이템 변수에 대입
        selectedItem = slots[index];
        selectedItemIndex = index;
        // 아이템 이름, 정보 표시
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
        // 아이템 스텟 초기화
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        // 아이템 스텟 표시
        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatNames.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValues.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }
        // 버튼 표시
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    // 인덱스로 무기 선택
    public void SelectWeapon(int index)
    {
        if (weaponsSlots[index] == null)
            return;
        selectedItem = null;
        // 선택한 아이템 변수에 대입
        selectedWeapon = weaponsSlots[index];
        selectedWeaponsIndex = index;
        // 아이템 이름, 정보 표시
        selectedItemName.text = selectedWeapon.item.displayName;
        selectedItemDescription.text = selectedWeapon.item.description;
        // 아이템 스텟 초기화
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;

        // 버튼 표시
        useButton.SetActive(selectedWeapon.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedWeapon.item.type == ItemType.Equipable && !weaponsUiSlots[index].equipped);
        unEquipButton.SetActive(selectedWeapon.item.type == ItemType.Equipable && weaponsUiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    // 인벤토리창에서 아이템 선택 취소하기
    private void ClearSeletecItemWindow()
    {
        // 아이템 선택안함 표현
        selectedItem = null;
        selectedWeapon = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        // 아이템 스탯 표시 제거
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        // 버튼 표시 끄기
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // 아이템 사용버튼
    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].type)
                {
                    //case ConsumableType.Health:
                    //    condition.Heal(selectedItem.item.consumables[i].value); break;
                    //case ConsumableType.Hunger:
                    //    condition.Eat(selectedItem.item.consumables[i].value); break;
                }
            }
        }
        RemoveSelectedItem();
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
        if (selectedItem != null)
            ThrowItem(selectedItem.item); // 선택아이템 버림
        else if (selectedWeapon != null)
            ThrowWeapon(selectedWeapon.item); // 선택무기 버림
        RemoveSelectedItem(); // 선택아이템 인벤토리에서 삭제
    }

    // 선택한 아이템 삭제
    private void RemoveSelectedItem()
    {
        if (selectedItem != null)
        {
            selectedItem.quantity--;

            if (selectedItem.quantity <= 0)
            {
                if (uiSlots[selectedItemIndex].equipped)
                {
                    UnEquip(selectedItemIndex);
                }

                selectedItem.item = null;
                ClearSeletecItemWindow();
            }
        }
        else if (selectedWeapon != null)
        {
            // 선택한 무기 인벤토리에서 삭제
            Debug.Log("코드 구현 필요:선택한 무기 인벤토리에서 삭제");
        }

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
}