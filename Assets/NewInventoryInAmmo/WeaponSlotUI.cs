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
    private WeaponSlot curWeapon;

    public int index;
    public bool equipped;

    // ������ ����
    public void Set(WeaponSlot slot)
    {
        curWeapon = slot; // ���Կ� �� ����
        icon.gameObject.SetActive(true); // ������ ǥ��
        icon.sprite = slot.item.icon; // ������ ��������Ʈ ����
        nameText.text = slot.item.displayName; // �ؽ�Ʈ ǥ��
    }

    // ������ Ŭ����
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