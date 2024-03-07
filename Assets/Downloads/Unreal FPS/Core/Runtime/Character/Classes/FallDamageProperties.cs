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
    public partial class FPHealth
    {
        [System.Serializable]
        public struct FallDamageProperties : IEquatable<FallDamageProperties>
        {
            [SerializeField] private int damage;
            [SerializeField] private float minHeight;
            [SerializeField] private float maxHeight;

            /// <summary>
            /// FallDamage properties constructor.
            /// </summary>
            /// <param name="damage">Damage (in health point value).</param>
            /// <param name="minHeight">Min height position for take damage.</param>
            /// <param name="maxHeight">Max height position for take damage.</param>
            internal FallDamageProperties(int damage, float minHeight, float maxHeight)
            {
                this.damage = damage;
                this.minHeight = minHeight;
                this.maxHeight = maxHeight;
            }

            /// <summary>
            /// Return damage (in health point value).
            /// </summary>
            /// <returns></returns>
            public int GetDamage()
            {
                return damage;
            }

            /// <summary>
            /// Set damage (in health point value).
            /// </summary>
            /// <param name="value"></param>
            public void SetDamage(int value)
            {
                damage = value;
            }

            /// <summary>
            /// Return min height position for take damage.
            /// </summary>
            /// <returns></returns>
            public float GetMinHeight()
            {
                return minHeight;
            }

            /// <summary>
            /// Set min height position for take damage.
            /// </summary>
            /// <param name="value"></param>
            public void SetMinHeight(float value)
            {
                minHeight = value;
            }

            /// <summary>
            /// Return max height position for take damage.
            /// </summary>
            /// <returns></returns>
            public float GetMaxHeight()
            {
                return maxHeight;
            }

            /// <summary>
            /// Set max height position for take damage.
            /// </summary>
            /// <param name="value"></param>
            public void SetMaxHeight(float value)
            {
                maxHeight = value;
            }

            /// <summary>
            /// Empty FallDamage properties.
            /// </summary>
            /// <returns></returns>
            public static FallDamageProperties Empty
            {
                get
                {
                    return new FallDamageProperties(0, 0, 0);
                }
            }

            public static bool operator ==(FallDamageProperties left, FallDamageProperties right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(FallDamageProperties left, FallDamageProperties right)
            {
                return !Equals(left, right);
            }

            public override bool Equals(object obj)
            {
                return (obj is FallDamageProperties metrics) && Equals(metrics);
            }

            public bool Equals(FallDamageProperties other)
            {
                return (damage, minHeight, maxHeight) == (other.damage, other.minHeight, other.maxHeight);
            }

            public override int GetHashCode()
            {
                return (damage, minHeight, maxHeight).GetHashCode();
            }
        }
    }
}