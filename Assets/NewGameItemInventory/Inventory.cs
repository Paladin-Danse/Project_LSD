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
        //�����ϸ� �ش� �������� �κ��丮�� ������� ��񽽷Կ� ����.
        slotUI.Clear();
    }
}

// �κ��丮�� ǥ���ϱ� ���� Ŭ����
public class Inventory : MonoBehaviour, IObjectCrash
{
    public ItemSlotUI uiSlot; // ui ����
    public ItemSlot[] slots; // item ����
    public Transform slotParent;
    
    public InventoryData inventorySO;
    public GameObject inventoryWindow; // �κ��丮 �ѱ�
    public Transform dropPosition; // ������ ������ ��ġ

    [Header("Selected Item")]
    private ItemSlot selectedItem; // ������ ������
    private int selectedItemIndex; // ������ ������ �ε���
    public TextMeshProUGUI selectedItemName; // �̸�
    //public TextMeshProUGUI selectedItemDescription; // ����
    //public TextMeshProUGUI selectedItemStatNames; // ����
    public Dictionary<string, TextMeshProUGUI> selectedItemStatValues; // ���� ��
    //public GameObject equipButton; // ���� ��ư
    //public GameObject unEquipButton; // ���� ��ư
    //public GameObject dropButton; // ���� ��ư

    private Dictionary<AmmoType, int> inventoryAmmo;
    public TextMeshProUGUI rifleAmmoCountText;
    public TextMeshProUGUI pistolAmmoCountText;

    public WeaponSlotUI weaponSlotUI1;
    public WeaponSlotUI weaponSlotUI2;

    private int curEquipIndex; // ???
    public int itemSlotCount;

    private PlayerCharacter character;

    [Header("Events")]
    public UnityEvent onOpenInventory; // �κ��丮 ���� �̺�Ʈ
    public UnityEvent onCloseInventory; // �κ��丮 �ݱ� �̺�Ʈ

    void Awake()
    {
        character = GetComponent<PlayerCharacter>();
        slots = new ItemSlot[itemSlotCount]; // ������ ����
        slotParent = inventoryWindow.transform.Find("Inventory/Scroll View/Viewport/Slots");

        inventorySO.init();
        inventoryAmmo = new Dictionary<AmmoType, int>();
        
        foreach (AmmoType ammo in Enum.GetValues(typeof(AmmoType)))
            inventoryAmmo.Add(ammo, 0);
    }

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++) // ���� �ʱ�ȭ
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
        inventoryWindow.SetActive(false); // �κ��丮 ���α�
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
            if (slots[i].item != null) // ���Կ� �������� �ִٸ�
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
        //selectedItemDescription.text = selectedItem.item.description;
        // ������ ���� �ʱ�ȭ
        //selectedItemStatNames.text = string.Empty;
        foreach (TextMeshProUGUI StatValueText in selectedItemStatValues.Values)
        {
            StatValueText.text = string.Empty;
        }
    }

    // �κ��丮â���� ������ ���� ����ϱ�
    private void ClearSeletecItemWindow()
    {
        // ������ ���þ��� ǥ��
        selectedItem = null;
        selectedItemName.text = string.Empty;
        //selectedItemDescription.text = string.Empty;
        // ������ ���� ǥ�� ����
        //selectedItemStatNames.text = string.Empty;
        foreach(TextMeshProUGUI StatValueText in selectedItemStatValues.Values)
        {
            StatValueText.text = string.Empty;
        }
    }

    // ������ ����
    public void OnEquip()
    {
        
    }

    // ������ ��������
    void UnEquip(int index)
    {

    }

    // ������ ����
    public void OnDrop()
    {
        ThrowItem(selectedItem.item); // ���þ����� ������ ����
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