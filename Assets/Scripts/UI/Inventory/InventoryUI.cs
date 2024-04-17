using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InventoryUI : MonoBehaviour
{
    private Inventory _inventory;

    public GameObject inventoryWindow; // 인벤토리 켜기
    public ItemSlotUI uiSlot; // ui 슬롯
    public TextMeshProUGUI selectedItemName; // 이름
    public Dictionary<string, TextMeshProUGUI> selectedItemStatValues; // 스텟 값

    
    public WeaponSlotUI weaponSlotUI1 { get; private set;}
    public WeaponSlotUI weaponSlotUI2 { get; private set; }

    public TextMeshProUGUI moneyText;

    public TextMeshProUGUI rifleAmmoCountText;
    public TextMeshProUGUI pistolAmmoCountText;
    
    public Transform AmmoSlots;
    /*
    public GameObject AmmoSlotUI;
    public Dictionary<AmmoType, TextMeshProUGUI> AmmoCountText;
    */

    public Action UI_All_Update;

    public void Init(Inventory inventory)
    {
        _inventory = inventory;
        inventoryWindow = GameObject.Find("InventoryCanvas").transform.Find("Inventory").gameObject;
        moneyText = inventoryWindow.transform.Find("Money/Text").GetComponent<TextMeshProUGUI>();
        
        weaponSlotUI1 = inventoryWindow.transform.Find("WeaponSlot 1").GetComponent<WeaponSlotUI>();
        weaponSlotUI2 = inventoryWindow.transform.Find("WeaponSlot 2").GetComponent<WeaponSlotUI>();

        if(Player.Instance.playerCharacter)
        {
            PlayerCharacter character = Player.Instance.playerCharacter;
            WeaponSlotUI_Update();
        }

        weaponSlotUI1?.button.onClick.AddListener(() => _inventory.UnEquip(1));
        weaponSlotUI1?.button.onClick.AddListener(weaponSlotUI1.Clear);
        weaponSlotUI2?.button.onClick.AddListener(() => _inventory.UnEquip(2));
        weaponSlotUI2?.button.onClick.AddListener(weaponSlotUI2.Clear);

        AmmoSlots = inventoryWindow.transform.Find("AmmoSlots");

        rifleAmmoCountText = AmmoSlots.Find("Rifle/Count").GetComponent<TextMeshProUGUI>();
        pistolAmmoCountText = AmmoSlots.Find("Pistol/Count").GetComponent<TextMeshProUGUI>();

        GameObject itemName = inventoryWindow.transform.Find("InfoBG/ItemName").gameObject;
        selectedItemName = itemName?.GetComponent<TextMeshProUGUI>();
        GameObject itemStatValues = inventoryWindow.transform.Find("InfoBG/ItemStatValues").gameObject;
        selectedItemStatValues = new Dictionary<string, TextMeshProUGUI>();
        for (int i = 0; i < inventory.inventorySO.itemStatValues.Count; i++)
        {
            GameObject textObj = itemStatValues.transform.Find($"{inventory.inventorySO.statNames[i]}/StatValue").gameObject;
            selectedItemStatValues.Add(inventory.inventorySO.statNames[i], textObj?.GetComponent<TextMeshProUGUI>());
        }

        /*
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
        UI_All_Update += AmmoUI_Update;
        UI_All_Update += MoneyUI_Update;
        UI_All_Update += WeaponSlotUI_Update;
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
            weaponSlotUI1.UI_Update(Player.Instance.playerCharacter.primaryWeapon.itemData);
        }
        if (Player.Instance.playerCharacter.secondaryWeapon != null)
        {
            weaponSlotUI2.UI_Update(Player.Instance.playerCharacter.secondaryWeapon.itemData);
        }
    }
    public void SelectedItemUI_Update(ItemSlot item)
    {
        if (item != null)
        {
            selectedItemName.text = item.data.displayName;
            // 아이템 스텟 초기화
            foreach (string StatValueName in selectedItemStatValues.Keys)
            {
                selectedItemStatValues[StatValueName].text = item.data.itemStatValues.Values.ToString();
            }
        }
        else
        {
            selectedItemName.text = null;
            // 아이템 스텟 초기화
            foreach (string StatValueName in selectedItemStatValues.Keys)
            {
                selectedItemStatValues[StatValueName].text = string.Empty;
            }
        }
    }
    public void MoneyUI_Update()
    {
        moneyText.text = Player.Instance.inventory.money.ToString() + " G";
    }
}
