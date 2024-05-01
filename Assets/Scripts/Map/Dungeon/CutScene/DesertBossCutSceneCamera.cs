using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DesertBossCutSceneCamera : MonoBehaviour
{
    public PlayableDirector TLCamera;

    private void Awake()
    {
        TimelineAsset timelineAsset = (TimelineAsset)TLCamera.playableAsset;
        foreach (var i in timelineAsset.outputs) 
        {
            if(i.streamName == "Cinemachine Track") 
            {
                TLCamera.SetGenericBinding(i.sourceObject, Camera.main.GetComponent<CinemachineBrain>());
                break;
            }
        }
    }

    public void DesertDungeonCutSceneCameraPlay()
    {
        TLCamera.Play();
    }

    public void DesertDungeonCutSceneCameraStop()
    {
        TLCamera.Stop();
    }
}
