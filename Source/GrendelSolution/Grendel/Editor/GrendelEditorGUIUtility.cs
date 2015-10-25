using System;

using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    internal class GrendelEditorGUIUtility
    {
        private static Color sLightViewColor = Color.clear;
        private static Color sDarkViewColor = Color.clear;

        internal static Color LightViewColor
        {
            get
            {
                if (sLightViewColor == Color.clear)
                {
                    sLightViewColor = new Color(0.76f, 0.76f, 0.76f, 1f);
                }

                return sLightViewColor;
            }
        }

        internal static Color DarkViewColor
        {
            get
            {
                if (sDarkViewColor == Color.clear)
                {
                    sDarkViewColor = new Color(0.22f, 0.22f, 0.22f, 0f);
                }

                return sDarkViewColor;
            }
        }

        internal static Color CurrentSkinViewColor
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                {
                    return DarkViewColor;
                }
                else
                {
                    return LightViewColor;
                }
            }
        }
    }
}
