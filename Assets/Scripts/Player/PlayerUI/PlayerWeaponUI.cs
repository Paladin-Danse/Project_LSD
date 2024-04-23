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

    public void BindUI(PlayerCharacter character)
    {
        playerCharacter = character;
        // todo : WeaponChange 이벤트 만들고 RefreshWeaponUI 바인드
        playerCharacter.OnWeaponSwapped += RefreshUI;
        playerCharacter.curWeapon.OnMagChanged += RefreshInventoryAmmoText;
        playerCharacter.curWeapon.OnMagChanged += RefreshWeaponMagText;
        RefreshUI();
    }

    public void UnbindUI()
    {
        // todo : WeaponChange 이벤트 만들고 RefreshWeaponUI 바인드
        playerCharacter.OnWeaponSwapped -= RefreshUI;
        playerCharacter.curWeapon.OnMagChanged -= RefreshInventoryAmmoText;
        playerCharacter.curWeapon.OnMagChanged -= RefreshWeaponMagText;
        // Player.Instance.playerCharacter.curWeapon.OnMagChanged -= RefreshWeaponUI;
    }
    public void RefreshUI()
    {
        if (playerCharacter.curWeapon == null) 
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

    void RefreshWeaponMagText()
    {
        playerWeaponMagText.text = $"{playerCharacter.curWeapon.curMagazine}";
    }

    void RefreshInventoryAmmoText()
    {
        playerInventoryAmmoText.text = $"{Player.Instance.inventory.InventoryAmmoCheck(playerCharacter.curWeapon_AmmoType)}";
    }

    void RefreshIcon() 
    {
        playerWeaponImage.sprite = playerCharacter.curWeapon.itemData.iconSprite;
    }
}
