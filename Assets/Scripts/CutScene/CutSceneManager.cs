using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneManager : MonoBehaviour
{
    private static CutSceneManager instance;
    public static CutSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CutSceneManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(CutSceneManager).Name);
                    instance = obj.AddComponent<CutSceneManager>();
                }
            }
            return instance;
        }
    }

    public PlayableDirector playableDirector;

    public void CutSceneSkip()
    {
        playableDirector.Stop();
        SceneLoader.Instance.LoadScene(Defines.EScene.Dungeon);
    }

    public void ChangeSceneOnCutSceneEnd() 
    {
        Invoke("OnCutSceneEnd", 12f);
    }

    public void OnCutSceneEnd() 
    {
        SceneLoader.Instance.LoadScene(Defines.EScene.Dungeon);
    }    
}
