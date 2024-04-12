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
    private WeaponSlot curWeapon;

    // 아이템 셋팅
    public void UI_Update(ItemData data)
    {
        if (data != null)
        {
            //curWeapon = slot; // 슬롯에 들어간 무기
            icon.gameObject.SetActive(true); // 아이콘 표시
            icon.sprite = data.icon; // 아이콘 스프라이트 설정
            nameText.text = data.displayName; // 텍스트 표현
        }
        else
        {
            icon.gameObject.SetActive(false);
            nameText.text = string.Empty;
        }
    }

    // 아이템 클리어
    public void Clear()
    {
        curWeapon = null;
        icon.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        //Inventory.instance.SelectItem(index);
    }
}