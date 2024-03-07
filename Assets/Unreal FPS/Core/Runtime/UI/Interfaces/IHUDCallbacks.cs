/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.UI;

namespace UnrealFPS.UI
{
    /// <summary>
    /// IHUDCallback interface contains required functions for the HUDManager.  
    /// </summary>
    public interface IHUDCallbacks
    {
        /// <summary>
        /// Processing health point on the HUD components.
        /// </summary>
        /// <param name="health">Player health point</param>
        void HealthProcessing(int health, int maxHealth);

        /// <summary>
        /// Processing weapon information on the HUD components.
        /// </summary>
        /// <param name="name">Weapon name.</param>
        /// <param name="weaponSprite">Weapon sprite.</param>
        void WeaponProcessing(string name, Sprite weaponSprite = null);

        /// <summary>
        /// Processing ammo information on the HUD components.
        /// </summary>
        /// <param name="bulletCount">Weapon bullet count.</param>
        /// <param name="clipCount">Weapon clip count.</param>
        void AmmoProcessing(int bulletCount, int clipCount);

        /// <summary>
        /// Display current HUD.
        /// </summary>
        /// 
        /// <remarks>
        /// True - HUD enabled / False - HUD disabled.
        /// </remarks>
        /// <param name="display"></param>
        void DisplayHUD(bool display);

        /// <summary>
        /// Display message on the HUD.
        /// </summary>
        /// <param name="message">Message text</param>
        void DisplayMessage(string message);

        /// <summary>
        /// Hide message from the HUD.
        /// </summary>
        void HideMessage();

        /// <summary>
        /// Player HUD components.
        /// </summary>
        HUDComponents GetHUDComponents();
    }
}