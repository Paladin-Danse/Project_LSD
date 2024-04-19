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

    public int index;

    public void Init(int _index, ItemSlot _itemSlot)
    {
        index = _index;
        itemSlot = _itemSlot;
        itemSlot.Init(index);
    }
    /*
    private void OnEnable()
    {
        outline.enabled = equipped; // �����ߴٸ� �ƿ������� ǥ��
    }
    */
    // ������ ����
    public void Set(ItemData data)
    {
        
    }

    public void UI_Update()
    {
        icon.sprite = itemSlot?.data.icon; // ������ ��������Ʈ ����
        if(icon.sprite) icon.gameObject.SetActive(true); // ������ ǥ��
    }

    // ������ Ŭ����
    public void Clear()
    {
        if (itemSlot.data == null)
        {
            icon.gameObject.SetActive(false);
        }
    }

    public void OnButtonClick()
    {
        //Debug.Log($"�κ��丮 {index}�� ���� Ŭ��");
    }
    public void OnClick()
    {
        Weapon weapon;
        //Equipment

        switch (itemSlot.data.type)
        {
            case ItemType.Weapon:
                Player.Instance.playerCharacter.InventoryWeaponEquip(new Weapon(itemSlot.data));
                break;
            case ItemType.Equipable:
                break;
            default:
                break;
        }
    }
}