/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;

namespace UnrealFPS.Editor
{
    // [CreateAssetMenu(fileName = "User Helper", menuName = UEditorPaths.EDITOR + "Internal/User Helper", order = 127)]
    public class UserHelper : ScriptableObject
    {
        public const string OFFICIAL_EMAIL = "renownedstudio@gmail.com";
        public const string ONLINE_DOCUMENTATION_URL = "https://docs.google.com/document/d/1H6NnAOCbOH2GLAAi3XyTcFFI0szRGHgk8ysuvuly9Ag/edit";
        public const string OFFICIAL_THREAD_URL = "https://forum.unity.com/threads/unreal-fps-official-thread.461248/";
        public const string OFFICIAL_DISCORD_CHANNEL_URL = "https://discord.gg/sBtPDhb";
        public const string OFFICIAL_TWITTER_URL = "https://twitter.com/RenownedStudio";

        public static void OpenDocumentation()
        {
            if (NetworkIsAvailable())
                Application.OpenURL(ONLINE_DOCUMENTATION_URL);
            else
                Debug.Log("Network is not available open.\nYou can open local documentation in [Unreal FPS/User Helper/Documentation]");
        }

        public static void OpenOnlineDocumentation()
        {
            Application.OpenURL(ONLINE_DOCUMENTATION_URL);
        }

        public static void OpenOfficialThread()
        {
            Application.OpenURL(OFFICIAL_THREAD_URL);
        }

        public static void OpenDiscordChannel()
        {
            Application.OpenURL(OFFICIAL_DISCORD_CHANNEL_URL);
        }

        public static void OpenTwitter()
        {
            Application.OpenURL(OFFICIAL_TWITTER_URL);
        }

        public static bool NetworkIsAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}