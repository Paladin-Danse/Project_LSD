using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;


public class SceneLoader
{
    private static readonly SceneLoader instance = new SceneLoader();
    public static SceneLoader Instance { get { return instance; } }

    public Defines.EScene loadSceneContext = Defines.EScene.Unknown;

    public LoadingCanvasController loadingCanvasController;

    public void LoadScene(Defines.EScene scene, LoadSceneMode loadSceneModev = LoadSceneMode.Single) 
    {
        SceneManagerBase sceneManager = GameObject.FindObjectOfType<SceneManagerBase>();
        if (sceneManager != null)
        {
            Debug.Log("SceneManager exist");
        }

        Player.Instance.UnPossess();
        ObjectPoolManager.Instance.ClearPools();
        UIController.Instance.Clear();

        UIController.Instance.Push<LoadingCanvasController>("LoadingCanvas", out loadingCanvasController);
        loadSceneContext = scene;
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);
    }
}
