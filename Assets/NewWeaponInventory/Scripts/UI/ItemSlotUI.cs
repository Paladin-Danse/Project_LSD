using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �κ��丮 �� ���� ��ũ��Ʈ
public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
    public TextMeshProUGUI nameText;
    private ItemSlot curSlot;
    private Outline outline;

    public int index;
    public bool equipped;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped; // �����ߴٸ� �ƿ������� ǥ��
    }

    // ������ ����
    public void Set(ItemSlot slot)
    {
        curSlot = slot; // ����� ������ ����
        icon.gameObject.SetActive(true); // ������ ǥ��
        icon.sprite = slot.item.icon; // ������ ��������Ʈ ����
        quatityText.text = slot.quantity > 1 ? slot.quantity.ToString() : string.Empty; // �ؽ�Ʈ ǥ��
        nameText.text = slot.item.displayName; // �ؽ�Ʈ ǥ��
        if (outline != null) // �ƿ������� �ִٸ�
        {
            outline.enabled = equipped;
        }
    }

    // ������ Ŭ����
    public void Clear()
    {
        curSlot = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }

    public void OnButtonClick()
    {
        Debug.Log("�κ��丮 ���� Ŭ��");
        Inventory.instance.SelectItem(index);
    }
}