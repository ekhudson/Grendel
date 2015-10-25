using System;
using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    internal class GrendelLayerColours
    {
        private static Color[] sLayerColors = new Color[]
        {
            Color.green,
            Color.cyan,
            Color.yellow,
            Color.red,
            Color.gray,
            Color.blue,
            Color.magenta,
            GrendelColor.Orange,
            GrendelColor.Purple,
            GrendelColor.Copper,
            GrendelColor.Olive,
            GrendelColor.Mauve,
            GrendelColor.Steel,
            GrendelColor.Gold,
            GrendelColor.Teal,
            GrendelColor.LightOrange,
            GrendelColor.LightPink,
            GrendelColor.OliveLight,
            GrendelColor.GrayLight,
            GrendelColor.Maroon,
            GrendelColor.DarkPink,
            GrendelColor.Rose,
            GrendelColor.Denim,
            GrendelColor.Canary,    
            GrendelColor.LightPurple,
            GrendelColor.Sand,
            GrendelColor.Peach,
            GrendelColor.Blueberry,
            GrendelColor.Aquamarine,
            GrendelColor.Pink,
            GrendelColor.DarkOrange,
            GrendelColor.OliveDark,
        };

        private static Color sTempColor = Color.white;
        private static Vector2 sScrollPos = Vector2.zero;

        internal static Color GetLayerColor(int layer)
        {
            return sLayerColors[layer];
        }

        [PreferenceItem("Grendel")]
        private static void PreferencesGUI()
        {
            sScrollPos = GUILayout.BeginScrollView(sScrollPos, GUI.skin.box);

            for(int j = 0; j < sLayerColors.Length; j++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(j.ToString());
                EditorGUILayout.ColorField(sLayerColors[j]);
                GUILayout.EndHorizontal();
            }

            sTempColor = EditorGUILayout.ColorField(sTempColor);
            EditorGUILayout.SelectableLabel(sTempColor.ToString());

            GUILayout.EndScrollView();
        }

    }
}
