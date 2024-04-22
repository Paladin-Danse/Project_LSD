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
    public Weapon weaponData;

    public int index;
    public bool equipped;


    // ������ ����
    public void InputData(Weapon data)
    {
        if (data != null)
        {
            weaponData = data;
            weaponItemData = data.itemData;
            button.enabled = true;
        }
    }

    public void UI_Update()
    {
        if (weaponData != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = weaponItemData.iconSprite;
            nameText.text = weaponItemData.displayName;
        }
        else
        {
            Clear();
        }
    }

    // ������ Ŭ����
    public void Clear()
    {
        weaponData = null;
        weaponItemData = null;
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