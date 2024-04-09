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
    private Outline outline;

    public int index;
    //public bool equipped;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }
    public void Init(ItemSlot _itemSlot)
    {
        Transform componentFinder;

        itemSlot = _itemSlot;
        index = itemSlot.index;
        componentFinder = transform.Find("Icon");
        componentFinder?.TryGetComponent(out icon);
    }
    /*
    private void OnEnable()
    {
        outline.enabled = equipped; // 장착했다면 아웃라인을 표시
    }
    */
    // 아이템 셋팅
    public void Set(ItemSlot slot)
    {
        icon.gameObject.SetActive(true); // 아이콘 표시
        icon.sprite = slot.item.icon; // 아이콘 스프라이트 설정
        /*
        if (outline != null) // 아웃라인이 있다면
        {
            outline.enabled = equipped;
        }
        */
    }

    // 아이템 클리어
    public void Clear()
    {
        icon.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        Debug.Log($"인벤토리 {index}번 슬롯 클릭");
        itemSlot.OnClick();
    }
}