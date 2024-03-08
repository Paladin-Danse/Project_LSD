/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnrealFPS.Runtime;

namespace UnrealFPS.UI
{
    /// <summary>
    /// Player HUD Components.
    /// </summary>
    [System.Serializable]
    public struct HUDComponents
    {
        public RectTransform hudWindow;
        public Scrollbar healthScrollbar;
        public Text healthPoint;
        public Text bulletCount;
        public Text clipCount;
        public Text weaponName;
        public Image weaponImage;
        public RectTransform messageWindow;
        public Text message;
    }

    /// <summary>
    /// Player UI HUD Manager.
    /// </summary>
    [ExecuteInEditMode]
    public class HUDManager : MonoBehaviour, IHUDCallbacks
    {
        [SerializeField] private Transform player;
        [SerializeField] private HUDComponents components;

        private IInventory inventory;
        private IHealth health;

        private IWeaponReloading weaponReloading;
        private WeaponID lastWeaponID;

        int lastHealth;
        int lastBulletCount;
        int lastClipCount;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            if (player != null)
            {
                inventory = player.GetComponent<IInventory>();
                health = player.GetComponent<IHealth>();
                if (inventory != null)
                {
                    lastWeaponID = inventory.GetActiveWeaponID();
                    weaponReloading = inventory.GetActiveWeaponTransform() != null ? inventory.GetActiveWeaponTransform().GetComponent<IWeaponReloading>() : null;
                }
            }
        }

        protected virtual void LateUpdate()
        {
            if (inventory != null)
            {
                if (inventory.GetActiveWeaponID() != lastWeaponID)
                {
                    lastWeaponID = inventory.GetActiveWeaponID();
                    weaponReloading = inventory.GetActiveWeaponTransform() != null ? inventory.GetActiveWeaponTransform().GetComponent<IWeaponReloading>() : null;
                }

                if (lastWeaponID != null)
                {
                    WeaponProcessing(lastWeaponID.GetDisplayName(), lastWeaponID.GetImage());
                    AmmoProcessing(weaponReloading.GetBulletCount(), weaponReloading.GetClipCount());
                }
                else
                {
                    WeaponProcessing("Empty");
                    AmmoProcessing(0, 0);
                }
            }

            if (health != null)
            {
                HealthProcessing(health.GetHealth(), health.GetMaxHealth());
            }
        }

        /// <summary>
        /// Processing health point on the HUD components.
        /// </summary>
        /// <param name="health">Player health point</param>
        public virtual void HealthProcessing(int health, int maxHealth)
        {
            if (lastHealth == health)
                return;

            if (components.healthPoint != null)
                components.healthPoint.text = health.ToString() + "%";
            if (components.healthScrollbar != null)
                components.healthScrollbar.size = Mathf.InverseLerp(0, maxHealth, health);
            lastHealth = health;
        }

        /// <summary>
        /// Processing weapon information on the HUD components.
        /// </summary>
        /// <param name="name">Weapon name.</param>
        /// <param name="weaponSprite">Weapon sprite.</param>
        public virtual void WeaponProcessing(string name, Sprite weaponSprite = null)
        {
            if (components.weaponName != null && name != "")
                components.weaponName.text = name;
            else if (name == "")
                components.weaponName.text = "UnNamed Weapon: " + Random.Range(0, 99);

            if (components.weaponImage != null && weaponSprite != null)
                components.weaponImage.sprite = weaponSprite;
        }

        /// <summary>
        /// Processing ammo information on the HUD components.
        /// </summary>
        /// <param name="bulletCount">Weapon bullet count.</param>
        /// <param name="clipCount">Weapon clip count.</param>
        public virtual void AmmoProcessing(int bulletCount, int clipCount)
        {
            if (lastBulletCount != bulletCount && components.bulletCount != null)
            {
                components.bulletCount.text = bulletCount.ToString();
                lastBulletCount = bulletCount;
            }
            if (lastClipCount != clipCount && components.clipCount != null)
            {
                components.clipCount.text = clipCount.ToString();
                lastClipCount = clipCount;
            }
        }

        /// <summary>
        /// Display current HUD.
        /// </summary>
        /// 
        /// <remarks>
        /// True - HUD enabled / False - HUD disabled.
        /// </remarks>
        /// <param name="show"></param>
        public virtual void DisplayHUD(bool show)
        {
            if (components.hudWindow != null)
                components.hudWindow.gameObject.SetActive(show);
        }

        /// <summary>
        /// Display message on the HUD.
        /// </summary>
        /// <param name="message">Message text</param>
        public virtual void DisplayMessage(string message)
        {
            if (components.messageWindow == null)
                return;

            components.message.text = message;
            components.messageWindow.gameObject.SetActive(true);
        }

        /// <summary>
        /// Hide message from the HUD.
        /// </summary>
        public virtual void HideMessage()
        {
            if (components.messageWindow == null)
                return;

            components.message.text = "";
            components.messageWindow.gameObject.SetActive(false);
        }

        /// <summary>
        /// Player HUD components instance.
        /// </summary>
        public HUDComponents GetHUDComponents()
        {
            return components;
        }

        public void SetComponents(HUDComponents value)
        {
            components = value;
        }

        public Transform GetPlayer()
        {
            return player;
        }

        public void SetPlayer(Transform value)
        {
            player = value;
        }
    }
}