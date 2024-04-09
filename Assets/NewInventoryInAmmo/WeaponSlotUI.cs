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

    // ������ ����
    public void UI_Update(ItemData data)
    {
        if (data != null)
        {
            //curWeapon = slot; // ���Կ� �� ����
            icon.gameObject.SetActive(true); // ������ ǥ��
            icon.sprite = data.icon; // ������ ��������Ʈ ����
            nameText.text = data.displayName; // �ؽ�Ʈ ǥ��
        }
        else
        {
            icon.gameObject.SetActive(false);
            nameText.text = string.Empty;
        }
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