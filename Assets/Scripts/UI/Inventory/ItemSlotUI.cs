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
    public WeaponStat weaponStat;

    public int index;

    public void Init(int _index, ItemSlot _itemSlot)
    {
        index = _index;
        itemSlot = _itemSlot;
        itemSlot.Init(index);
        weaponStat = null;
    }
    public void Set(ItemData data, WeaponStat _weaponStat)
    {
        itemSlot.data = data;
        weaponStat = _weaponStat;
        UI_Update();

        if(icon.gameObject.activeSelf) button.onClick.AddListener(OnClick);
    }
    //Equipment�߰��� ��, Set�Լ��� �����ε��ؼ� ���.

    public void UI_Update()
    {
        if (itemSlot.data == null)
        {
            Clear();
            return;
        }
        icon.sprite = itemSlot.data.iconSprite; // ������ ��������Ʈ ����
        if(icon.sprite) icon.gameObject.SetActive(true); // ������ ǥ��
    }

    // ������ Ŭ����
    public void Clear()
    {
        button.onClick.RemoveAllListeners();
        itemSlot.data = null;
        weaponStat = null;
        icon.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        //Debug.Log($"�κ��丮 {index}�� ���� Ŭ��");
    }
    public void OnClick()
    {
        bool isEquip;
        switch (itemSlot.data.type)
        {
            case ItemType.Weapon:
                isEquip = Player.Instance.inventory.OnEquip(weaponStat);
                break;
            case ItemType.Equipable:
                isEquip = false;
                break;
            default:
                isEquip = false;
                break;
        }
        if(isEquip)
        {
            Clear();
            UI_Update();
            Player.Instance.inventory.inventoryUI.WeaponSlotUI_Update();
        }
        else
        {
            Debug.Log("EquipSlot is Full!!");
        }
    }
}