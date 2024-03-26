using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Text CurrentWeaponNameText;
    private Text currentMagazineTxt;
    private Text maxMagazineTxt;

    public void InitSetting()
    {
        if(!transform.Find("HUDCanvas/Components/Weapon/Weapon Name").TryGetComponent(out CurrentWeaponNameText))
            Debug.Log("Player(CurrentWeaponNameText) : Wrong path");
        if (!transform.Find("HUDCanvas/Components/Weapon/Weapon Bullet Count").TryGetComponent(out currentMagazineTxt))
            Debug.Log("Player(currentMagazineTxt) : Wrong path");
        if (!transform.Find("HUDCanvas/Components/Weapon/Weapon Clip Count").TryGetComponent(out maxMagazineTxt))
            Debug.Log("Player(maxMagazineTxt) : Wrong path");
    }

    public void UITextUpdate(PlayerCharacter player)
    {
        //CurrentWeaponNameText.text = player.curWeapon.WeaponName;
        currentMagazineTxt.text = player.curWeapon.curMagazineText;
        maxMagazineTxt.text = player.curWeapon.maxMagazineText;
    }
}
