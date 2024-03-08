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
    [CreateAssetMenu(fileName = "Explosion Properties", menuName = UnrealFPSInfo.NAME + "/Weapon/Explosion Properties", order = 126)]
    public class ExplosionProperties : ScriptableObject, IProperties<ExplosionProperty>
    {
        [SerializeField] private ExplosionProperty[] properties;

        public ExplosionProperty[] GetProperties()
        {
            return properties;
        }

        public ExplosionProperty GetProperty(int index)
        {
            return properties[index];
        }

        public void SetProperties(ExplosionProperty[] properties)
        {
            this.properties = properties;
        }

        public void SetProperty(int index, ExplosionProperty property)
        {
            properties[index] = property;
        }

        public int GetLength()
        {
            return properties != null ? properties.Length : 0;
        }
    }
}