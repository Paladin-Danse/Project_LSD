using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgSound; // 배경음
    public AudioClip[] introBGMList; // 배경음리스트
    public AudioClip[] SafeZoneBGMList;
    public AudioClip[] DesertDungeonBGMList;

    [Header("BtnSound")]
    public AudioClip btnSound;
    public AudioClip btnPushSound;

    [Header("StorySound")]
    public AudioClip storySound;

    public AudioMixer audioMixer;
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // 씬시작시 실행되는 메서드
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {        
        if(arg0.name == "IntroScene")
        {            
            for (int i = 0; i < introBGMList.Length; i++)
            {
                BgSoundPlay(introBGMList[i]);
            }            
        }
        else if(arg0.name == "SafeZoneScene")
        {
            for (int i = 0; i < SafeZoneBGMList.Length; i++)
            {
                BgSoundPlay(SafeZoneBGMList[i]); break;
            }
        }
        else if(arg0.name == "DungeonScene")
        {
            for (int i = 0; i < DesertDungeonBGMList.Length; i++)
            {
                BgSoundPlay(DesertDungeonBGMList[i]); break;
            }
        }
    }



    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }

    public void IntroBtnSound()
    {
        bgSound.PlayOneShot(btnSound);
    }    

    public void PushIntroBtnSound()
    {
        bgSound.PlayOneShot(btnPushSound);
    }
}
