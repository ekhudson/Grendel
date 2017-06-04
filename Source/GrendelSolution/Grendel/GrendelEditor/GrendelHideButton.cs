using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    internal class GrendelHideButton
    {
        private static GUIStyle sHideButtonStyle = null;
        private static GUIContent sVisibleContent = null;
        private static GUIContent sInvisibleContent = null;
        private static Color sSubHiddenColor = Color.gray;
        private const float kSubHiddenAlpha = 0.50f;

        private static void SetupStyles()
        {
            sHideButtonStyle = new GUIStyle(GUIStyle.none);
            sHideButtonStyle.fontSize = (int)GrendelHierarchyView.kIconWidth;
            sHideButtonStyle.alignment = TextAnchor.MiddleCenter;
            sVisibleContent = new GUIContent(GrendelEditorIcons.EyeOpen);
            sVisibleContent.tooltip = sVisibleContent.tooltip = "Hide / Show Object";
            sInvisibleContent = new GUIContent(GrendelEditorIcons.EyeClosed);
            sInvisibleContent.tooltip = sInvisibleContent.tooltip = "Hide / Show Object";               
        }

        public static void DrawHideButton(GrendelObjectData obj, Rect iconPosition)
        {
            if (sHideButtonStyle == null)
            {
                SetupStyles();
            }

            bool hidden = obj.IsHidden;

            Color previousGUIColor = GUI.color;

            if (!obj.gameObject.activeInHierarchy)
            {
                sSubHiddenColor.a = kSubHiddenAlpha;
                GUI.color = sSubHiddenColor;
            }

            hidden = GUI.Toggle(iconPosition, hidden, hidden ? sInvisibleContent : sVisibleContent, sHideButtonStyle);

            if (hidden != obj.IsHidden)
            {
                obj.SetHidden(hidden, true);
                EditorUtility.SetDirty(obj);
                SceneView.RepaintAll();
            }

            if (!obj.gameObject.activeInHierarchy)
            {
                GUI.color = previousGUIColor;
            }
        }
    }
}
