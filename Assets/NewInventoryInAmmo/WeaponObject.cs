using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour, IInteractable
{
    public Weapon weapon; // 무기 정보

    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", weapon.name);
    }

    public void OnInteract()
    {
        Inventory.instance.AddWeapon(weapon); // 인벤토리에 아이템 추가하기
        Destroy(gameObject); // 아이템 제거
    }
}