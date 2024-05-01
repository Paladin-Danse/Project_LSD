using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DesertBossCutSceneArea : MonoBehaviour
{
    public PlayableDirector pd;
    public GameObject pdObj;
    public DesertBossCutSceneCamera desertBossCutSceneCamera;
    public PlayerSpawnPoint spawnPoint;
    PlayerCharacter pc;
    //void Start()
    //{
    //    pd = GetComponentInChildren<PlayableDirector>();        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            pd.Play();
            pc = other.gameObject.GetComponent<PlayerCharacter>();
            SoundManager.instance.bgSound.Pause();
            Player.Instance.OnControllUI();
            UIController.Instance.Push("EmptyCanvas", EUIShowMode.Single);
            desertBossCutSceneCamera.DesertDungeonCutSceneCameraPlay();
            //Invoke("GamePlay", 18.5f);
            StartCoroutine(GamePlay());
            GameManager.Instance.PauseGame();
        }

    }

    IEnumerator GamePlay()
    {
        yield return YieldCacher.WaitForSecondsRealtime(16.35f);

        GameManager.Instance.ResumeGame();

        SoundManager.instance.bgSound.Play();
        UIController.Instance.Pop();
        Player.Instance.OnControllCharacter();

        gameObject.SetActive(false);
    }

    public void CutSceneSkip()
    {
        pd.Stop();
        pdObj.SetActive(false);
        // DesertBossCutSceneCamera.Instance.DesertDungeonCutSceneCameraStop();
    }

}
