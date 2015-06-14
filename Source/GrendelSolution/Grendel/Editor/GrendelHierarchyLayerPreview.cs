using System;

using UnityEngine;
using UnityEditor;

namespace Grendel.Editor
{
    internal static class GrendelHierarchyLayerPreview
    {
        private static GUIStyle sLabelStyle;

        internal static void DrawLayerPreview(int layer, Rect iconPosition)
        {
            if (sLabelStyle == null)
            {
                sLabelStyle = new GUIStyle(EditorStyles.miniLabel);
                sLabelStyle.fontSize = 9;
                sLabelStyle.alignment = TextAnchor.MiddleCenter;
            }

            string layerName = LayerMask.LayerToName(layer);
            string shortName = layerName;

            if (shortName.Length > 2)
            {
                shortName = shortName.Remove(2);
            }

            GUIContent labelContent = new GUIContent(shortName, layerName);

            Color previousColor = GUI.color;
            GUI.color = Color.Lerp(GrendelLayerColours.GetLayerColor(layer), GrendelEditorGUIUtility.CurrentSkinViewColor, 0.35f);
            //GUI.DrawTexture(iconPosition, EditorGUIUtility.whiteTexture);
            GUI.Box(iconPosition, EditorGUIUtility.whiteTexture);
            GUI.Label(iconPosition, labelContent, sLabelStyle);
            GUI.color = previousColor;
        }
    }
}
