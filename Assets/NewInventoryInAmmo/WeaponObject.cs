using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour, IInteractable
{
    public Weapon weapon; // ���� ����

    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", weapon.name);
    }

    public void OnInteract()
    {
        Inventory.instance.AddWeapon(weapon); // �κ��丮�� ������ �߰��ϱ�
        Destroy(gameObject); // ������ ����
    }
}