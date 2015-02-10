using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace Grendel.Editor
{
    [InitializeOnLoad]
    public class GrendelHierarchyView
    {
        private static float sIconWidth = 16f;
        private static float sIconBufferWidth = 2f;
        private static float sIconRightMargin = 8f;
        private static float sIndentWidth = 14f;

        private static string sLockButtonStyleName = "IN LockButton";
        private static GUIStyle sLockButtonStyle = null;

        private static GUIStyle sHideButtonStyle = null;
        private static GUIContent sHideButtonContent = null;
        private static bool testHideShow = false;

        private static GUIStyle sTreeViewStyle = null;
        private static float sTreeViewOffset = 24f;

        private static int sCurrentParentCount = 0;
        private static int sCurrentChildCount = 0;
        private static int sCurrentIndentAmount = 0;
        private static int sPreviousIndentAmount = 0;

        static GrendelHierarchyView()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        private static void SetupStyles()
        {
            sLockButtonStyle = sLockButtonStyleName;

            sHideButtonStyle = new GUIStyle(EditorStyles.whiteLabel);
            sHideButtonStyle.fontSize = (int)sIconWidth;

            sTreeViewStyle = new GUIStyle(GUIStyle.none);
            sTreeViewStyle.fontSize = 13;
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect position)
        {
            sCurrentIndentAmount = (int)( (position.x / sIndentWidth) -  1);

            if (sLockButtonStyle == null)
            {
                SetupStyles();
            }

            GameObject gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

            sCurrentChildCount = gameObject.transform.childCount;
            sCurrentParentCount = gameObject.GetComponentsInParent<Transform>().Length - 1;

            Rect iconPosition = new Rect(position);
            iconPosition.width = sIconWidth;
            iconPosition.x = (position.width - sIconWidth) + sIconRightMargin;

            DrawLockButton(gameObject, iconPosition);

            iconPosition.x -= (sIconWidth + sIconBufferWidth);

            DrawHideButton(gameObject, iconPosition);

            iconPosition.x -= (sIconWidth + sIconBufferWidth);

            DrawTreeBranch(gameObject, position);

            DrawPreview(gameObject, position);

            sPreviousIndentAmount = sCurrentIndentAmount;
        }

        private static void DrawLockButton(GameObject gameObject, Rect iconPosition)
        {
            iconPosition.x += (sIndentWidth * sCurrentIndentAmount);

            GUI.Toggle(iconPosition, false, string.Empty, sLockButtonStyleName);
        }

        private static void DrawHideButton(GameObject gameObject, Rect iconPosition)
        {
            iconPosition.x += (sIndentWidth * sCurrentIndentAmount);

            GUI.color = testHideShow ? Color.gray : Color.white;

            sHideButtonContent = testHideShow ?
                EditorGUIUtility.IconContent("ViewToolOrbit On") :
                EditorGUIUtility.IconContent("ViewToolOrbit");

            sHideButtonContent.tooltip = "Hide / Show Object";

            testHideShow = GUI.Toggle(iconPosition, testHideShow, sHideButtonContent, sHideButtonStyle);
            
            GUI.contentColor = Color.white;
        }

        private static void DrawTreeBranch(GameObject gameObject, Rect iconPosition)
        {
            iconPosition.x -= sTreeViewOffset;

            if (sCurrentParentCount <= 0)
            {
                return;
            }

            sTreeViewStyle.normal.textColor = Color.gray;

            GUI.Label(iconPosition, "├", sTreeViewStyle);

            if (sCurrentIndentAmount <= 1)
            {
                return;
            }

            float xPos = iconPosition.x;

            sTreeViewStyle.normal.textColor = Color.gray;

            for (int i = 1; i < sCurrentIndentAmount; i++)
            {
                iconPosition.x = (xPos - (sIndentWidth * i)) + 1f;
                GUI.Label(iconPosition, "¦", sTreeViewStyle);
            }          
        }

        private static void DrawPreview(GameObject gameObject, Rect position)
        {
            position.x -= sIconWidth;
            position.width = sIconWidth;

            GUIContent preview = AssetPreview.GetMiniThumbnail(gameObject);

            if (preview.image != null && gameObject.transform.childCount == 0)
            {
                GUI.DrawTexture(position, preview.image);
            }
        }
    }
}
