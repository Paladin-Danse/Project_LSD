using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour, IInteractable
{
    public GameObject weaponObj; // 무기 정보
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
        //Inventory.instance.AddWeapon(weapon); // 인벤토리에 아이템 추가하기
        Destroy(gameObject); // 아이템 제거
    }
}