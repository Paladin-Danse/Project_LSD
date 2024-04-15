using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public interface IPlayerUIInterface 
{
    public void BindUI(PlayerCharacter character);
    public void UnbindUI(PlayerCharacter character);

    public void RefreshUI();
}

public class PlayerUI : MonoBehaviour
{
    PlayerHealthUI healthUI;
    PlayerWeaponUI weaponUI;
    PlayerSkillUI skillUI;
    PlayerMissionUI missionUI;
    PlayerCrosshairUI crosshairUI;
    PlayerInteractUI interactUI;

    PlayerCharacter bindedPlayerCharacter;

    public void BindPlayerCharacter(PlayerCharacter character) 
    {
        bindedPlayerCharacter = character;
        BindUI(bindedPlayerCharacter);
    }

    public void ReleasePlayerCharacter()
    {
        UnBindUI();
        bindedPlayerCharacter = null;
    }


    void BindUI(PlayerCharacter character) 
    {
        healthUI.BindUI(character);
        weaponUI.BindUI(character);
        interactUI.BindUI(character);
    }

    void UnBindUI() 
    {
        healthUI.UnbindUI(bindedPlayerCharacter);
        weaponUI.UnbindUI(bindedPlayerCharacter);
        interactUI.UnbindUI(bindedPlayerCharacter);
    }
}
