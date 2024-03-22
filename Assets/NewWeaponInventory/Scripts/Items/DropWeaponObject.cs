using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeaponObject : MonoBehaviour, IInteractable
{
    public ItemWeaponData item;

    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", item.displayName);
    }

    public void OnInteract()
    {
        Inventory.instance.PickUpItem(item); // �κ��丮(��Ŭ��ȭ)�� ������ �߰��ϱ�
        Destroy(gameObject); // ������ ����
    }
}