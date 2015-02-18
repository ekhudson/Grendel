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
        private static Vector2 sTreeViewShadowOffset = new Vector2(1, 1);
        private const string kTreeItemTop = "└";
        private const string kTreeItemBottom = "┌";
        private const string kTreeOuterBranch = "¦";
        private static Rect sPreviousItemBranchRect = new Rect();
        private static List<Transform> sCurrentParents = new List<Transform>();
        private static Transform sCurrentTransform = null;
        private static List<GameObject> sCurrentSelectedObjects = new List<GameObject>(); 
        
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

            sCurrentTransform = gameObject.transform;
            sCurrentParents = new List<Transform>(gameObject.GetComponentsInParent<Transform>());

            if (sCurrentParents.Contains(gameObject.transform))
            {
                sCurrentParents.Remove(gameObject.transform);
            }
            
            sCurrentChildCount = gameObject.transform.childCount;
            sCurrentParentCount = sCurrentParents.Count - 1;

            //if (gameObject.transform.parent != null)
            //{
            //    GUI.Label(position, "------------------" + (gameObject.transform.GetSiblingIndex() + 1) + " / " + gameObject.transform.parent.childCount.ToString());
            //}

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
            if (sCurrentParentCount <= 0)
            {
                return;
            }

            iconPosition.x -= sTreeViewOffset;
            Rect shadowPos = new Rect(iconPosition);
            shadowPos.center += sTreeViewShadowOffset;

            bool parentSelected = false;

            foreach(Transform parent in sCurrentParents)
            {
                if (GrendelSelection.SelectedGameObjects.Contains(parent.gameObject))
                {
                    parentSelected = true;
                }
            }

            Color currentTreeColour = parentSelected ? Color.cyan : Color.white;

            GrendelGUI.ShadedLabel(iconPosition, kTreeItemTop, currentTreeColour, Color.gray, sTreeViewShadowOffset, sTreeViewStyle);

            if (sPreviousIndentAmount <= sCurrentIndentAmount && sPreviousIndentAmount > 0)
            {
                GrendelGUI.ShadedLabel(sPreviousItemBranchRect, kTreeItemBottom, currentTreeColour, Color.gray, sTreeViewShadowOffset, sTreeViewStyle);
            }

            sPreviousItemBranchRect = new Rect(iconPosition);

            if (sCurrentIndentAmount <= 1)
            {
                return;
            }

            float xPos = iconPosition.x;

            for (int i = 1; i < sCurrentIndentAmount; i++)
            {
                iconPosition.x = (xPos - (sIndentWidth * i));

                Color outerBranchColor = Color.white;

                for (int j = i - 1; j < sCurrentIndentAmount; j++)
                {
                    if (GrendelSelection.SelectedGameObjects.Contains(sCurrentParents[j].gameObject))
                    {
                        outerBranchColor = Color.cyan;
                        break;
                    }
                }

                    if (gameObject.transform.GetSiblingIndex() == gameObject.transform.parent.childCount - 1 &&
                        i == 1)
                    {
                        if (gameObject.transform.parent.parent != null && gameObject.transform.parent.GetSiblingIndex() != gameObject.transform.parent.parent.childCount - 1)
                        {
                            iconPosition.x += 1;
                            GrendelGUI.ShadedLabel(iconPosition, kTreeOuterBranch, outerBranchColor, Color.gray, sTreeViewShadowOffset, sTreeViewStyle);
                        }
                        else
                        {
                            GrendelGUI.ShadedLabel(iconPosition, kTreeItemTop, currentTreeColour, Color.gray, sTreeViewShadowOffset, sTreeViewStyle);

                            iconPosition.x += 9f;

                            GrendelGUI.ShadedLabel(iconPosition, "–", currentTreeColour, Color.gray, sTreeViewShadowOffset, sTreeViewStyle);
                        }
                    }
                    else if (gameObject.transform)
                    {
                        iconPosition.x += 1;

                        GrendelGUI.ShadedLabel(iconPosition, kTreeOuterBranch, outerBranchColor, Color.gray, sTreeViewShadowOffset, sTreeViewStyle);
                    }
            }
        }

        private static void DrawPreview(GameObject gameObject, Rect position)
        {
            position.x -= sIconWidth;
            position.width = sIconWidth;

            GUIContent preview = new GUIContent();

            preview.image = AssetPreview.GetMiniTypeThumbnail(gameObject.GetType());

            if (preview.image != null && gameObject.transform.childCount == 0)
            {
                GUI.DrawTexture(position, preview.image);
            }
        }
    }
}
