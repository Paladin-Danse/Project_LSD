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
    public ItemData weaponItemData;

    public int index;
    public bool equipped;


    // ������ ����
    public void UI_Update(ItemData data)
    {
        if (data != null)
        {
            icon.gameObject.SetActive(true); // ������ ǥ��
            weaponItemData = data;
            icon.sprite = data.icon; // ������ ��������Ʈ ����
            nameText.text = data.displayName; // �ؽ�Ʈ ǥ��
            button.enabled = true;
        }
        else
        {
            Clear();
        }
    }

    // ������ Ŭ����
    public void Clear()
    {
        icon.sprite = null;
        nameText.text = string.Empty;
        button.enabled = false;
        icon.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        //Inventory.instance.SelectItem(index);
    }
}