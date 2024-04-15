using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour, IPlayerUIInterface
{
    [Header("PlayerHealth")]
    public TMP_Text playerHealthText;
    public Image playerHealthHorizontalBar;

    public void BindUI(PlayerCharacter character)
    {
        character.health.HealthChanged += RefreshUI;
        character.OnStatChanged += RefreshUI;
        RefreshUI();
    }

    public void UnbindUI(PlayerCharacter character)
    {
        character.health.HealthChanged -= RefreshUI;
        character.OnStatChanged -= RefreshUI;
    }

    public void RefreshUI()
    {
        playerHealthText.text = $"{Player.Instance.playerCharacter.health.curHealth}";
        playerHealthHorizontalBar.fillAmount = Player.Instance.playerCharacter.health.curHealth / Player.Instance.playerCharacter.currentStat.maxHealth;
    }

}
