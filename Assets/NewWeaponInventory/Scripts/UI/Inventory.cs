using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

// ������ ���� Ŭ����
public class ItemSlot
{
    public ItemData item;
    public int quantity;
}

// ���� ���� Ŭ����
public class WeaponSlot
{
    public ItemWeaponData item;
}

// �κ��丮�� ǥ���ϱ� ���� Ŭ����
public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots; // ui ����
    public ItemSlot[] slots; // item ����

    public WeaponSlotUI[] weaponsUiSlots; // ui ����
    public WeaponSlot[] weaponsSlots; // weapon ����

    public GameObject inventoryWindow; // �κ��丮 �ѱ�
    public Transform dropPosition; // ������ ������ ��ġ

    [Header("Selected Item")]
    private ItemSlot selectedItem; // ������ ������
    private int selectedItemIndex; // ������ ������ �ε���

    [Header("Selected Weapons")]
    private WeaponSlot selectedWeapon; // ������ ������
    private int selectedWeaponsIndex; // ������ ������ �ε���

    [Header("Selected Info")]
    public TextMeshProUGUI selectedItemName; // �̸�
    public TextMeshProUGUI selectedItemDescription; // ����
    public TextMeshProUGUI selectedItemStatNames; // ����
    public TextMeshProUGUI selectedItemStatValues; // ���� ��
    public GameObject useButton; // ��� ��ư
    public GameObject equipButton; // ���� ��ư
    public GameObject unEquipButton; // ���� ��ư
    public GameObject dropButton; // ���� ��ư

    private int curEquipIndex; // ???

    private PlayerController controller; // �÷��̾� ��Ʈ�ѷ�
    private PlayerConditions condition; // �÷��̾� ��Ȳ

    [Header("Events")]
    public UnityEvent onOpenInventory; // �κ��丮 ���� �̺�Ʈ
    public UnityEvent onCloseInventory; // �κ��丮 �ݱ� �̺�Ʈ

    public static Inventory instance; // �κ��丮 �̱��� ����
    void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerConditions>();
    }

    private void Start()
    {
        inventoryWindow.SetActive(false); // �κ��丮 ���α�
        slots = new ItemSlot[uiSlots.Length]; // ������ ����
        weaponsSlots = new WeaponSlot[weaponsUiSlots.Length];

        for (int i = 0; i < slots.Length; i++) // ���� �ʱ�ȭ
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

    // �κ��丮 ���� �ݱ� ���
    public void Toggle(InputAction.CallbackContext callbackContext)
    {
        if (inventoryWindow.activeInHierarchy) // ���̾��Ű�󿡼� �����ֳ�?
        {
            inventoryWindow.SetActive(false); // �κ��丮â�� ����
            onCloseInventory?.Invoke(); // �κ��丮â ���� �κ�ũ
            //controller.ToggleCursor(false); // ��Ʈ�ѷ�.Ŀ�� ���
            Cursor.lockState = CursorLockMode.Locked; // Ŀ�� ���
        }
        else
        {
            inventoryWindow.SetActive(true);
            onOpenInventory?.Invoke();
            //controller.ToggleCursor(true);
            Cursor.lockState = CursorLockMode.Confined; // Ŀ�� �������
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    // �κ��丮�� ������ �߰�
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

        ItemSlot emptySlot = GetEmptySlot(); // ����ִ� ������ ���� ã��

        if (emptySlot != null) // ����ִ� ������ �ִٸ�
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        ThrowItem(item); // �κ��丮�� �� á�ٸ� ������
    }

    // ���� �ݱ�
    public void PickUpItem(ItemWeaponData item)
    {
        WeaponSlot emptySlot = GetEmptyWeaponSlot(); // ����ִ� ���� ���� ã��

        if (emptySlot != null) // ����ִ� ������ �ִٸ�
        {
            emptySlot.item = item;
            UpdateUI();
            return;
        }

        ThrowWeapon(item);
    }

    // ������ ����
    void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    // ���� ����
    void ThrowWeapon(ItemWeaponData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    // �κ��丮â ������Ʈ
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null) // ���Կ� �������� �ִٸ�
                uiSlots[i].Set(slots[i]); // ����
            else
                uiSlots[i].Clear(); // ���ٸ� Ŭ����
        }

        for (int i = 0; i < weaponsSlots.Length; i++)
        {
            if (weaponsSlots[i].item != null) // ���Կ� �������� �ִٸ�
                weaponsUiSlots[i].Set(weaponsSlots[i]); // ����
            else
                weaponsUiSlots[i].Clear(); // ���ٸ� Ŭ����
        }
    }

    // �ش� ������ ���� ����
    ItemSlot GetItemStack(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount)
                return slots[i];
        }

        return null;
    }

    // ����ִ� ������ ���� ��������
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }

        return null;
    }


    // ����ִ� ������ ���� ��������
    WeaponSlot GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weaponsSlots.Length; i++)
        {
            if (weaponsSlots[i].item == null)
                return weaponsSlots[i];
        }

        return null;
    }

    // �ε����� �κ��丮â���� ������ ��������
    public void SelectItem(int index)
    {
        if (slots[index] == null)
            return;
        selectedWeapon = null;
        // ������ ������ ������ ����
        selectedItem = slots[index];
        selectedItemIndex = index;
        // ������ �̸�, ���� ǥ��
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
        // ������ ���� �ʱ�ȭ
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        // ������ ���� ǥ��
        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatNames.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValues.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }
        // ��ư ǥ��
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    // �ε����� ���� ����
    public void SelectWeapon(int index)
    {
        if (weaponsSlots[index] == null)
            return;
        selectedItem = null;
        // ������ ������ ������ ����
        selectedWeapon = weaponsSlots[index];
        selectedWeaponsIndex = index;
        // ������ �̸�, ���� ǥ��
        selectedItemName.text = selectedWeapon.item.displayName;
        selectedItemDescription.text = selectedWeapon.item.description;
        // ������ ���� �ʱ�ȭ
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;

        // ��ư ǥ��
        useButton.SetActive(selectedWeapon.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedWeapon.item.type == ItemType.Equipable && !weaponsUiSlots[index].equipped);
        unEquipButton.SetActive(selectedWeapon.item.type == ItemType.Equipable && weaponsUiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    // �κ��丮â���� ������ ���� ����ϱ�
    private void ClearSeletecItemWindow()
    {
        // ������ ���þ��� ǥ��
        selectedItem = null;
        selectedWeapon = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        // ������ ���� ǥ�� ����
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        // ��ư ǥ�� ����
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // ������ ����ư
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

    // ������ ���� ��ư
    public void OnEquipButton()
    {

    }

    // ������ ��������
    void UnEquip(int index)
    {

    }

    // ������ �������� ��ư
    public void OnUnEquipButton()
    {

    }

    // ������ ���� ��ư
    public void OnDropButton()
    {
        if (selectedItem != null)
            ThrowItem(selectedItem.item); // ���þ����� ����
        else if (selectedWeapon != null)
            ThrowWeapon(selectedWeapon.item); // ���ù��� ����
        RemoveSelectedItem(); // ���þ����� �κ��丮���� ����
    }

    // ������ ������ ����
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
            // ������ ���� �κ��丮���� ����
            Debug.Log("�ڵ� ���� �ʿ�:������ ���� �κ��丮���� ����");
        }

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
}