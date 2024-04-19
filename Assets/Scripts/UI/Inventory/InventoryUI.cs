using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InventoryUI : MonoBehaviour
{
    private Inventory _inventory;

    //public GameObject inventoryWindow; // �κ��丮 �ѱ�
    public Transform slotParent;
    public ItemSlotUI uiSlot; // ui ����
    public ItemSlotUI[] slots; // item ����
    public TextMeshProUGUI selectedItemName; // �̸�
    public Dictionary<string, TextMeshProUGUI> selectedItemStatValues; // ���� ��

    [field: SerializeField]
    public WeaponSlotUI weaponSlotUI1 { get; private set;}

    [field: SerializeField]
    public WeaponSlotUI weaponSlotUI2 { get; private set; }

    public TextMeshProUGUI moneyText;

    public TextMeshProUGUI rifleAmmoCountText;
    public TextMeshProUGUI pistolAmmoCountText;
    
    /*
    public Transform AmmoSlots;
    
    public GameObject AmmoSlotUI;
    public Dictionary<AmmoType, TextMeshProUGUI> AmmoCountText;
    */

    public Action UI_All_Update;

    public void Init(Inventory inventory)
    {
        _inventory = inventory;
        
        if(Player.Instance.playerCharacter)
        {
            PlayerCharacter character = Player.Instance.playerCharacter;
            WeaponSlotUI_Update();
        }

        weaponSlotUI1?.button.onClick.RemoveAllListeners();
        weaponSlotUI2?.button.onClick.RemoveAllListeners();

        weaponSlotUI1?.button.onClick.AddListener(() => _inventory.UnEquip(1));
        weaponSlotUI1?.button.onClick.AddListener(weaponSlotUI1.Clear);
        weaponSlotUI2?.button.onClick.AddListener(() => _inventory.UnEquip(2));
        weaponSlotUI2?.button.onClick.AddListener(weaponSlotUI2.Clear);

        //����� �ָ�... ���߿� �ٸ� ź�� �߰��� ���� ����Ͽ� ���ܵ�.
        //rifleAmmoCountText = AmmoSlots.Find("Rifle/Count").GetComponent<TextMeshProUGUI>();
        //pistolAmmoCountText = AmmoSlots.Find("Pistol/Count").GetComponent<TextMeshProUGUI>();

        GameObject itemStatValues = transform.Find("Inventory/InfoBG/ItemStatValues").gameObject;
        selectedItemStatValues = new Dictionary<string, TextMeshProUGUI>();
        for (int i = 0; i < inventory.inventorySO.itemStatValues.Count; i++)
        {
            GameObject textObj = itemStatValues.transform.Find($"{inventory.inventorySO.statNames[i]}/StatValue").gameObject;
            selectedItemStatValues.Add(inventory.inventorySO.statNames[i], textObj?.GetComponent<TextMeshProUGUI>());
        }

        slots = new ItemSlotUI[inventory.inventorySO.itemSlotCount]; // ������ ����

        for (int i = 0; i < slots.Length; i++) // ���� �ʱ�ȭ
        {
            slots[i] = Instantiate(uiSlot, slotParent);
            slots[i].Init(i, new ItemSlot());
            slots[i].Clear();
        }

        UI_All_Update += AmmoUI_Update;
        UI_All_Update += MoneyUI_Update;
        UI_All_Update += WeaponSlotUI_Update;

        UI_All_Update?.Invoke();
        /*���� ���������� ������ �ٸ� ź�� �߰��Ǿ��� �� Dictionary ������ Text�� �����ϴ� �뵵�� ����ϱ� ���� ����.
        �ٸ� ���� ���������� ���� ����� ����� ä�� 8�� ������ ź�� ��� �ν����Ϳ��� �����Ѵٸ� �� �� ���� ���� ��õ.
        foreach (AmmoType type in Enum.GetValues(typeof(AmmoType)))
        {
            Instantiate(AmmoSlotUI, AmmoSlots);
            TextMeshProUGUI text;

            switch (type)
            {
                case AmmoType.Rifle:
                    text = rifleAmmoCountText;
                    break;
                case AmmoType.Pistol:
                    text = pistolAmmoCountText;
                    break;
                default:
                    text = null;
                    Debug.Log("Error(InventoryUI) : Wrong AmmoType");
                    break;
            }

            AmmoCountText.Add(type, text);
        }
        */
    }
    
    public void AmmoUI_Update()
    {
        rifleAmmoCountText.text = _inventory.inventoryAmmo[AmmoType.Rifle].ToString();
        pistolAmmoCountText.text = _inventory.inventoryAmmo[AmmoType.Pistol].ToString();
    }
    public void WeaponSlotUI_Update()
    {
        if (Player.Instance.playerCharacter.primaryWeapon != null)
        {
            weaponSlotUI1.InputData(Player.Instance.playerCharacter.primaryWeapon);
            weaponSlotUI1.UI_Update();
        }
        if (Player.Instance.playerCharacter.secondaryWeapon != null)
        {
            weaponSlotUI2.InputData(Player.Instance.playerCharacter.secondaryWeapon);
            weaponSlotUI2.UI_Update();
        }
    }
    public void SelectedItemUI_Update(ItemSlot item)
    {
        if (item != null)
        {
            selectedItemName.text = item.data.displayName;
            // ������ ���� �ʱ�ȭ
            foreach (string StatValueName in selectedItemStatValues.Keys)
            {
                selectedItemStatValues[StatValueName].text = item.data.itemStatValues.Values.ToString();
            }
        }
        else
        {
            selectedItemName.text = null;
            // ������ ���� �ʱ�ȭ
            foreach (string StatValueName in selectedItemStatValues.Keys)
            {
                selectedItemStatValues[StatValueName].text = string.Empty;
            }
        }
    }
    // �κ��丮â ������Ʈ
    public void UpdateSlotUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemSlot.data != null) // ���Կ� �������� �ִٸ�
                slots[i].UI_Update(); // ����
            else
                slots[i].Clear(); // ���ٸ� Ŭ����
        }
    }

    // ����ִ� ������ ���� ��������
    public ItemSlotUI GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemSlot.isEmpty)
                return slots[i];
        }

        return null;
    }
    /*
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
    */
    public void MoneyUI_Update()
    {
        moneyText.text = Player.Instance.inventory.money.ToString() + " G";
    }
}
