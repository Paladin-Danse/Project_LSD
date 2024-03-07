/* =====================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS.Runtime
{
    public partial class WeaponShootingSystem
    {
        [System.Serializable]
        public struct SoundProperties
        {
            [SerializeField] private AudioClip[] fireSounds;
            [SerializeField] private AudioClip[] emptySounds;

            public AudioClip[] GetFireSounds()
            {
                return fireSounds;
            }

            public void SetFireSounds(AudioClip[] value)
            {
                fireSounds = value;
            }

            public AudioClip GetFireSound(int index)
            {
                return fireSounds[index];
            }

            public void SetFireSound(int index, AudioClip value)
            {
                fireSounds[index] = value;
            }

            public int GetFireSoundsCount()
            {
                return fireSounds.Length;
            }

            public AudioClip[] GetEmptySounds()
            {
                return emptySounds;
            }

            public void SetEmptySounds(AudioClip[] value)
            {
                emptySounds = value;
            }

            public AudioClip GetEmptySound(int index)
            {
                return emptySounds[index];
            }

            public void SetEmptySound(int index, AudioClip value)
            {
                emptySounds[index] = value;
            }

            public int GetEmptySoundsCount()
            {
                return emptySounds.Length;
            }

            public AudioClip GetRandomFireSound()
            {
                if (fireSounds == null || fireSounds.Length == 0)
                    return null;
                return fireSounds[Random.Range(0, fireSounds.Length)];
            }

            public AudioClip GetRandomEmptySound()
            {
                if (emptySounds == null || emptySounds.Length == 0)
                    return null;
                return emptySounds[Random.Range(0, emptySounds.Length)];
            }
        }
    }
}