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
        outline.enabled = equipped; // �����ߴٸ� �ƿ������� ǥ��
    }

    // ������ ����
    public void Set(ItemSlot slot)
    {
        icon.gameObject.SetActive(true); // ������ ǥ��
        icon.sprite = slot.item.icon; // ������ ��������Ʈ ����
        nameText.text = slot.item.displayName; // �ؽ�Ʈ ǥ��
        if (outline != null) // �ƿ������� �ִٸ�
        {
            outline.enabled = equipped;
        }
    }

    // ������ Ŭ����
    public void Clear()
    {
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }

    public void OnButtonClick()
    {
        Debug.Log($"�κ��丮 {index}�� ���� Ŭ��");
    }
}