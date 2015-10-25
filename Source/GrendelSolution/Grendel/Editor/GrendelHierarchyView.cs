using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using Grendel.Extensions;

namespace Grendel.Editor
{
    [InitializeOnLoad]
    public class GrendelHierarchyView
    {
        internal const float kIconWidth = 16f;
        internal const float kIconBufferWidth = 2f;
        internal const float kIconRightMargin = 8f;        
        internal const float kIndentWidth = 14f;
        internal const float kHierarchySidebarWidth = 44f;

        private static GUIStyle sItemLabelStyle;
        private static GUIStyle sSelectionRectStyle;

        private static string sLockButtonStyleName = "IN LockButton";
        private static GUIStyle sLockButtonStyle = null;
        
        private static GUIStyle sHideButtonStyle = null;
        private static GUIContent sHideButtonContent = null;

        private static int sCurrentParentCount = 0;
        private static int sCurrentChildCount = 0;
        private static int sCurrentIndentAmount = 0;
        private static int sPreviousIndentAmount = 0;
        private static int sCurrentObjectIndex = 0;
        private static int sTotalCurrentObjectCount = 0;
        private static Color sCurrentRowColor = GrendelEditorGUIUtility.CurrentSkinViewColor;

        private static Rect sPreviousItemPosition = new Rect();

        private static List<Transform> sCurrentParents = new List<Transform>();
        private static Transform sCurrentTransform = null;
        private static List<GameObject> sCurrentSelectedObjects = new List<GameObject>();
        
        private static Color sOddRowColor = Color.Lerp(Color.magenta, GrendelEditorGUIUtility.CurrentSkinViewColor , 0.95f);
        private static Color sLockedLabelColor = Color.Lerp(Color.yellow, GrendelEditorGUIUtility.CurrentSkinViewColor, 0.45f);

        internal static int CurrentParentCount { get { return sCurrentParentCount;  } }
        internal static List<Transform> CurrentParents { get { return sCurrentParents;  } }

        static GrendelHierarchyView()
        {
            SetHierarchyEnabled(GrendelHierarchyPreferences.GrendelHierarchyEnabled);
        }

        internal static void SetHierarchyEnabled(bool enabled)
        {
            if (!enabled)
            {
                EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemOnGUI;
            }
            else if (enabled)
            {
                sCurrentObjectIndex = 0;
                sTotalCurrentObjectCount = 0;
                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
            }
        }

        private static void SetupStyles()
        {
            sItemLabelStyle = new GUIStyle(EditorStyles.label);

            sSelectionRectStyle = "SelectionRect";

            sLockButtonStyle = sLockButtonStyleName;

            sHideButtonStyle = new GUIStyle(EditorStyles.whiteLabel);
            sHideButtonStyle.fontSize = (int)kIconWidth;
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect position)
        {
            sCurrentIndentAmount = (int)( (position.x / kIndentWidth) -  1);

            if (sLockButtonStyle == null)
            {
                SetupStyles();
            }

            GameObject gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

            if (gameObject == null || !GrendelHierarchyPreferences.GrendelHierarchyEnabled)
            {
                return;
            }

            sCurrentTransform = gameObject.transform;
            sCurrentParents = new List<Transform>(gameObject.GetComponentsInParent<Transform>());

            if (sCurrentParents.Contains(gameObject.transform))
            {
                sCurrentParents.Remove(gameObject.transform);
            }

            sCurrentChildCount = gameObject.transform.childCount;
            sCurrentParentCount = sCurrentParents.Count;

            if (sPreviousItemPosition.y > position.y)
            {
                //we're back at the top of the hierarchy
                sTotalCurrentObjectCount = sCurrentObjectIndex;
                sCurrentObjectIndex = 0;               
            }

            ColorNextRow(position);

            //if (gameObject.transform.parent != null)
            //{
            //    GUI.Label(position, "------------------" + sTotalCurrentObjectCount.ToString());
            //} 

            GrendelHierarchyTreeView.DrawTreeBranch(gameObject, position, sCurrentIndentAmount, sPreviousIndentAmount);

            Rect previewPosition = new Rect(position);

            previewPosition.x -= kIconWidth;
            previewPosition.width = kIconWidth;

            GrendelHierarchyPreview.DrawPreview(gameObject, previewPosition, position);

            Rect sideBarPosition = new Rect(position);
            sideBarPosition.width += (sCurrentIndentAmount + 1) * kIndentWidth;

            DrawSidebar(sideBarPosition);

            Rect iconPosition = new Rect(position);
            iconPosition.width = kIconWidth;
            iconPosition.x = (position.width - kIconWidth) + kIconRightMargin;          

            DrawLockButton(gameObject, iconPosition);

            iconPosition.x -= (kIconWidth + kIconBufferWidth);

            DrawHideButton(gameObject, iconPosition);

            Rect layerPreviewPosition = new Rect(iconPosition);
            layerPreviewPosition.x -= (kIconWidth * 2) + (kIconBufferWidth * 2);
            layerPreviewPosition.x += sCurrentIndentAmount * kIndentWidth;
            layerPreviewPosition.width = kIconWidth * 2;

            GrendelHierarchyLayerPreview.DrawLayerPreview(gameObject, layerPreviewPosition);

            sPreviousIndentAmount = sCurrentIndentAmount;

            sPreviousItemPosition = new Rect(position);

            sCurrentObjectIndex++;
        }

        private static void DrawSidebar(Rect position)
        {
            GUI.color = (sCurrentObjectIndex & 1) != 1 ? GrendelEditorGUIUtility.CurrentSkinViewColor : sOddRowColor;

            position.x = position.width - kHierarchySidebarWidth;
            position.width = kHierarchySidebarWidth;

            GUI.DrawTexture(position, EditorGUIUtility.whiteTexture);
            GUI.color = Color.white;
        }

        //TODO: repaint the hierarchy window on new object / delete object in order to
        //properly recolour the rows

        private static void ColorNextRow(Rect position)
        {
            int nextRowIndex = sCurrentObjectIndex + 1;

            if (nextRowIndex < sTotalCurrentObjectCount)
            {
                if ((nextRowIndex & 1) == 1)
                {
                    position.y += EditorGUIUtility.singleLineHeight;
                    DrawBackground(position, sOddRowColor);
                }
            }
        }       

        private static void DrawBackground(Rect position, Color color)
        {
            GUI.color = color;

            Rect bgRect = new Rect(position);
            bgRect.x -= ((sCurrentIndentAmount + 1) * kIndentWidth) - 1f;
            bgRect.width += ((sCurrentIndentAmount + 1) * kIndentWidth) - 1f;

            GUI.DrawTexture(bgRect, EditorGUIUtility.whiteTexture);
            GUI.color = Color.white;
        }

        private static void DrawLockButton(GameObject gameObject, Rect iconPosition)
        {
            iconPosition.x += (kIndentWidth * sCurrentIndentAmount);

            bool locked = gameObject.IsLocked();

            if (locked)
            {
                sLockButtonStyle.normal.textColor = Color.yellow;
            }

            locked = GUI.Toggle(iconPosition, locked, string.Empty, sLockButtonStyle);

            sLockButtonStyle.normal.textColor = Color.gray;

            if (locked != gameObject.IsLocked())
            {
                gameObject.SetLock(locked, true);
            }
        }

        private static void DrawHideButton(GameObject gameObject, Rect iconPosition)
        {
            iconPosition.x += (kIndentWidth * sCurrentIndentAmount);

            //TODO: Change 'true' to an actual check for 'hidden' status
            GUI.contentColor = true ? Color.gray : Color.white;

            sHideButtonContent = true ? EditorGUIUtility.IconContent("ViewToolOrbit On") : EditorGUIUtility.IconContent("ViewToolOrbit");

            sHideButtonContent.tooltip = "Hide / Show Object";

            bool active = gameObject.activeSelf;

            bool something = GUI.Toggle(iconPosition, true, sHideButtonContent, sHideButtonStyle);
            
            GUI.contentColor = Color.white;
        }
    }
}
