using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponUI : MonoBehaviour, IPlayerUIInterface
{
    public Image playerWeaponImage;
    public TMP_Text playerWeaponMagText;
    public TMP_Text playerInventoryAmmoText;

    public PlayerCharacter playerCharacter { get; set; }
    Weapon bindedWeapon = null;

    public void BindUI(PlayerCharacter character)
    {
        playerCharacter = character;
        SetWeapon(playerCharacter.curWeapon);
        playerCharacter.OnWeaponChanged += SetWeapon;
        RefreshUI(playerCharacter.curWeapon);
    }

    public void UnbindUI()
    {
        playerCharacter.OnWeaponChanged -= SetWeapon;
    }

    public void RefreshUI() 
    {
    
    }

    public void RefreshUI(Weapon weapon)
    {
        if (weapon == null) 
        {
            gameObject.SetActive(false); 
        }
        else
        {
            gameObject.SetActive(true);
            RefreshWeaponMagText();
            RefreshInventoryAmmoText();
            RefreshIcon();
        }
    }

    void SetWeapon(Weapon weapon) 
    {
        ReleaseWeapon();
        if (weapon != null) 
        {
            Debug.Log("BindWeapon!");
            bindedWeapon = weapon;
            playerCharacter.ownedPlayer.inventory.OnAmmoChanged += RefreshInventoryAmmoText;
            bindedWeapon.OnMagChanged += RefreshWeaponMagText;
            RefreshUI(bindedWeapon);
        }
    }

    void ReleaseWeapon() 
    {
        if(bindedWeapon != null) 
        {
            Debug.Log("ReleaseWeapon!");
            playerCharacter.ownedPlayer.inventory.OnAmmoChanged -= RefreshInventoryAmmoText;
            bindedWeapon.OnMagChanged -= RefreshWeaponMagText;
            bindedWeapon = null;
            RefreshUI(null);
        }
    }

    void RefreshWeaponMagText()
    {
        playerWeaponMagText.text = $"{bindedWeapon.curMagazine}";
    }

    void RefreshInventoryAmmoText()
    {
        playerInventoryAmmoText.text = $"{Player.Instance.inventory.InventoryAmmoCheck(bindedWeapon.baseStatSO.weaponStat.e_useAmmo)}";
    }

    void RefreshIcon() 
    {
        playerWeaponImage.sprite = bindedWeapon.itemData.iconSprite;
    }
}
