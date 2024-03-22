using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �κ��丮 �� ���� ��ũ��Ʈ
public class WeaponSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI equipText;
    private WeaponSlot curSlot;
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
        equipText.enabled = equipped;
    }

    // ������ ����
    public void Set(WeaponSlot slot)
    {
        curSlot = slot; // ����� ������ ����
        icon.gameObject.SetActive(true); // ������ ǥ��
        icon.sprite = slot.item.icon; // ������ ��������Ʈ ����
        nameText.text = slot.item.displayName; // �ؽ�Ʈ ǥ��
        equipText.enabled = equipped; // �������� ǥ��
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
    }

    public void OnButtonClick()
    {
        Debug.Log("�κ��丮 ���� ���� Ŭ��");
        Inventory.instance.SelectWeapon(index);
    }
}