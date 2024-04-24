using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvasController : MonoBehaviour
{
    public TMP_Text progressText;
    public Image progressImage;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetProgress(float progress) 
    {
        progressImage.fillAmount = progress;
    }

    public void SetProgressText(string newText) 
    {
        progressText.text = newText;
    }
}
