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

    public int index;
    public bool equipped;


    // 아이템 셋팅
    public void UI_Update(ItemData data)
    {
        if (data != null)
        {
            icon.gameObject.SetActive(true); // 아이콘 표시
            weaponItemData = data;
            icon.sprite = data.icon; // 아이콘 스프라이트 설정
            nameText.text = data.displayName; // 텍스트 표현
            button.enabled = true;
        }
        else
        {
            Clear();
        }
    }

    // 아이템 클리어
    public void Clear()
    {
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