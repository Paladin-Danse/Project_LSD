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
    public AudioSource UISound;
    public AudioSource storySoundSource;
    public AudioClip[] introBGMList; // 배경음리스트
    public AudioClip[] SafeZoneBGMList;
    public AudioClip[] DesertDungeonBGMList;    

    [Header("BtnSound")]
    public AudioClip btnSound;
    public AudioClip btnPushSound;

    [Header("StorySound")]
    public AudioClip storySound;

    [Header("DungeonUISound")]
    public AudioClip planetEnterSound;
    public AudioClip planetClickSound;
    public AudioClip dungeonEntranceSound;


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
                PlayRandomBGM(introBGMList);
            }            
        }
        else if(arg0.name == "SafeZoneSceneDev")
        {
            for (int i = 0; i < SafeZoneBGMList.Length; i++)
            {
                PlayRandomBGM(SafeZoneBGMList);
            }
        }
        else if(arg0.name == "DungeonSceneDev")
        {
            for (int i = 0; i < DesertDungeonBGMList.Length; i++)
            {
                PlayRandomBGM(DesertDungeonBGMList);
            }
        }
    }



    //public void BgSoundPlay(AudioClip clip)
    //{
    //    bgSound.clip = clip;
    //    bgSound.loop = true;
    //    bgSound.volume = 0.1f;
    //    bgSound.Play();
    //}

    private void PlayRandomBGM(AudioClip[] bgmList)
    {
        if (bgmList.Length > 0)
        {
            int clipRange = Random.Range(0, bgmList.Length);
            AudioClip randomClip = bgmList[clipRange];
            bgSound.clip = randomClip;
            bgSound.loop = true;
            bgSound.volume = 0.1f;            
            bgSound.Play();
        }
    }

    public void BtnSound()
    {
        UISound.PlayOneShot(btnSound);
    }    

    public void PushBtnSound()
    {
        UISound.PlayOneShot(btnPushSound);
    }

    public void PlanetEnterSound()
    {
        UISound.PlayOneShot(planetEnterSound);
    }

    public void PlanetClickSound()
    {
        UISound.PlayOneShot(planetClickSound);
    }

    public void DungeonEntranceSound()
    {
        UISound.PlayOneShot(dungeonEntranceSound);
    }
}
