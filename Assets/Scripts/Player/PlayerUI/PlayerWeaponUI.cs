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

    public void BindUI(PlayerCharacter character)
    {
        // todo : WeaponChange �̺�Ʈ ����� RefreshWeaponUI ���ε�
        character.weaponStatHandler.OnStatChanged += RefreshInventoryAmmoText;
        character.curWeapon.OnMagChanged += RefreshWeaponMagText;
    }

    public void UnbindUI(PlayerCharacter character)
    {
        // todo : WeaponChange �̺�Ʈ ����� RefreshWeaponUI ���ε�
        character.weaponStatHandler.OnStatChanged -= RefreshInventoryAmmoText;
        character.curWeapon.OnMagChanged -= RefreshWeaponMagText;
        // Player.Instance.playerCharacter.curWeapon.OnMagChanged -= RefreshWeaponUI;
    }
    public void RefreshUI()
    {
        RefreshWeaponMagText();
        RefreshInventoryAmmoText();
    }

    void RefreshWeaponMagText()
    {
        playerWeaponMagText.text = $"{Player.Instance.playerCharacter.curWeapon.curMagazine}";
    }

    void RefreshInventoryAmmoText()
    {
        // todo : Inventory Ammo�� ����
        // ���� ������ ���� Ammo ���� �ٸ��� ����� ��
    }
}
