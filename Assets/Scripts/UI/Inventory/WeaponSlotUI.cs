using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 각 슬롯 스크립트
public class WeaponSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI nameText;
    public ItemData weaponItemData;
    public Weapon weaponData;

    public int index;
    public bool equipped;


    // 아이템 셋팅
    public void InputData(Weapon data)
    {
        if (data != null)
        {
            weaponData = data;
            weaponItemData = data.itemData;
            button.enabled = true;
        }
    }

    public void UI_Update()
    {
        if (weaponData != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = weaponItemData.iconSprite;
            nameText.text = weaponItemData.displayName;
        }
        else
        {
            Clear();
        }
    }

    // 아이템 클리어
    public void Clear()
    {
        weaponData = null;
        weaponItemData = null;
        icon.sprite = null;
        nameText.text = string.Empty;
        button.enabled = false;
        icon.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        //Inventory.instance.SelectItem(index);
    }
}