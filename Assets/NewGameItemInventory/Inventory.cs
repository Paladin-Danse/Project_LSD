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

// ������ ���� Ŭ����
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

// �κ��丮�� ǥ���ϱ� ���� Ŭ����
public class Inventory : MonoBehaviour, IObjectCrash
{
    public ItemSlotUI[] uiSlots; // ui ����
    public ItemSlot[] slots; // item ����

    public InventoryData inventorySO;
    public GameObject inventoryWindow; // �κ��丮 �ѱ�
    public Transform dropPosition; // ������ ������ ��ġ

    [Header("Selected Item")]
    private ItemSlot selectedItem; // ������ ������
    private int selectedItemIndex; // ������ ������ �ε���
    public TextMeshProUGUI selectedItemName; // �̸�
    public TextMeshProUGUI selectedItemDescription; // ����
    public TextMeshProUGUI selectedItemStatNames; // ����
    public TextMeshProUGUI selectedItemStatValues; // ���� ��
    public GameObject equipButton; // ���� ��ư
    public GameObject unEquipButton; // ���� ��ư
    public GameObject dropButton; // ���� ��ư

    private Dictionary<AmmoType, int> inventoryAmmo;
    public TextMeshProUGUI rifleAmmoCountText;
    public TextMeshProUGUI pistolAmmoCountText;

    public WeaponSlotUI weaponSlotUI1;
    public WeaponSlotUI weaponSlotUI2;

    private int curEquipIndex; // ???

    private PlayerCharacter character;

    [Header("Events")]
    public UnityEvent onOpenInventory; // �κ��丮 ���� �̺�Ʈ
    public UnityEvent onCloseInventory; // �κ��丮 �ݱ� �̺�Ʈ

    void Awake()
    {
        inventoryWindow = new Inventory().gameObject;
        character = GetComponent<PlayerCharacter>();
        slots = new ItemSlot[uiSlots.Length]; // ������ ����
        inventorySO.init();
        inventoryAmmo = new Dictionary<AmmoType, int>();

        foreach (AmmoType ammo in Enum.GetValues(typeof(AmmoType)))
            inventoryAmmo.Add(ammo, 0);
    }

    private void Start()
    {
        inventoryWindow.SetActive(false); // �κ��丮 ���α�
        

        for (int i = 0; i < slots.Length; i++) // ���� �ʱ�ȭ
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
            UpdateAmmoUI();
            weaponSlotUI1.nameText.text = character.primaryWeapon.name;
            weaponSlotUI2.nameText.text = character.secondaryWeapon.name;
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

    [System.Obsolete]
    public void TakeAmmoItemColliderCrash(AmmoType ammoType, int count)
    {
        inventoryAmmo[ammoType] += count;
        if (inventoryAmmo[ammoType] > inventorySO.maxAmmo[ammoType]) inventoryAmmo[ammoType] = inventorySO.maxAmmo[ammoType];

        if (inventoryWindow.active) // �κ��丮�� �����ִٸ�
        {
            UpdateAmmoUI();
        }
    }
    public int LostorUsedAmmo(AmmoType ammoType, int count)
    {
        int leftAmmo = inventoryAmmo[ammoType] < count ? inventoryAmmo[ammoType] : count;
        inventoryAmmo[ammoType] -= leftAmmo;

        if (inventoryWindow.activeSelf) // �κ��丮�� �����ִٸ�
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

    // �κ��丮�� ������ �߰�
    public void AddItem(ItemData item)
    {
        ItemSlot emptySlot = GetEmptySlot(); // ����ִ� ������ ���� ã��

        if (emptySlot != null) // ����ִ� ������ �ִٸ�
        {
            emptySlot.item = item;
            UpdateUI();
            return;
        }

        ThrowItem(item); // �κ��丮�� �� á�ٸ� ������
    }

    // ������ ����
    void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360f));
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

    // �ε����� �κ��丮â���� ������ ��������
    public void SelectItem(int index)
    {
        if (slots[index].item == null)
            return;
        // ������ ������ ������ ����
        selectedItem = slots[index];
        selectedItemIndex = index;
        // ������ �̸�, ���� ǥ��
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
        // ������ ���� �ʱ�ȭ
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        
        // ��ư ǥ��
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    // �κ��丮â���� ������ ���� ����ϱ�
    private void ClearSeletecItemWindow()
    {
        // ������ ���þ��� ǥ��
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        // ������ ���� ǥ�� ����
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;
        // ��ư ǥ�� ����
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
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
        ThrowItem(selectedItem.item); // ���þ����� ������ ����
        RemoveSelectedItem(); // ���þ����� �κ��丮���� ����
    }

    // ������ ������ ����
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