/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnrealFPS.Runtime
{
    [System.Serializable]
    public struct DecalProperty : IEquatable<DecalProperty>
    {
        [SerializeField] private PhysicMaterial physicMaterial;
        [SerializeField] private Texture2D texture;
        [SerializeField] private GameObject[] decals;
        [SerializeField] private AudioClip[] sounds;

        /// <summary>
        /// Decal property constructor.
        /// </summary>
        /// <param name="physicMaterial">Surface physic material.</param>
        /// <param name="texture">Surface texture.</param>
        /// <param name="decals">Surface decals.</param>
        /// <param name="sounds">Surface hit sounds.</param>
        public DecalProperty(PhysicMaterial physicMaterial, Texture2D texture, GameObject[] decals, AudioClip[] sounds)
        {
            this.physicMaterial = physicMaterial;
            this.texture = texture;
            this.decals = decals;
            this.sounds = sounds;
        }

        /// <summary>
        /// Return surface physic material.
        /// </summary>
        /// <returns></returns>
        public PhysicMaterial GetPhysicMaterial()
        {
            return physicMaterial;
        }

        /// <summary>
        /// Set surface physic material.
        /// </summary>
        /// <param name="value"></param>
        public void SetPhysicMaterial(PhysicMaterial value)
        {
            physicMaterial = value;
        }

        /// <summary>
        /// Return surface texture.
        /// </summary>
        /// <returns></returns>
        public Texture2D GetTexture()
        {
            return texture;
        }

        /// <summary>
        /// Set surface texture.
        /// </summary>
        /// <param name="value"></param>
        public void SetTexture(Texture2D value)
        {
            texture = value;
        }

        /// <summary>
        /// Return surface decals.
        /// </summary>
        /// <returns></returns>
        public GameObject[] GetDecals()
        {
            return decals;
        }

        /// <summary>
        /// Set range surface decals.
        /// </summary>
        /// <param name="value"></param>
        public void SetDecalsRange(GameObject[] value)
        {
            decals = value;
        }

        /// <summary>
        /// Return surface decal.
        /// </summary>
        /// <param name="index">Surface decal index.</param>
        /// <returns></returns>
        public GameObject GetDecal(int index)
        {
            return decals[index];
        }

        /// <summary>
        /// Set surface decal.
        /// </summary>
        /// <param name="index">Surface decal index.</param>
        /// <param name="value"Surface decal.</param>
        public void SetDecal(int index, GameObject value)
        {
            decals[index] = value;
        }

        public GameObject GetRandomDecal()
        {
            if (decals == null || decals.Length == 0)
                return null;
            return decals[Random.Range(0, decals.Length)];
        }

        /// <summary>
        /// Return surface decals array length.
        /// </summary>
        /// <returns></returns>
        public int GetDecalsCount()
        {
            return decals.Length;
        }

        /// <summary>
        /// Return surface hit sounds.
        /// </summary>
        /// <returns></returns>
        public AudioClip[] GetSounds()
        {
            return sounds;
        }

        /// <summary>
        /// Set range surface hit sounds.
        /// </summary>
        /// <param name="value"></param>
        public void SetSoundsRange(AudioClip[] value)
        {
            sounds = value;
        }

        /// <summary>
        /// Return surface hit sound.
        /// </summary>
        /// <param name="index">Surface hit sound index.</param>
        /// <returns></returns>
        public AudioClip GetSound(int index)
        {
            return sounds[index];
        }

        /// <summary>
        /// Set surface hit sound.
        /// </summary>
        /// <param name="index">Surface hit sound index.</param>
        /// <param name="value">Surface hit sound.</param>
        public void SetSound(int index, AudioClip value)
        {
            sounds[index] = value;
        }

        public AudioClip GetRandomSound()
        {
            if (sounds == null || sounds.Length == 0)
                return null;
            return sounds[Random.Range(0, sounds.Length)];
        }

        /// <summary>
        /// Return surface hit sounds array length.
        /// </summary>
        /// <returns></returns>
        public int GetSoundsCount()
        {
            return sounds.Length;
        }

        /// <summary>
        /// Empty Decal property.
        /// </summary>
        /// <returns></returns>
        public readonly static DecalProperty Empty = new DecalProperty(null, null, null, null);

        public static bool operator ==(DecalProperty left, DecalProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DecalProperty left, DecalProperty right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is DecalProperty metrics) && Equals(metrics);
        }

        public bool Equals(DecalProperty other)
        {
            return (physicMaterial, texture) == (other.physicMaterial, other.texture);
        }

        public override int GetHashCode()
        {
            return (physicMaterial, texture, decals, sounds).GetHashCode();
        }
    }
}