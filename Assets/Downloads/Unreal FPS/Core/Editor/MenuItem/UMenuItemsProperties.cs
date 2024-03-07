/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS.Editor
{
    [CreateAssetMenu(fileName = "Menu Items Properties", menuName = UEditorPaths.EDITOR + "Menu Items Properties", order = 132)]
    public class UMenuItemsProperties : ScriptableObject
    {
        #region [Struct Properties]
        [System.Serializable]
        public struct PlayerProperties
        {
            public GameObject player;
            public Vector3 position;
            public Vector3 rotation;
        }

        [System.Serializable]
        public struct WeaponProperties
        {
            public GameObject weapon;
            public Vector3 position;
            public Vector3 rotation;
        }

        [System.Serializable]
        public struct WeaponAmmoProperties
        {
            public GameObject weaponAmmo;
            public Vector3 position;
            public Vector3 rotation;
        }

        [System.Serializable]
        public struct AIProperties
        {
            public GameObject ai;
            public Vector3 position;
            public Vector3 rotation;
        }

        [System.Serializable]
        public struct SpawnZoneProperties
        {
            public GameObject spawnZone;
            public Vector3 position;
            public Vector3 rotation;
        }
        #endregion

        [SerializeField] PlayerProperties playerProperties;
        [SerializeField] WeaponProperties weaponProperties;
        [SerializeField] WeaponAmmoProperties weaponAmmoProperties;
        [SerializeField] AIProperties aiProperties;
        [SerializeField] SpawnZoneProperties spawnZoneProperties;

        #region [Getter / Setter]
        public PlayerProperties GetPlayerProperties()
        {
            return playerProperties;
        }

        public void SetPlayerProperties(PlayerProperties value)
        {
            playerProperties = value;
        }

        public WeaponProperties GetWeaponProperties()
        {
            return weaponProperties;
        }

        public void SetWeaponProperties(WeaponProperties value)
        {
            weaponProperties = value;
        }

        public WeaponAmmoProperties GetWeaponAmmoProperties()
        {
            return weaponAmmoProperties;
        }

        public void SetWeaponAmmoProperties(WeaponAmmoProperties value)
        {
            weaponAmmoProperties = value;
        }

        public AIProperties GetAIProperties()
        {
            return aiProperties;
        }

        public void SetAIProperties(AIProperties value)
        {
            aiProperties = value;
        }

        public SpawnZoneProperties GetSpawnZoneProperties()
        {
            return spawnZoneProperties;
        }

        public void SetSpawnZoneProperties(SpawnZoneProperties value)
        {
            spawnZoneProperties = value;
        }
        #endregion
    }
}