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
                    return EditorApplication.isPlayingOrWillChangePlaymode ? DarkViewColor * PlaymodeTintColor : DarkViewColor;
                }
                else
                {
                    return EditorApplication.isPlayingOrWillChangePlaymode ? LightViewColor * PlaymodeTintColor : LightViewColor;
                }
            }
        }

        internal static Color PlaymodeTintColor
        {
            get
            {
                Color tintColor = new Color(0.8f, 0.8f, 0.8f, 1f);
                string tintColorString = EditorPrefs.GetString("Playmode tint");
                string[] components = tintColorString.Split(';');

                if (components.Length > 0)
                {
                    float red = 1f;
                    float green = 1f;
                    float blue = 1f;
                    float alpha = 1f;
                    float.TryParse(components[1], out red);
                    float.TryParse(components[2], out green);
                    float.TryParse(components[3], out blue);
                    float.TryParse(components[4], out alpha);
                    tintColor = new Color(red, green, blue, alpha);
                }

                return tintColor;
            }            
        }
    }
}
