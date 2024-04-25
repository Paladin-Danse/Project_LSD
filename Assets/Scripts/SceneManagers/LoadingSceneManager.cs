using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    AsyncOperation asyncOperation;
    public TMP_Text progressText;
    public Image filledImage;

    private void Awake()
    {
        progressText = SceneLoader.Instance.loadingCanvasController.progressText;
        filledImage = SceneLoader.Instance.loadingCanvasController.progressImage;
    }

    private void Start()
    {
        ObjectPoolManager.Instance.ClearPools();
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        asyncOperation = SceneManager.LoadSceneAsync((int)SceneLoader.Instance.loadSceneContext, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        while(!asyncOperation.isDone) 
        {
            RefreshProgressUI(asyncOperation.progress);

            if (asyncOperation.progress >= 0.9f) 
            {
                ChangeProgressText("¾À ·Îµù ¿Ï·á");
                RefreshProgressUI(1.0f);
                GameManager.Instance.ResumeGame();
                yield return YieldCacher.WaitForSeconds(1.0f);
                UIController.Instance.Pop();
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    
    void ChangeProgressText(string text) 
    {
        this.progressText.text = text;
    }

    void RefreshProgressUI(float progress) 
    {
        filledImage.fillAmount = progress;
    }
}
