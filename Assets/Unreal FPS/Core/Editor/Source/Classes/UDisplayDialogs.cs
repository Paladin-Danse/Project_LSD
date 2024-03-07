/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEditor;

namespace UnrealFPS.Editor
{
    public static class UDisplayDialogs
    {
        /// <summary>
        /// Unreal FPS dialog message.
        /// </summary>
        /// <param name="message"></param>
        public static bool Message(string title, string message, string ok = "Ok", string cancel = null)
        {
            return EditorUtility.DisplayDialog(UnrealFPSInfo.NAME + ": " + title, message, ok, cancel);
        }

        /// <summary>
        /// Unreal FPS dialog complex message.
        /// </summary>
        /// <param name="message"></param>
        public static int MessageComplex(string title, string message, string ok = "Ok", string cancel = "Cancel", string alt = "Other")
        {
            return EditorUtility.DisplayDialogComplex(UnrealFPSInfo.NAME + ": " + title, message, ok, cancel, alt);
        }

        /// <summary>
        /// Confirmation dialog message.
        /// </summary>
        /// <param name="message"></param>
        public static bool Confirmation(string message)
        {
            return EditorUtility.DisplayDialog(UnrealFPSInfo.NAME + ": Confirmation", message, "Ok", "No");
        }

        /// <summary>
        /// Project not fully configured.
        /// </summary>
        /// <param name="itemName"></param>
        public static int ProjectNotConfigured()
        {
            return EditorUtility.DisplayDialogComplex(UnrealFPSInfo.NAME + ": Project Verifier", "Configure of the project is not finished!\nConfigure the project via Project setup assistant.", "Open assistant", "Ok", "Don't show again");
        }

        /// <summary>
        /// Error create some item from MenuItem menu.
        /// </summary>
        /// <param name="itemName"></param>
        public static bool ErrorCreateItemPropNull(string itemName)
        {
            return EditorUtility.DisplayDialog(UnrealFPSInfo.NAME + ": Create " + itemName + " Error", "Message: " + itemName + " cannot be created...\n\n" +
                "Reason: Menu Items Properties asset not found.\n\n" +
                "Solution: Create Menu Items Properties asset from\n" + UEditorPaths.EDITOR + "Menu Items Properties in Resources/" + UEditorResourcesHelper.PROPETIES_PATH +" folder.", "Ok");
        }

        /// <summary>
        /// Error create some item from MenuItem menu.
        /// </summary>
        /// <param name="itemName"></param>
        public static bool ErrorCreateItemObjNull(string itemName)
        {
            return EditorUtility.DisplayDialog(UnrealFPSInfo.NAME + ": Create " + itemName + " Error", "Message: " + itemName + " cannot be created...\n\n" +
                "Reason: " + itemName + " object not found.\n\n" +
                "Solution: Go to Unreal FPS > UProject Manager\nand fill " + itemName + " GameObject.", "Ok");
        }
    }
}