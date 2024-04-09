using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �κ��丮 �� ���� ��ũ��Ʈ
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
        outline.enabled = equipped; // �����ߴٸ� �ƿ������� ǥ��
    }
    */
    // ������ ����
    public void Set(ItemSlot slot)
    {
        icon.gameObject.SetActive(true); // ������ ǥ��
        icon.sprite = slot.item.icon; // ������ ��������Ʈ ����
        /*
        if (outline != null) // �ƿ������� �ִٸ�
        {
            outline.enabled = equipped;
        }
        */
    }

    // ������ Ŭ����
    public void Clear()
    {
        icon.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        Debug.Log($"�κ��丮 {index}�� ���� Ŭ��");
        itemSlot.OnClick();
    }
}