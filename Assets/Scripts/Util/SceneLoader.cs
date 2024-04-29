using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System;


public class SceneLoader
{
    private static readonly SceneLoader instance = new SceneLoader();
    public static SceneLoader Instance { get { return instance; } }

    public Defines.EScene loadSceneContext = Defines.EScene.Unknown;

    public LoadingCanvasController loadingCanvasController;

    public Action LoadingCompleted;

    public void LoadScene(Defines.EScene scene, LoadSceneMode loadSceneModev = LoadSceneMode.Single) 
    {
        if (loadingCanvasController == null)
            loadingCanvasController = Addressables.InstantiateAsync("LoadingCanvas").WaitForCompletion().GetComponent<LoadingCanvasController>();
        else
            loadingCanvasController.gameObject.SetActive(true);

        GameManager.Instance.PauseGame();
        SceneManagerBase sceneManager = GameObject.FindObjectOfType<SceneManagerBase>();
        if (sceneManager != null)
        {
            Debug.Log("SceneManager exist");
            sceneManager.OnUnloadScene();
        }

        if (scene == Defines.EScene.Title)
        {
            GameObject.Destroy(Player.Instance.gameObject);
        }
        loadSceneContext = scene;
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);
    }

    public void LoadCompleted() 
    {
        if(loadingCanvasController != null)
            loadingCanvasController.gameObject.SetActive(false);
    }
}
