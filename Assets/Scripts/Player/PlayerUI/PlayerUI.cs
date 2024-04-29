using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public interface IPlayerUIInterface 
{
    public void BindUI(PlayerCharacter character);
    public void UnbindUI();

    public void RefreshUI();

    public PlayerCharacter playerCharacter { get; set; }
}

public class PlayerUI : MonoBehaviour
{
    public PlayerHealthUI healthUI;
    public PlayerWeaponUI weaponUI;
    public PlayerSkillUI skillUI;
    public PlayerMissionUI missionUI;
    public PlayerCrosshairUI crosshairUI;
    public PlayerInteractUI interactUI;
    public PlayerDamageFeedbackUI damageFeedbackUI;

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
        damageFeedbackUI.BindUI(character);
    }

    void UnBindUI() 
    {
        healthUI.UnbindUI();
        weaponUI.UnbindUI();
        interactUI.UnbindUI();
        damageFeedbackUI.UnbindUI();
    }
}
