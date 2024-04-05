using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour, IInteractable
{
    public GameObject weaponObj; // ���� ����
    private Weapon weapon;

    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject dropPrefab;
    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }
    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", displayName);
    }

    public void OnInteract()
    {
        //Inventory.instance.AddWeapon(weapon); // �κ��丮�� ������ �߰��ϱ�
        Destroy(gameObject); // ������ ����
    }
}