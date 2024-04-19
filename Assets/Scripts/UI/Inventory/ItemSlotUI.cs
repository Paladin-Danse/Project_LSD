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

    public int index;

    public void Init(int _index, ItemSlot _itemSlot)
    {
        index = _index;
        itemSlot = _itemSlot;
        itemSlot.Init(index);
    }
    /*
    private void OnEnable()
    {
        outline.enabled = equipped; // 장착했다면 아웃라인을 표시
    }
    */
    // 아이템 셋팅
    public void Set(ItemData data)
    {
        
    }

    public void UI_Update()
    {
        icon.sprite = itemSlot?.data.icon; // 아이콘 스프라이트 설정
        if(icon.sprite) icon.gameObject.SetActive(true); // 아이콘 표시
    }

    // 아이템 클리어
    public void Clear()
    {
        if (itemSlot.data == null)
        {
            icon.gameObject.SetActive(false);
        }
    }

    public void OnButtonClick()
    {
        //Debug.Log($"인벤토리 {index}번 슬롯 클릭");
    }
    public void OnClick()
    {
        Weapon weapon;
        //Equipment

        switch (itemSlot.data.type)
        {
            case ItemType.Weapon:
                Player.Instance.playerCharacter.InventoryWeaponEquip(new Weapon(itemSlot.data));
                break;
            case ItemType.Equipable:
                break;
            default:
                break;
        }
    }
}