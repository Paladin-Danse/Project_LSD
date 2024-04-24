using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject soundSetting;
    
    public void OnSetting()
    {
        soundSetting.SetActive(!soundSetting.active);
    }

    public void LoadIntroScene()
    {
        SceneLoader.Instance.LoadScene(Defines.EScene.Title);
    }
}
