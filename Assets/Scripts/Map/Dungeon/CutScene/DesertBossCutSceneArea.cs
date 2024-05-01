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
            pc.rigidbody_.isKinematic = true;
            pc.GetComponent<CapsuleCollider>().enabled = false;
            DesertBossCutSceneCamera.Instance.DesertDungeonCutSceneCameraPlay();
            SoundManager.instance.bgSound.Pause();
            //Player.Instance.UnPossess();
            UIController.Instance.Push("EmptyCanvas", EUIShowMode.Single);
            //Cursor.lockState = CursorLockMode.None;
            Invoke("GamePlay", 18.5f);
        }

    }

    void GamePlay()
    {
        SoundManager.instance.bgSound.Play();
        pc.GetComponent<CapsuleCollider>().enabled = true;
        pc.rigidbody_.isKinematic = false;        
        UIController.Instance.Pop();        
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
