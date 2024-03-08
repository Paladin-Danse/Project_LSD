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
    [CreateAssetMenu(fileName = "Editor GUI Properties", menuName = UEditorPaths.EDITOR + "Editor GUI Properties", order = 133)]
    public class UEditorGUIProperties : ScriptableObject
    {
        [SerializeField] private Color headerColor = Color.white;
        [SerializeField] private Color windowColor = Color.gray;
        [SerializeField] private Color boxBackgroundColor = Color.white;
        [SerializeField] private Color boxColor = Color.white;
        [SerializeField] private Color subBboxColor = new Color32(210, 210, 210, 200);

        public virtual void ResetColors()
        {
            headerColor = Color.white;
            windowColor = Color.gray;
            boxBackgroundColor = Color.white;
            boxColor = Color.white;
            subBboxColor = new Color32(210, 210, 210, 200);
        }

        public Color GetHeaderColor()
        {
            return headerColor;
        }

        public void SetHeaderColor(Color value)
        {
            headerColor = value;
        }

        public Color GetWindowColor()
        {
            return windowColor;
        }

        public void SetWindowColor(Color value)
        {
            windowColor = value;
        }

        public Color GetBoxBackgroundColor()
        {
            return boxBackgroundColor;
        }

        public void SetBoxBackgroundColor(Color value)
        {
            boxBackgroundColor = value;
        }

        public Color GetBoxColor()
        {
            return boxColor;
        }

        public void SetBoxColor(Color value)
        {
            boxColor = value;
        }

          public Color GetSubBboxColor()
        {
            return subBboxColor;
        }

        public void SetSubBboxColor(Color value)
        {
            subBboxColor = value;
        }
    }
}