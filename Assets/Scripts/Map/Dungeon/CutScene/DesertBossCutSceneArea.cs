using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DesertBossCutSceneArea : MonoBehaviour
{
    public PlayableDirector pd;
    public GameObject pdObj;
    PlayerCharacter pc;
    //void Start()
    //{
    //    pd = GetComponentInChildren<PlayableDirector>();        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            gameObject.SetActive(false);
            pd.Play();
            pc = other.gameObject.GetComponent<PlayerCharacter>();
            pc.GetComponent<PlayerCharacter>().gameObject.layer = 0;
            DesertBossCutSceneCamera.Instance.DesertDungeonCutSceneCameraPlay();
            SoundManager.instance.bgSound.Pause();
            //Player.Instance.UnPossess();
            //UIController.Instance.Clear();
            //Cursor.lockState = CursorLockMode.None;
            Invoke("GamePlay", 18.5f);
        }

    }

    void GamePlay()
    {
        SoundManager.instance.bgSound.Play();
        pc.GetComponent<PlayerCharacter>().gameObject.layer = 3;
        //UIController.Instance.Pop();
        //Player.Instance.Possess(pc);
        //Player.Instance.OnControllCharacter();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void CutSceneSkip()
    {
        pd.Stop();
        pdObj.SetActive(false);
        DesertBossCutSceneCamera.Instance.DesertDungeonCutSceneCameraStop();
    }

}
