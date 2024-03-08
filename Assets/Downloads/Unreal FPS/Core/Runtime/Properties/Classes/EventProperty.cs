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

namespace UnrealFPS.Runtime
{
    public partial struct AnimationEventProperty
    {
        [Serializable]
        public struct EventProperty : IEquatable<EventProperty>
        {
            [SerializeField] private float animationTime;
            [SerializeField] private AudioClip soundEffect;
            [SerializeField] private ShakeCamera.ShakeProperties shakeProperties;

            /// <summary>
            /// Constructor of the event property struct.
            /// </summary>
            public EventProperty(float animationTime, AudioClip soundEffect, ShakeCamera.ShakeProperties shakeProperties)
            {
                this.animationTime = animationTime;
                this.soundEffect = soundEffect;
                this.shakeProperties = shakeProperties;
            }

            public float GetAnimationTime()
            {
                return animationTime;
            }

            public void SetAnimationTime(float value)
            {
                animationTime = value;
            }

            public AudioClip GetSoundEffect()
            {
                return soundEffect;
            }

            public void SetSoundEffect(AudioClip value)
            {
                soundEffect = value;
            }

            public ShakeCamera.ShakeProperties GetShakeProperties()
            {
                return shakeProperties;
            }

            public void SetShakeProperties(ShakeCamera.ShakeProperties value)
            {
                shakeProperties = value;
            }

            /// <summary>
            /// Empty event property.
            /// </summary>
            public readonly static EventProperty Empty = new EventProperty(-1, null, ShakeCamera.ShakeProperties.Empty);


            public static bool operator ==(EventProperty left, EventProperty right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(EventProperty left, EventProperty right)
            {
                return !Equals(left, right);
            }

            public override bool Equals(object obj)
            {
                return (obj is EventProperty metrics) && Equals(metrics);
            }

            public bool Equals(EventProperty other)
            {
                return (animationTime, soundEffect, shakeProperties) == (other.animationTime, other.soundEffect, other.shakeProperties);
            }

            public override int GetHashCode()
            {
                return (animationTime, soundEffect, shakeProperties).GetHashCode();
            }
        }
    }
}