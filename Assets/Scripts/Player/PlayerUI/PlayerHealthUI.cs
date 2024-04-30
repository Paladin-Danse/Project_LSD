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

    public PlayerCharacter playerCharacter { get; set; }

    public void BindUI(PlayerCharacter character)
    {
        playerCharacter = character;
        playerCharacter.health.HealthChanged += RefreshUI;
        playerCharacter.health.OnDie += RefreshUI;
        playerCharacter.OnStatChanged += RefreshUI;
        RefreshUI();
    }

    public void UnbindUI()
    {
        playerCharacter.health.HealthChanged -= RefreshUI;
        playerCharacter.health.OnDie -= RefreshUI;
        playerCharacter.OnStatChanged -= RefreshUI;
    }

    public void RefreshUI()
    {
        playerHealthText.text = $"{playerCharacter.health.curHealth}";
        playerHealthHorizontalBar.fillAmount = playerCharacter.health.curHealth / playerCharacter.currentStat.maxHealth;
    }
}
