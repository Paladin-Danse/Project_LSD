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
using UnrealFPS.Utility;

namespace UnrealFPS.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class Grenade : MonoBehaviour, IGrenade, IPoolDestroyer
    {
        [SerializeField] private GameObject grenadeObject;
        [SerializeField] private float radius = 1.25f;
        [SerializeField] private ParticleSystem explosionEffect;
        [SerializeField] private AudioClip explosionSound;
        [SerializeField] private ExplosionProperties explosionProperties;
        [SerializeField] private DecalProperties decalProperties;
        [SerializeField] private float delay = 5.0f;

        private AudioSource audioSource;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected virtual void OnEnable()
        {
            StartCoroutine(StartDelay(delay));
        }

        /// <summary>
        /// Explosion processing.
        /// </summary>
        public virtual void Explosion()
        {
            Vector3 position = transform.position;
            Collider[] colliders = Physics.OverlapSphere(position, radius);
            for (int i = 0, length = colliders.Length; i < length; i++)
            {
                Collider collider = colliders[i];
                float distance = Vector3.Distance(position, collider.transform.position);
                ExplosionProperty projectileExplosion = GetProjectileExplosion(distance);
                IHealth health = collider.GetComponent<IHealth>();
                Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
                if (health != null)
                {
                    health.TakeDamage(projectileExplosion.GetDamage());
                }
                if (rigidbody != null)
                {
                    rigidbody.AddExplosionForce(projectileExplosion.GetImpulse(),
                        position,
                        radius,
                        projectileExplosion.GetUpwardsModifier(),
                        ForceMode.Impulse);
                }

                if (collider.CompareTag(TNC.PLAYER))
                {
                    ShakeCamera.ShakeProperties shakeProperties = ShakeCamera.ExplosionShakeProperties(radius, distance);
                    ShakeCamera.Instance.AddShakeEvent(ShakeCamera.ExplosionShakeProperties());
                }

                explosionEffect.Play();
                audioSource.PlayOneShot(explosionSound);
            }

            RaycastHit hit;
            if (Physics.Raycast(position, -Vector3.up, out hit, radius))
            {
                DecalProperty decalProperty = DecalHelper.GetDecalPropertyBySurface(decalProperties, hit);
                GameObject decal = decalProperty.GetRandomDecal();
                AudioClip decalSoundEffect = decalProperty.GetRandomSound();

                if (decal != null)
                {
                    PoolManager.Instantiate(decal, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if (decalSoundEffect != null)
                {
                    audioSource.clip = decalSoundEffect;
                    audioSource.Play();
                }
            }
        }

        public virtual IEnumerator StartDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            grenadeObject.SetActive(false);
            Explosion();
            float effectDelay = Mathf.Max(explosionEffect.main.duration, explosionSound.length, audioSource.clip != null ? audioSource.clip.length : 0);
            yield return new WaitForSeconds(effectDelay);
            gameObject.SetActive(false);
            grenadeObject.SetActive(true);
        }

        /// <summary>
        /// Get projectile explosion by distance.
        /// </summary>
        protected ExplosionProperty GetProjectileExplosion(float distance)
        {
            for (int i = 0, length = explosionProperties.GetLength(); i < length; i++)
            {
                ExplosionProperty projectileExplosion = explosionProperties.GetProperty(i);
                if (UMathf.InRange(distance, projectileExplosion.GetMinDistance(), projectileExplosion.GetMaxDistance()))
                {
                    return projectileExplosion;
                }
            }
            return Runtime.ExplosionProperty.Empty;
        }

        public GameObject GetGrenadeObject()
        {
            return grenadeObject;
        }

        public void SetGrenadeObject(GameObject value)
        {
            grenadeObject = value;
        }

        public float GetRadius()
        {
            return radius;
        }

        public void SetRadius(float value)
        {
            radius = value;
        }

        public ParticleSystem GetExplosionEffect()
        {
            return explosionEffect;
        }

        public void SetExplosionEffect(ParticleSystem value)
        {
            explosionEffect = value;
        }

        public AudioClip GetExplosionSound()
        {
            return explosionSound;
        }

        public void SetExplosionSound(AudioClip value)
        {
            explosionSound = value;
        }

        public ExplosionProperties GetExplosionProperties()
        {
            return explosionProperties;
        }

        public void SetExplosionProperties(ExplosionProperties value)
        {
            explosionProperties = value;
        }

        public DecalProperties GetDecalProperties()
        {
            return decalProperties;
        }

        public void SetDecalProperties(DecalProperties value)
        {
            decalProperties = value;
        }

        public float GetDelay()
        {
            return delay;
        }

        public void SetDelay(float value)
        {
            delay = value;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }
    }
}