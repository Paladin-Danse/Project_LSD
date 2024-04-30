using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 각 슬롯 스크립트
public class ItemSlotUI : MonoBehaviour
{
    public ItemSlot itemSlot;
    public Button button;
    public Image icon;
    public WeaponStat weaponStat;

    public int index;

    public void Init(int _index, ItemSlot _itemSlot)
    {
        index = _index;
        itemSlot = _itemSlot;
        itemSlot.Init(index);
        weaponStat = null;
    }
    public void Set(ItemData data, WeaponStat _weaponStat)
    {
        itemSlot.data = data;
        weaponStat = _weaponStat;
        UI_Update();

        if(icon.gameObject.activeSelf) button.onClick.AddListener(OnClick);
    }
    //Equipment추가될 시, Set함수를 오버로딩해서 사용.

    public void UI_Update()
    {
        if (itemSlot.data == null)
        {
            Clear();
            return;
        }
        icon.sprite = itemSlot.data.iconSprite; // 아이콘 스프라이트 설정
        if(icon.sprite) icon.gameObject.SetActive(true); // 아이콘 표시
    }

    // 아이템 클리어
    public void Clear()
    {
        button.onClick.RemoveAllListeners();
        itemSlot.data = null;
        weaponStat = null;
        icon.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        //Debug.Log($"인벤토리 {index}번 슬롯 클릭");
    }
    public void OnClick()
    {
        bool isEquip;
        switch (itemSlot.data.type)
        {
            case ItemType.Weapon:
                isEquip = Player.Instance.inventory.OnEquip(weaponStat);
                break;
            case ItemType.Equipable:
                isEquip = false;
                break;
            default:
                isEquip = false;
                break;
        }
        if(isEquip)
        {
            Clear();
            UI_Update();
            Player.Instance.inventory.inventoryUI.WeaponSlotUI_Update();
        }
        else
        {
            Debug.Log("EquipSlot is Full!!");
        }
    }
}