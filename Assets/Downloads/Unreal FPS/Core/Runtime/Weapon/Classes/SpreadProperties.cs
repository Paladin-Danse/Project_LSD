/* ================================================================
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
    [CreateAssetMenu(fileName = "Spread Properties", menuName = UnrealFPSInfo.NAME + "/Weapon/Spread Properties", order = 126)]
    public class SpreadProperties : ScriptableObject, IProperties<SpreadProperty>
    {
        [SerializeField] private SpreadProperty[] properties;

        private Transform firePoint;
        private Vector3 originalRotation;

        public virtual void Initialize(Transform firePoint)
        {
            this.firePoint = firePoint;
            originalRotation = firePoint.eulerAngles;
        }

        public virtual void ApplySpread(WeaponActionState actionState)
        {
            for (int i = 0, length = properties.Length; i < length; i++)
            {
                SpreadProperty property = properties[i];
                if (property.CompareState(actionState))
                {
                    firePoint.localRotation = GetSpreadRotation(property.GetMinBulletSpread(), property.GetMaxBulletSpread());
                    ShakeCamera.Instance.AddShakeEvent(property.GetShakeProperties());
                }
            }
        }

        public void ResetRotation()
        {
            firePoint.localRotation = Quaternion.Euler(Vector3.zero);
        }

        public static Quaternion GetSpreadRotation(float min, float max)
        {
            Vector3 vector = Vector3.zero;
            vector.x += Random.Range(min, max);
            vector.y += Random.Range(min, max);
            vector.z += Random.Range(min, max);
            return Quaternion.Euler(vector);
        }

        public SpreadProperty[] GetProperties()
        {
            return properties;
        }

        public void SetProperties(SpreadProperty[] value)
        {
            properties = value;
        }

        public SpreadProperty GetProperty(int index)
        {
            return properties[index];
        }

        public void SetProperty(int index, SpreadProperty value)
        {
            properties[index] = value;
        }

        public int GetLength()
        {
            return properties != null ? properties.Length : 0;
        }
    }
}