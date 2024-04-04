using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    AsyncOperation asyncOperation;
    public TMP_Text text;
    public Image filledImage;

    private void Awake()
    {
        text = SceneLoader.Instance.loadingCanvasController.progressText;
        filledImage = SceneLoader.Instance.loadingCanvasController.progressImage;
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        asyncOperation = SceneManager.LoadSceneAsync((int)SceneLoader.Instance.loadSceneContext, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        while(!asyncOperation.isDone) 
        {
            RefreshProgressUI(asyncOperation.progress);
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneLoader.Instance.loadSceneContext));
        Destroy(SceneLoader.Instance.loadingCanvasController.gameObject);
    }

    void RefreshProgressUI(float progress) 
    {
        filledImage.fillAmount = progress;
    }
}
