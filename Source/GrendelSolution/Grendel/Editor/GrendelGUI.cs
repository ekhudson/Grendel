using System;

using UnityEngine;
using UnityEditor;

namespace Grendel.Editor
{
    internal static class GrendelGUI
    {
        private static GUIStyle sTempStyle;
        private static GUIStyle sDividerStyle = null;

        internal static GUIStyle DividerStyle
        {
            get
            {
                if (sDividerStyle == null)
                {
                    sDividerStyle = new GUIStyle(GUIStyle.none);
                    sDividerStyle.normal.background = EditorGUIUtility.whiteTexture;
                }

                return sDividerStyle;
            }
        }

        internal static void ShadedLabel(Rect position, string content, Color textColor, Color shadowColor)
        {
            ShadedLabel(position, new GUIContent(content), textColor, shadowColor, Vector2.one, null);
        }

        internal static void ShadedLabel(Rect position, string content, Color textColor, Color shadowColor, Vector2 shadowOffset)
        {
            ShadedLabel(position, new GUIContent(content), textColor, shadowColor, shadowOffset, null);
        }

        internal static void ShadedLabel(Rect position, GUIContent content, Color textColor, Color shadowColor, Vector2 shadowOffset)
        {
            ShadedLabel(position, content, textColor, shadowColor, shadowOffset, null);
        }

        internal static void ShadedLabel(Rect position, string content, Color textColor, Color shadowColor, Vector2 shadowOffset, GUIStyle style)
        {
            ShadedLabel(position, new GUIContent(content), textColor, shadowColor, shadowOffset, style);
        }

        internal static void ShadedLabel(Rect position, GUIContent content, Color textColor, Color shadowColor, Vector2 shadowOffset, GUIStyle style)
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

            Color originalColor = GUI.color;
            GUI.color = shadowColor;
            GUI.Label(shadowPos, content, sTempStyle);
            GUI.color = originalColor;

            sTempStyle.normal.textColor = textColor;            
           
            GUI.Label(position, content, sTempStyle);            
        }

        internal static void GUILine(Vector2 position, float width, float height, Color lineColor)
        {
            Rect lineRect = new Rect(position.x, position.y, width, height);
            GUILine(lineRect, lineColor);
        }

        internal static void GUILine(Rect position, Color lineColor)
        {
            Color originalGUIColor = GUI.color;

            GUI.color = lineColor;

            GUI.Box(position, string.Empty, DividerStyle);

            GUI.color = originalGUIColor;
        }

        internal static void ShadedGUILine(Vector2 position, float width, float height, Color lineColor, Color shadowColor, Vector2 shadowOffset)
        {
            Rect lineRect = new Rect(position.x, position.y, width, height);
            ShadedGUILine(lineRect, lineColor, shadowColor, shadowOffset);
        }

        internal static void ShadedGUILine(Vector2 position, float width, float height, Color lineColor, Color shadowColor)
        {
            ShadedGUILine(position, width, height, lineColor, shadowColor, Vector2.one);
        }

        internal static void ShadedGUILine(Rect position, Color lineColor, Color shadowColor, Vector2 shadowOffset)
        {
            Color originalGUIColor = GUI.color;
            Rect shadowRect = new Rect(position);
            shadowRect.center += shadowOffset;

            GUILine(shadowRect, shadowColor);

            GUILine(position, lineColor);
        }
    }
}
