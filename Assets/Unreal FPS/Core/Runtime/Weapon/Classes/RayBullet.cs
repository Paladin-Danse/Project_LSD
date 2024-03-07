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
    [CreateAssetMenu(fileName = "Ray Bullet", menuName = UnrealFPSInfo.NAME + "/Weapon/Ray Bullet", order = 124)]
    public class RayBullet : ScriptableObject, IEquatable<RayBullet>, IBullet
    {
        [SerializeField] private string model;
        [SerializeField] private int damage;
        [SerializeField] private float variance;
        [SerializeField] private int numberbullet = 1;
        [SerializeField] private DecalProperties decalProperties;

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
            value = Mathf.Abs(value);
            damage = value;
        }

        public float GetVariance()
        {
            return variance;
        }

        public void SetVariance(float value)
        {
            variance = value;
        }

        public int GetNumberBullet()
        {
            return numberbullet;
        }

        public void SetNumberBullet(int value)
        {
            if (numberbullet < 1)
                numberbullet = 1;
            else
                numberbullet = value;
        }

        public DecalProperties GetDecalProperties()
        {
            return decalProperties;
        }

        public void SetDecalProperties(DecalProperties value)
        {
            decalProperties = value;
        }

        public static bool operator ==(RayBullet left, RayBullet right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RayBullet left, RayBullet right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is RayBullet metrics) && Equals(metrics);
        }

        public bool Equals(RayBullet other)
        {
            return (model, damage) == (other.model, other.damage);
        }

        public override int GetHashCode()
        {
            return (model, damage, variance, numberbullet, decalProperties).GetHashCode();
        }
    }
}