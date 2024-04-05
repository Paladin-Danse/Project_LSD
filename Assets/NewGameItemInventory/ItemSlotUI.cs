using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 각 슬롯 스크립트
public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
    public TextMeshProUGUI nameText;
    private Outline outline;

    public int index;
    public bool equipped;

    public ItemSlotUI(int _index)
    {
        index = _index;
    }

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped; // 장착했다면 아웃라인을 표시
    }

    // 아이템 셋팅
    public void Set(ItemSlot slot)
    {
        icon.gameObject.SetActive(true); // 아이콘 표시
        icon.sprite = slot.item.icon; // 아이콘 스프라이트 설정
        nameText.text = slot.item.displayName; // 텍스트 표현
        if (outline != null) // 아웃라인이 있다면
        {
            outline.enabled = equipped;
        }
    }

    // 아이템 클리어
    public void Clear()
    {
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }

    public void OnButtonClick()
    {
        Debug.Log($"인벤토리 {index}번 슬롯 클릭");
    }
}