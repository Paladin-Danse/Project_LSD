/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnrealFPS.UI;
using UnrealFPS.Utility;

namespace UnrealFPS.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class ReSpawnManager : MonoBehaviour, IReSpawnManager
    {
        public enum SpawnType
        {
            /// <summary>
            /// Spawn automatically after respawn delay time passes.
            /// </summary>
            Auto,

            /// <summary>
            ///  Spaw after respawn delay time passes and pressed specific key. 
            /// </summary>
            ByKey
        }

        [System.Serializable]
        public struct ShapeProperties
        {
            public enum Shape
            {
                Rectangle,
                Circle
            }

            public Shape shape;
            public float radius;
            public float length;
            public float weight;
        }

        [System.Serializable]
        public struct SpawnUI
        {
            public RectTransform panel;
            public Text text;
            public string message;
        }

        [SerializeField] private SpawnType spawnType;
        [SerializeField] private Transform player;
        [SerializeField] private int respawnHealth;
        [SerializeField] private Vector3 reSpawnRotation;
        [SerializeField] private bool randomRotation;
        [SerializeField] private float respawnDelay;
        [SerializeField] private ShapeProperties shapeProperties;
        [SerializeField] private KeyCode spawnKey;
        [SerializeField] private AudioClip soundEffect;
        [SerializeField] private SpawnUI spawnUI;

        private IHealth health;
        private AudioSource audioSource;
        private IEnumerator respawn;
        [SerializeField] private float savedTime;
        private bool readyToSpawn;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            health = player.GetComponent<IHealth>();
            audioSource = GetComponent<AudioSource>();
            savedTime = respawnDelay;
        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (!health.IsAlive())
            {
                DipslayMessage(savedTime);
                if (!readyToSpawn)
                {
                    savedTime -= Time.deltaTime;
                    if (savedTime <= 0)
                        readyToSpawn = true;
                }
                else if (readyToSpawn && (spawnType == SpawnType.Auto || (spawnType == SpawnType.ByKey && Input.GetKeyDown(spawnKey))))
                {
                    ReSpawn();
                    readyToSpawn = false;
                    savedTime = respawnDelay;
                }
            }
        }

        /// <summary>
        /// Respawn player after specific time.
        /// </summary>
        public void ReSpawn(float time = 0.0f)
        {
            if (respawn != null)
                return;
            respawn = ReSpawnProcessing(time);
            StartCoroutine(respawn);
        }

        /// <summary>
        /// Respawn player after specific time.
        /// </summary>
        protected virtual IEnumerator ReSpawnProcessing(float time)
        {
            Vector3 targetPosition = Vector3.zero;
            switch (shapeProperties.shape)
            {
                case ShapeProperties.Shape.Circle:
                    targetPosition = UMathf.RandomPositionInCircle(transform.position, shapeProperties.radius);
                    break;
                case ShapeProperties.Shape.Rectangle:
                    targetPosition = UMathf.RandomPositionInRectangle(transform.position, shapeProperties.length, shapeProperties.weight);
                    break;
            }
            Quaternion targetRotation = !randomRotation ? Quaternion.Euler(reSpawnRotation) : Random.rotation;
            if (time > 0) yield return new WaitForSeconds(time);
            health.SetHealth(respawnHealth);
            player.SetPositionAndRotation(targetPosition, targetRotation);
            PlaySoundEffect();
            HideMessage();
            yield break;
        }

        /// <summary>
        /// Play spawn sound effect.
        /// </summary>
        protected void PlaySoundEffect()
        {
            if (soundEffect != null)
                audioSource.PlayOneShot(soundEffect);
        }

        protected void DipslayMessage(float timer)
        {
            if (string.IsNullOrEmpty(spawnUI.message) || spawnUI.panel == null || spawnUI.text == null)
                return;

            string message = "";
            if (timer > 0)
            {
                message = string.Join(": ", spawnUI.message, timer.ToString("0.0"));
            }
            else if (timer <= 0 && spawnType == SpawnType.ByKey)
            {
                message = string.Join(": ", spawnUI.message, spawnKey);
            }
            spawnUI.text.text = message;
            spawnUI.panel.gameObject.SetActive(true);
        }

        protected void HideMessage()
        {
            if (spawnUI.panel == null)
                return;
            spawnUI.panel.gameObject.SetActive(false);
        }

        public ShapeProperties GetShapeProperties()
        {
            return shapeProperties;
        }

        public void SetShapeProperties(ShapeProperties value)
        {
            shapeProperties = value;
        }

        public SpawnType GetSpawnType()
        {
            return spawnType;
        }

        public void SetSpawnType(SpawnType value)
        {
            spawnType = value;
        }

        public Transform GetPlayer()
        {
            return player;
        }

        public void SetPlayer(Transform value)
        {
            player = value;
        }

        public int GetRespawnHealth()
        {
            return respawnHealth;
        }

        public void SetRespawnHealth(int value)
        {
            respawnHealth = value;
        }

        public Vector3 GetReSpawnRotation()
        {
            return reSpawnRotation;
        }

        public void SetReSpawnRotation(Vector3 value)
        {
            reSpawnRotation = value;
        }

        public bool RandomRotation()
        {
            return randomRotation;
        }

        public void RandomRotation(bool value)
        {
            randomRotation = value;
        }

        public float GetRespawnDelay()
        {
            return respawnDelay;
        }

        public void SetRespawnDelay(float value)
        {
            respawnDelay = value;
        }

        public KeyCode GetSpawnKey()
        {
            return spawnKey;
        }

        public void SetSpawnKey(KeyCode value)
        {
            spawnKey = value;
        }

        public AudioClip GetSoundEffect()
        {
            return soundEffect;
        }

        public void SetSoundEffect(AudioClip value)
        {
            soundEffect = value;
        }

        public SpawnUI GetSpawnUI()
        {
            return spawnUI;
        }

        public void SetSpawnUI(SpawnUI value)
        {
            spawnUI = value;
        }

        public IHealth GetHealth()
        {
            return health;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        public void SetAudioSource(AudioSource value)
        {
            audioSource = value;
        }
    }
}