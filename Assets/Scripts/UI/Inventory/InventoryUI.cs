using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InventoryUI : MonoBehaviour
{
    private Inventory _inventory;

    //public GameObject inventoryWindow; // 인벤토리 켜기
    public Transform slotParent;
    public ItemSlotUI uiSlot; // ui 슬롯
    public ItemSlotUI[] slots; // item 슬롯
    public TextMeshProUGUI selectedItemName; // 이름
    public Dictionary<string, TextMeshProUGUI> selectedItemStatValues; // 스텟 값

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

        //지우기 애매... 나중에 다른 탄이 추가될 때를 대비하여 남겨둠.
        //rifleAmmoCountText = AmmoSlots.Find("Rifle/Count").GetComponent<TextMeshProUGUI>();
        //pistolAmmoCountText = AmmoSlots.Find("Pistol/Count").GetComponent<TextMeshProUGUI>();

        GameObject itemStatValues = transform.Find("Inventory/InfoBG/ItemStatValues").gameObject;
        selectedItemStatValues = new Dictionary<string, TextMeshProUGUI>();
        for (int i = 0; i < inventory.inventorySO.itemStatValues.Count; i++)
        {
            GameObject textObj = itemStatValues.transform.Find($"{inventory.inventorySO.statNames[i]}/StatValue").gameObject;
            selectedItemStatValues.Add(inventory.inventorySO.statNames[i], textObj?.GetComponent<TextMeshProUGUI>());
        }

        slots = new ItemSlotUI[inventory.inventorySO.itemSlotCount]; // 아이템 슬롯

        for (int i = 0; i < slots.Length; i++) // 슬롯 초기화
        {
            slots[i] = Instantiate(uiSlot, slotParent);
            slots[i].Init(i, new ItemSlot());
            slots[i].Clear();
        }

        UI_All_Update += AmmoUI_Update;
        UI_All_Update += MoneyUI_Update;
        UI_All_Update += WeaponSlotUI_Update;

        UI_All_Update?.Invoke();
        /*위와 마찬가지의 이유로 다른 탄이 추가되었을 때 Dictionary 변수에 Text를 저장하는 용도로 사용하기 위해 만듦.
        다만 위와 마찬가지로 현재 방식을 사용한 채로 8개 가량의 탄을 모두 인스펙터에서 관리한다면 둘 다 지울 것을 추천.
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
    // 인벤토리창 업데이트
    public void UpdateSlotUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemSlot.data != null) // 슬롯에 아이템이 있다면
                slots[i].UI_Update(); // 세팅
            else
                slots[i].Clear(); // 없다면 클리어
        }
    }

    // 비어있는 아이템 슬롯 가져오기
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
    */
    public void MoneyUI_Update()
    {
        moneyText.text = Player.Instance.inventory.money.ToString() + " G";
    }
}
