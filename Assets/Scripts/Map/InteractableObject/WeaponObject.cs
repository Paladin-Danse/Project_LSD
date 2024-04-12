using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour, IInteractable
{
    public GameObject weaponObj; // 무기 정보
    private Weapon weapon;

    private void Awake()
    {
        weapon = weaponObj.GetComponent<Weapon>();
        weapon.Init(Player.Instance.playerCharacter);
    }
    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", weapon.itemData.displayName);
    }

    public void OnInteract()
    {
        Player.Instance.inventory.AddWeapon(weapon);
        Destroy(gameObject); // 아이템 제거
    }
}