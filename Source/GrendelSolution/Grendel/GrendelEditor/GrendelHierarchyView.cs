using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using Grendel.Extensions;

namespace Grendel.GrendelEditor
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
        private static GUIStyle sLockedLabelStyle = null;
        
        private static GUIStyle sHideButtonStyle = null;
        private static GUIContent sHideButtonContent = null;

        private static int sCurrentParentCount = 0;
        private static int sCurrentChildCount = 0;
        private static int sCurrentIndentAmount = 0;
        private static int sPreviousIndentAmount = 0;
        private static int sCurrentObjectIndex = 0;
        private static int sTotalCurrentObjectCount = 0;
        private static Color sEvenRowColor = GrendelEditorGUIUtility.CurrentSkinViewColor;

        private static Rect sPreviousItemPosition = new Rect();
        private static Rect sCurrentItemPosition = new Rect();

        private static List<Transform> sCurrentParents = new List<Transform>();
        private static Transform sCurrentTransform = null;
        private static GrendelObjectData sCurrentObjectData = null;
        private static List<GameObject> sCurrentSelectedObjects = new List<GameObject>();
        
        private static Color sOddRowColor = Color.Lerp(Color.magenta, GrendelEditorGUIUtility.CurrentSkinViewColor , 0.95f);
        private static Color sLockedLabelColor = Color.Lerp(Color.yellow, GrendelEditorGUIUtility.CurrentSkinViewColor, 0.45f);
        private static Color sSelectionColor = new Color(0.15f, 0.45f, 1f, 1.0f);
        private static GUIContent sVisibleContent = null;
        private static GUIContent sInvisibleContent = null;

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

        private static Color CurrentRowColor
        {
            get
            {
                if (GrendelSelection.SelectedGameObjects.Contains(sCurrentTransform.gameObject))
                {
                    //TODO: Figure out why this doesn't return gray when the hierarchy window is out of focus. Does unity think ALL hierarchy windows are always in focus?
                    if (EditorWindow.GetWindowWithRect<EditorWindow>(sCurrentItemPosition).Equals(EditorWindow.focusedWindow))
                    {
                        return sSelectionColor;
                    }
                    else
                    {
                        return Color.gray;
                    }                    
                }
                else
                {
                    if ((sCurrentObjectIndex & 1) == 1)
                    {
                        return sOddRowColor;
                    }
                    else
                    {
                        return sEvenRowColor;
                    }
                }
            }
        }

        private static void SetupStyles()
        {
            sItemLabelStyle = new GUIStyle(EditorStyles.label);

            sSelectionRectStyle = GrendelStyles.SelectionRect;

            sLockButtonStyle = sLockButtonStyleName;

            sHideButtonStyle = new GUIStyle(GUIStyle.none);
            sHideButtonStyle.fontSize = (int)kIconWidth;
            sHideButtonStyle.alignment = TextAnchor.MiddleCenter;
            sVisibleContent = new GUIContent(GrendelEditorIcons.EyeOpen);
            sInvisibleContent = new GUIContent(GrendelEditorIcons.EyeClosed);

            sLockedLabelStyle = new GUIStyle(GUI.skin.label);
            sLockedLabelStyle.fontStyle = FontStyle.Bold;
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

            //TODO: Figure out how to see if the name of the gameobject is being edited. It looks really bad right now if it is.

            sCurrentObjectData = GrendelHierarchy.EnsureObjectHasGrendelData(gameObject);

            sCurrentItemPosition = new Rect(position);
            sCurrentTransform = gameObject.transform;
            sCurrentParents = new List<Transform>(gameObject.GetComponentsInParent<Transform>(true));

            if (sCurrentParents.Count > 0 && sCurrentParents.Contains(gameObject.transform))
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
            //    GUI.Label(position, "------------------" + EditorWindow.GetWindowWithRect<EditorWindow>(sCurrentItemPosition).ToString());
            //} 

            GrendelHierarchyTreeView.DrawTreeBranch(gameObject, position, sCurrentIndentAmount, sPreviousIndentAmount);

            Rect previewPosition = new Rect(position);

            previewPosition.x -= kIconWidth;
            previewPosition.width = kIconWidth;

            GrendelHierarchyObjectPreview.DrawPreview(gameObject, previewPosition, position);

            Rect sideBarPosition = new Rect(position);
            sideBarPosition.width += (sCurrentIndentAmount + 1) * kIndentWidth;

            DrawSidebar(sideBarPosition);

            Rect iconPosition = new Rect(position);
            iconPosition.width = kIconWidth;
            iconPosition.x = (position.width - kIconWidth) + kIconRightMargin;

            DrawLockButton(gameObject, iconPosition);            

            iconPosition.x -= (kIconWidth + kIconBufferWidth);

            DrawHideButton(sCurrentObjectData, iconPosition);

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
            if (sCurrentObjectData == null)
            {
                return;
            }

            iconPosition.x += (kIndentWidth * sCurrentIndentAmount);

            bool locked = sCurrentObjectData.IsLocked;

            if (iconPosition.Contains(Event.current.mousePosition))
            {
                GUI.color = GrendelColor.ModifyAlpha(sLockedLabelColor, 0.5f);
                GUI.DrawTexture(sCurrentItemPosition, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill);
                HandleUtility.Repaint();
                GUI.color = Color.white;
            }

            if (!locked && Event.current.type == EventType.Repaint)
            {
                GUI.enabled = false;
                sLockButtonStyle.Draw(iconPosition, false, false, false, false);
                GUI.enabled = true;
            }

            locked = GUI.Toggle(iconPosition, locked, string.Empty, locked ? sLockButtonStyle : GUIStyle.none);         

            sLockButtonStyle.normal.textColor = Color.gray;

            if (locked != sCurrentObjectData.IsLocked)
            {
                sCurrentObjectData.SetLock(locked);
            }

            GUI.color = Color.white;
        }

        private static Color sSubHiddenColor = Color.gray;
        private const float kSubHiddenAlpha = 0.50f;

        private static void DrawHideButton(GrendelObjectData obj, Rect iconPosition)
        {
            iconPosition.x += (kIndentWidth * sCurrentIndentAmount);

            bool hidden = obj.IsHidden;

            sHideButtonContent = new GUIContent(hidden ? sInvisibleContent : sVisibleContent);

            sHideButtonContent.tooltip = "Hide / Show Object";

            Color previousGUIColor = GUI.color;

            if (!obj.gameObject.activeInHierarchy)
            {
                sSubHiddenColor.a = kSubHiddenAlpha;
                GUI.color = sSubHiddenColor;
            }

            hidden = GUI.Toggle(iconPosition, hidden, sHideButtonContent, sHideButtonStyle);

            if (hidden != obj.IsHidden)
            {
                obj.SetHidden(hidden);
            }

            if (!obj.gameObject.activeInHierarchy)
            {
                GUI.color = previousGUIColor;
            }
        }
    }
}
