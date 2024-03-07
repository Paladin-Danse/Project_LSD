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
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class PhysicsBullet : MonoBehaviour, IBullet, IPoolDestroyer
    {
        public enum BulletType
        {
            Standard,
            Rocket
        }

        [SerializeField] private BulletType bulletType;
        [SerializeField] private GameObject bullet;
        [SerializeField] private string model;
        [SerializeField] private int damage;
        [SerializeField] private float delay;
        [SerializeField] private int numberBullet = 1;
        [SerializeField] private float variance;
        [SerializeField] private ParticleSystem explosionEffect;
        [SerializeField] private AudioClip explosionSound;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float explosionPower;
        [SerializeField] private DecalProperties decalProperties;

        private AudioSource audioSource;
        private Rigidbody _rigidbody;
        private IEnumerator startDelay;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void OnEnable()
        {
            if (startDelay != null)
            {
                StopCoroutine(startDelay);
            }
            startDelay = StartDelay(delay);
            StartCoroutine(startDelay);
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected virtual void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision Detected!");
            ContactPoint contactPoint = collision.contacts[0];
            DecalProperty decalProperty = DecalHelper.GetDecalPropertyBySurface(decalProperties, contactPoint);
            GameObject decal = decalProperty.GetRandomDecal();
            AudioClip decalSoundEffect = decalProperty.GetRandomSound();
            if (decal != null)
            {
                PoolManager.Instantiate(decal, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            }
            if (decalSoundEffect != null)
            {
                audioSource.clip = decalSoundEffect;
                audioSource.Play();
            }
            if (bulletType == BulletType.Standard)
            {
                ApplyDamage(collision.transform);
            }
            else if (bulletType == BulletType.Rocket)
            {
                Explosion();
                if (explosionSound != null)
                {
                    audioSource.PlayOneShot(explosionSound);
                }

                if (explosionEffect != null)
                {
                    explosionEffect.Play();
                }
            }
            float disableTime = Mathf.Max(explosionSound != null ? explosionSound.length : 0,
                decalSoundEffect != null ? decalSoundEffect.length : 0,
                explosionEffect != null ? explosionEffect.main.duration : 0);
            StartCoroutine(DisableTimeout(disableTime));
        }

        /// <summary>
        /// Explosion Handler
        /// </summary>
        /// <param name="explosionPower"></param>
        /// <param name="explosionRadius"></param>
        protected virtual void Explosion()
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            for (int i = 0, length = colliders.Length; i < length; i++)
            {
                Collider collider = colliders[i];
                IHealth health = collider.GetComponent<IHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }

                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);
                }
            }
        }

        /// <summary>
        /// On Hit Damage Action
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="damageValue"></param>
        protected virtual void ApplyDamage(Transform transform)
        {
            IHealth health = transform.GetComponent<IHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        public IEnumerator StartDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _rigidbody.isKinematic = false;
            bullet.SetActive(true);
            gameObject.SetActive(false);
        }

        public IEnumerator DisableTimeout(float delay)
        {
            bullet.SetActive(false);
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
            yield return new WaitForSeconds(delay);
            bullet.SetActive(true);
            _rigidbody.isKinematic = false;
            gameObject.SetActive(false);
            yield break;
        }

        public GameObject GetBullet()
        {
            return bullet;
        }

        public void SetBullet(GameObject value)
        {
            bullet = value;
        }

        public string GetModel()
        {
            return model;
        }

        public void SetModel(string value)
        {
            model = value;
        }

        public int GetDamage()
        {
            return damage;
        }

        public void SetDamage(int value)
        {
            damage = value;
        }

        public float GetLifetime()
        {
            return delay;
        }

        public void SetLifetime(float value)
        {
            delay = value;
        }

        public int GetNumberBullet()
        {
            return numberBullet;
        }

        public void SetNumberBullet(int value)
        {
            numberBullet = value;
        }

        public float GetVariance()
        {
            return variance;
        }

        public void SetVariance(float value)
        {
            variance = value;
        }

        public BulletType GetBulletType()
        {
            return bulletType;
        }

        public void SetBulletType(BulletType value)
        {
            bulletType = value;
        }

        public ParticleSystem GetExplosionEffect()
        {
            return explosionEffect;
        }

        public void SetExplosionEffect(ParticleSystem value)
        {
            explosionEffect = value;
        }

        public float GetExplosionRadius()
        {
            return explosionRadius;
        }

        public void SetExplosionRadius(float value)
        {
            explosionRadius = value;
        }

        public float GetExplosionPower()
        {
            return explosionPower;
        }

        public void SetExplosionPower(float value)
        {
            explosionPower = value;
        }

        public AudioClip GetExplosionSound()
        {
            return explosionSound;
        }

        public void SetExplosionSound(AudioClip value)
        {
            explosionSound = value;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        public void SetAudioSource(AudioSource value)
        {
            audioSource = value;
        }

        public DecalProperties GetDecalProperties()
        {
            return decalProperties;
        }

        public void SetDecalProperties(DecalProperties value)
        {
            decalProperties = value;
        }
    }
}