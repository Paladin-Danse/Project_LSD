using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerInteractUI : MonoBehaviour, IPlayerUIInterface
{
    public TMP_Text promptText;

    public PlayerCharacter playerCharacter { get; set; }

    public void BindUI(PlayerCharacter character)
    {
        Debug.Log("Binded!");
        playerCharacter = character;
        playerCharacter.ownedPlayer.playerInteract.OnInteractableChanged += SetPromptText;
        gameObject.SetActive(true);
        RefreshUI();
    }

    public void RefreshUI()
    {
        SetPromptText(string.Empty);
    }

    public void UnbindUI()
    {
        if(playerCharacter != null) 
        {
            playerCharacter.ownedPlayer.playerInteract.OnInteractableChanged -= SetPromptText;
            gameObject.SetActive(false);
            playerCharacter = null;
        }
    }

    void SetPromptText(string text) 
    {
        if(text != string.Empty) 
        {
            promptText.gameObject.SetActive(true);
            promptText.text = $"<b>[F]</b> : {text}";
        }
        else 
        {
            promptText.gameObject.SetActive(false);
        }
    }
}
