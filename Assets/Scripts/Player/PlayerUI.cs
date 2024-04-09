using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text playerHealthText;
    public Text playerWeaponMagText;


    public void BindUI() 
    {
        Player.Instance.playerCharacter.health.HealthChanged += RefreshHPUI;
        Player.Instance.playerCharacter.OnStatChanged += RefreshHPUI;
        Player.Instance.playerCharacter.weaponStatHandler.OnStatChanged += RefreshWeaponUI;
        Player.Instance.playerCharacter.curWeapon.OnMagChanged += RefreshWeaponUI;

        RefreshHPUI();
        RefreshWeaponUI();
    }

    public void UnBindUI() 
    {
        Player.Instance.playerCharacter.health.HealthChanged -= RefreshHPUI;
        Player.Instance.playerCharacter.OnStatChanged -= RefreshHPUI;
        Player.Instance.playerCharacter.weaponStatHandler.OnStatChanged -= RefreshWeaponUI;
        Player.Instance.playerCharacter.curWeapon.OnMagChanged -= RefreshWeaponUI;
    }

    void RefreshHPUI() 
    {
        playerHealthText.text = $"{Player.Instance.playerCharacter.health.curHealth}/{Player.Instance.playerCharacter.currentStat.maxHealth}";
    }

    void RefreshWeaponUI()
    {
        if(Player.Instance.playerCharacter.curWeapon != null)
            playerWeaponMagText.text = $"{Player.Instance.playerCharacter.curWeapon.curMagazine}/{Player.Instance.playerCharacter.curWeapon.curWeaponStat.magazine}";
    }
}
