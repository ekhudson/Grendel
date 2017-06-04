using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
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
        internal const float kDefaultSidebarWidth = kIconRightMargin + ((kIconWidth + kIconBufferWidth) * 2f);
        private const float kDividerWidth = 1f;

        private static int sCurrentParentCount = 0;
        private static int sCurrentChildCount = 0;
        private static int sCurrentIndentAmount = 0;
        private static int sPreviousIndentAmount = 0;
        private static int sCurrentObjectIndex = 0;
        private static int sTotalCurrentObjectCount = 0;

        private static Rect sPreviousItemPosition = new Rect();
        private static Rect sCurrentItemPosition = new Rect();

        private static List<Transform> sCurrentParents = new List<Transform>();
        private static Transform sCurrentTransform = null;
        private static GrendelObjectData sCurrentObjectData = null;
        private static List<GameObject> sCurrentSelectedObjects = new List<GameObject>();
        
        private static Color sSelectionColor = new Color(0.15f, 0.45f, 1f, 1.0f);

        internal static int CurrentParentCount { get { return sCurrentParents.Count; } }
        internal static List<Transform> CurrentParents { get { return sCurrentParents;  } }
        
        static GrendelHierarchyView()
        {            
            SetHierarchyEnabled(true); //TODO: Is this even necessary anymore?
        }

        internal static Color OddRowColor
        {
            get
            {
                if (GrendelPreferencesHierarchy.sOddRowColourEnabled.BoolValue)
                {
                    Color color = GrendelPreferencesHierarchy.sOddRowColor.ColorValue;
                    float alpha = color.a;
                    color.a = 1;
                    color = Color.Lerp(GrendelEditorGUIUtility.CurrentSkinViewColor, color, alpha);

                    return color;
                }
                else
                {
                    return GrendelEditorGUIUtility.CurrentSkinViewColor;
                }
            }
        }

        internal static void RefreshLayerVisibility()
        {
            Tools.visibleLayers &= ~(1 << 7);
        } 

        internal static bool IsGameObjectBeingRenamed(GameObject gameObject)
        {
            return (GUI.GetNameOfFocusedControl() == "RenameOverlayField") && (Selection.activeGameObject == gameObject);
        }

        internal static bool IsObjectSelected(GameObject gameObject)
        {
            return GrendelSelection.SelectedGameObjects.Contains(gameObject);
        }

        internal static bool IsCurrentHierachyFocused(Rect itemPosition)
        {
            Vector2 screenPosition = EditorGUIUtility.GUIToScreenPoint(itemPosition.position);

            if (EditorWindow.focusedWindow == null || itemPosition == null)
            {
                return false;
            }
            
            return EditorWindow.focusedWindow.position.Contains(screenPosition);
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

        private static float CalculateSideBarWidth()
        {
            float width = kDefaultSidebarWidth;

            if (GrendelPreferencesHierarchy.sLayerPreviewEnabled.BoolValue)
            {
                width += (kIconWidth * 2) + (kIconBufferWidth * 2);
            }

            if (GrendelPreferencesHierarchy.sComponentPreviewEnabled.BoolValue)
            {
                width += (kIconWidth) + (kIconBufferWidth);
            }

            return width;
        }

        private static Color CurrentRowColor
        {
            get
            {
                if (IsObjectSelected(sCurrentTransform.gameObject))
                {
                    if (IsCurrentHierachyFocused(sCurrentItemPosition))
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
                        return OddRowColor;
                    }
                    else
                    {
                        return GrendelEditorGUIUtility.CurrentSkinViewColor;
                    }
                }
            }
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect position)
        {
            sCurrentIndentAmount = (int)((position.x / kIndentWidth) - 1);

            GameObject gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

            if (gameObject == null)
            {
                return;
            }             

            sCurrentObjectData = GrendelHierarchy.EnsureObjectHasGrendelData(gameObject);

            if (sCurrentObjectData == null)
            {
                return;
            }

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
                RefreshLayerVisibility();
            }

            GrendelFolderComponent folderComponent = gameObject.GetComponent<GrendelFolderComponent>();

            if (folderComponent != null && !IsGameObjectBeingRenamed(gameObject))
            {
                GrendelFolder.DrawFolder(position, folderComponent, CurrentRowColor);
            }

            ColorNextRow(position);         

            //if (gameObject.transform.parent != null)
            //{
            //    GUI.Label(position, "------------------" + EditorWindow.GetWindowWithRect<EditorWindow>(sCurrentItemPosition).ToString());
            //} 

            if (GrendelPreferencesHierarchy.sTreeViewEnabled.BoolValue)
            {
                GrendelHierarchyTreeView.DrawTreeBranch(gameObject, position, sCurrentIndentAmount, sPreviousIndentAmount);
            }

            Rect previewPosition = new Rect(position);

            previewPosition.x -= kIconWidth;
            previewPosition.width = kIconWidth;

            GrendelHierarchyObjectPreview.DrawPreview(gameObject, previewPosition, position, folderComponent == null);

            Rect sideBarPosition = new Rect(position);
            sideBarPosition.width += (sCurrentIndentAmount + 1) * kIndentWidth;

            DrawSidebar(sideBarPosition);

            Rect iconPosition = new Rect(position);
            iconPosition.width = kIconWidth;
            iconPosition.x = (position.width - kIconWidth) + kIconRightMargin;
            iconPosition.x += (kIndentWidth * sCurrentIndentAmount);

            GrendelLockButton.DrawLockButton(sCurrentObjectData, iconPosition);

            iconPosition.x -= (kIconWidth + kIconBufferWidth);

            GrendelHideButton.DrawHideButton(sCurrentObjectData, iconPosition);

            Rect layerPreviewPosition = new Rect(iconPosition);

            if (GrendelPreferencesHierarchy.sLayerPreviewEnabled.BoolValue)
            {                
                layerPreviewPosition.x -= (kIconWidth * 2) + (kIconBufferWidth * 2);
                layerPreviewPosition.width = kIconWidth * 2;

                GrendelHierarchyLayerPreview.DrawLayerPreview(sCurrentObjectData, layerPreviewPosition);
            }

            if (GrendelPreferencesHierarchy.sComponentPreviewEnabled.BoolValue)
            {
                iconPosition = new Rect(layerPreviewPosition);
                iconPosition.x -= (kIconWidth) + (kIconBufferWidth);
                iconPosition.width = kIconWidth;

                GrendelHierarchyObjectPreview.DrawTypeIcon(iconPosition, gameObject, folderComponent);
            }

            sPreviousIndentAmount = sCurrentIndentAmount;

            sPreviousItemPosition = new Rect(position);

            sCurrentObjectIndex++;
        }        

        private static void DrawSidebar(Rect position)
        {
            GUI.color = CurrentRowColor;

            float sideBarWidth = CalculateSideBarWidth();

            position.x = position.width - sideBarWidth;
            position.width = sideBarWidth;

            GUI.DrawTexture(position, EditorGUIUtility.whiteTexture);
            GUI.color = Color.white;

            position.y -= 1;
            position.width = kDividerWidth;

            GrendelGUI.ShadedGUILine(position, Color.gray, Color.white, Vector2.one);
        }

        //TODO: repaint the hierarchy window on new object / delete object in order to
        //properly recolour the rows

        private static void ColorNextRow(Rect position)
        {
            ColorNextRow(position, Color.white);
        }

        private static void ColorNextRow(Rect position, Color multiplyColor)
        {
            int nextRowIndex = sCurrentObjectIndex + 1;

            if (nextRowIndex < sTotalCurrentObjectCount)
            {
                position.y += EditorGUIUtility.singleLineHeight;

                if ((nextRowIndex & 1) == 1)
                {
                    DrawBackground(position, OddRowColor * multiplyColor);
                }
                else
                {
                    DrawBackground(position, GrendelEditorGUIUtility.CurrentSkinViewColor * multiplyColor);
                }
            }
        }

        internal static void DrawBackground(Rect position, Color color, Texture2D texture, bool scaleToRow)
        {
            GUI.color = color;

            Rect bgRect = new Rect(position);

            if (scaleToRow)
            {
                bgRect.x -= ((sCurrentIndentAmount + 1) * kIndentWidth) - 1f;
                bgRect.width += ((sCurrentIndentAmount + 1) * kIndentWidth) - 1f;
            }

            GUI.DrawTexture(bgRect, texture, ScaleMode.StretchToFill);
            GUI.color = Color.white;
        }

        internal static void DrawBackground(Rect position, Color color)
        {
            DrawBackground(position, color, EditorGUIUtility.whiteTexture, true);
        }
    }
}
