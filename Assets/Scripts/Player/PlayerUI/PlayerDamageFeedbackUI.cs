using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageFeedbackUI : MonoBehaviour, IPlayerUIInterface
{
    [Header("PlayerDamageFeedbackImage")]
    public Image PlayerDamageFeedbackImage;

    public PlayerCharacter playerCharacter { get; set; }
    bool isCoroutineRunning = false;
    float initialAlpha;

    public float duration = 1f;

    private void Awake()
    {
        initialAlpha = PlayerDamageFeedbackImage.color.a;
    }

    public void BindUI(PlayerCharacter character)
    {
        playerCharacter = character;
        playerCharacter.health.OnTakeDamage += ShowSplash;
    }

    public void UnbindUI()
    {
        playerCharacter.health.OnTakeDamage -= RefreshUI;
    }

    public void RefreshUI()
    {

    }

    public void ShowSplash() 
    {
        if (gameObject.activeInHierarchy) 
        {
            Debug.Log("Call Coroutine!");
            StopCoroutine(DamagedSplash());
            StartCoroutine(DamagedSplash());
        }
    }

    IEnumerator DamagedSplash() 
    {
        PlayerDamageFeedbackImage.gameObject.SetActive(true);
        Debug.Log("CoroutineStart!");

        Color newAlphaColor = PlayerDamageFeedbackImage.color;
        newAlphaColor.a = initialAlpha;
        PlayerDamageFeedbackImage.color = newAlphaColor;
        float reduceAlphaPerSecond = initialAlpha / duration;

        while (PlayerDamageFeedbackImage.color.a > 0) 
        {
            newAlphaColor.a -= reduceAlphaPerSecond / 10;
            PlayerDamageFeedbackImage.color = newAlphaColor;
            yield return YieldCacher.WaitForSeconds(duration / 10);
        }

        Debug.Log("CoroutineEnd!");
        PlayerDamageFeedbackImage.gameObject.SetActive(false);
    }
}
