using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DesertBossCutSceneCamera : MonoBehaviour
{
    private static DesertBossCutSceneCamera instance;
    public static DesertBossCutSceneCamera Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DesertBossCutSceneCamera>();

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(DesertBossCutSceneCamera).Name);
                    instance = obj.AddComponent<DesertBossCutSceneCamera>();
                }
            }
            return instance;
        }
    }

    public PlayableDirector TLCamera;
    public void DesertDungeonCutSceneCameraPlay()
    {
        TLCamera.Play();
    }

    public void DesertDungeonCutSceneCameraStop()
    {
        TLCamera.Stop();
    }
}
