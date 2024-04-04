using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Text currentWeaponNameText;
    private Text currentMagazineTxt;
    private Text maxMagazineTxt;

    public void InitSetting()
    {
        GameObject textFinder;

        textFinder = transform.Find("HUDCanvas/Components/Weapon/Weapon Name").gameObject;
        if (textFinder.TryGetComponent(out currentWeaponNameText))
            Debug.Log("Player(CurrentWeaponNameText) : Wrong path");
        textFinder = transform.Find("HUDCanvas/Components/Weapon/Weapon Bullet Count").gameObject;
        if (textFinder.TryGetComponent(out currentMagazineTxt))
            Debug.Log("Player(currentMagazineTxt) : Wrong path");
        textFinder = transform.Find("HUDCanvas/Components/Weapon/Weapon Clip Count").gameObject;
        if (textFinder.TryGetComponent(out maxMagazineTxt))
            Debug.Log("Player(maxMagazineTxt) : Wrong path");
    }

    public void UITextUpdate(PlayerCharacter player)
    {
        if(currentWeaponNameText) currentWeaponNameText.text = player.curWeapon.WeaponName;
        if(currentMagazineTxt) currentMagazineTxt.text = player.curWeapon.curMagazineText;
        if(maxMagazineTxt) maxMagazineTxt.text = player.curWeapon.maxMagazineText;
    }
}
