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
        playerCharacter.weaponStatHandler.OnStatChanged += RefreshInventoryAmmoText;
        playerCharacter.curWeapon.OnMagChanged += RefreshWeaponMagText;
    }

    public void UnbindUI()
    {
        // todo : WeaponChange 이벤트 만들고 RefreshWeaponUI 바인드
        playerCharacter.weaponStatHandler.OnStatChanged -= RefreshInventoryAmmoText;
        playerCharacter.curWeapon.OnMagChanged -= RefreshWeaponMagText;
        // Player.Instance.playerCharacter.curWeapon.OnMagChanged -= RefreshWeaponUI;
    }
    public void RefreshUI()
    {
        RefreshWeaponMagText();
        RefreshInventoryAmmoText();
    }

    void RefreshWeaponMagText()
    {
        playerWeaponMagText.text = $"{playerCharacter.curWeapon.curMagazine}";
    }

    void RefreshInventoryAmmoText()
    {
        // todo : Inventory Ammo와 연결
        // 무기 종류에 따라 Ammo 연결 다르게 해줘야 함
    }
}
