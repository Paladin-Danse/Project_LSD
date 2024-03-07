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
    [Serializable]
    public struct ExplosionProperty : IEquatable<ExplosionProperty>
    {
        [SerializeField] private int damage;
        [SerializeField] private float amplitude;
        [SerializeField] private float impulse;
        [SerializeField] private float upwardsModifier;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;

        public ExplosionProperty(int damage, float amplitude, float impulse, float upwardsModifier, float minDistance, float maxDistance)
        {
            this.damage = damage;
            this.amplitude = amplitude;
            this.impulse = impulse;
            this.upwardsModifier = upwardsModifier;
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
        }

        public int GetDamage()
        {
            return damage;
        }

        public void SetDamage(int value)
        {
            damage = value;
        }

        public float GetAmplitude()
        {
            return amplitude;
        }

        public void SetAmplitude(float value)
        {
            amplitude = value;
        }

        public float GetImpulse()
        {
            return impulse;
        }

        public void SetImpulse(float value)
        {
            impulse = value;
        }

        public float GetUpwardsModifier()
        {
            return upwardsModifier;
        }

        public void SetUpwardsModifier(float value)
        {
            upwardsModifier = value;
        }

        public float GetMinDistance()
        {
            return minDistance;
        }

        public void SetMinDistance(float value)
        {
            minDistance = value;
        }

        public float GetMaxDistance()
        {
            return maxDistance;
        }

        public void SetMaxDistance(float value)
        {
            maxDistance = value;
        }

        /// <summary>
        /// Empty projectile explosion.
        /// </summary>
        public readonly static ExplosionProperty Empty = new ExplosionProperty(0, 0, 0, 0, 0, 0);

        public static bool operator ==(ExplosionProperty left, ExplosionProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ExplosionProperty left, ExplosionProperty right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is ExplosionProperty metrics) && Equals(metrics);
        }

        public bool Equals(ExplosionProperty other)
        {
            return (minDistance, maxDistance) == (other.minDistance, other.maxDistance);
        }

        public override int GetHashCode()
        {
            return (damage, amplitude, impulse, upwardsModifier, minDistance, maxDistance).GetHashCode();
        }
    }
}