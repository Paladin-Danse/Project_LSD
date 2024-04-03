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

    public int index;
    public bool equipped;

    // 아이템 셋팅
    public void Set(WeaponSlot slot)
    {
        curWeapon = slot; // 슬롯에 들어간 무기
        icon.gameObject.SetActive(true); // 아이콘 표시
        icon.sprite = slot.item.icon; // 아이콘 스프라이트 설정
        nameText.text = slot.item.displayName; // 텍스트 표현
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