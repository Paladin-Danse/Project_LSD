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
    public struct SpreadProperty : IEquatable<SpreadProperty>
    {
        [SerializeField] private WeaponActionState actionState;
        [SerializeField] private float minBulletSpread;
        [SerializeField] private float maxBulletSpread;
        [SerializeField] private ShakeCamera.ShakeProperties shakeProperties;

        public SpreadProperty(WeaponActionState actionState, float minBulletSpread, float maxBulletSpread, ShakeCamera.ShakeProperties shakeProperties)
        {
            this.actionState = actionState;
            this.minBulletSpread = minBulletSpread;
            this.maxBulletSpread = maxBulletSpread;
            this.shakeProperties = shakeProperties;
        }

        public WeaponActionState GetActionState()
        {
            return actionState;
        }

        public void SetActionState(WeaponActionState value)
        {
            actionState = value;
        }

        public bool CompareState(WeaponActionState actionState)
        {
            return this.actionState == actionState;
        }

        public float GetMinBulletSpread()
        {
            return minBulletSpread;
        }

        public void SetMinBulletSpread(float value)
        {
            minBulletSpread = value;
        }

        public float GetMaxBulletSpread()
        {
            return maxBulletSpread;
        }

        public void SetMaxBulletSpread(float value)
        {
            maxBulletSpread = value;
        }

        public ShakeCamera.ShakeProperties GetShakeProperties()
        {
            return shakeProperties;
        }

        public void SetShakeProperties(ShakeCamera.ShakeProperties value)
        {
            shakeProperties = value;
        }

        public readonly static SpreadProperty Empty = new SpreadProperty(WeaponActionState.Idle, 0, 0, ShakeCamera.ShakeProperties.Empty);

        public static bool operator ==(SpreadProperty left, SpreadProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SpreadProperty left, SpreadProperty right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is SpreadProperty metrics) && Equals(metrics);
        }

        public bool Equals(SpreadProperty other)
        {
            return (actionState, shakeProperties.GetTarget()) == (other.actionState, other.shakeProperties.GetTarget());
        }

        public override int GetHashCode()
        {
            return (actionState, shakeProperties).GetHashCode();
        }
    }
}