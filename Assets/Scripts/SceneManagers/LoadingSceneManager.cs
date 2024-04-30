using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    AsyncOperation asyncOperation;
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
            SceneLoader.Instance.loadingCanvasController.SetProgress(asyncOperation.progress);

            if (asyncOperation.progress >= 0.9f) 
            {
                SceneLoader.Instance.loadingCanvasController.SetProgressText("¾À ·Îµù ¿Ï·á");
                SceneLoader.Instance.loadingCanvasController.SetProgress(1.0f);
                GameManager.Instance.ResumeGame();
                yield return YieldCacher.WaitForSeconds(1.0f);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
