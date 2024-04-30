using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour, IInteractable
{
    public GameObject weaponObj; // ���� ����
    private Weapon weapon;
    public WeaponStatSO weaponStatSO;

    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject dropPrefab;
    private void Awake()
    {
        
    }
    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", displayName);
    }

    public void OnInteract(Player player)
    {
        Player.Instance.inventory.AddWeapon(weaponStatSO.weaponItem, weaponStatSO.weaponStat); // �κ��丮�� ������ �߰��ϱ�
        Destroy(gameObject); // ������ ����
    }
}