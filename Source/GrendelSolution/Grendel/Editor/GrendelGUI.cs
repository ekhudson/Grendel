using System;

using UnityEngine;
using UnityEditor;

namespace Grendel.Editor
{
    public static class GrendelGUI
    {
        private static GUIStyle sTempStyle;

        public static void ShadedLabel(Rect position, string content, Color textColor, Color shadowColor, Vector2 shadowOffset)
        {
            ShadedLabel(position, new GUIContent(content), textColor, shadowColor, shadowOffset, null);
        }

        public static void ShadedLabel(Rect position, GUIContent content, Color textColor, Color shadowColor, Vector2 shadowOffset)
        {
            ShadedLabel(position, content, textColor, shadowColor, shadowOffset, null);
        }

        public static void ShadedLabel(Rect position, string content, Color textColor, Color shadowColor, Vector2 shadowOffset, GUIStyle style)
        {
            ShadedLabel(position, new GUIContent(content), textColor, shadowColor, shadowOffset, style);
        }

        public static void ShadedLabel(Rect position, GUIContent content, Color textColor, Color shadowColor, Vector2 shadowOffset, GUIStyle style)
        {
            if (style != null)
            {
                sTempStyle = new GUIStyle(style);
            }
            else
            {
                sTempStyle = new GUIStyle(GUI.skin.label);
            }


            Rect shadowPos = new Rect(position);

            shadowPos.center += shadowOffset;

            sTempStyle.normal.textColor = shadowColor;

            GUI.Label(shadowPos, content, sTempStyle);

            sTempStyle.normal.textColor = textColor;

            GUI.Label(position, content, sTempStyle);
        }
    }
}
