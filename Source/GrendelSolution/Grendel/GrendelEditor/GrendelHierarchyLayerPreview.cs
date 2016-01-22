using System;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Grendel.GrendelEditor
{
    internal static class GrendelHierarchyLayerPreview
    {
        private static GUIStyle sLabelStyle;
        private static GUIStyle sButtonStyle;
        private static GUIStyle sSelectionRectStyle;
        private static string[] sLayerNames = new string[0];
        internal static GrendelLayerPreviewPopupState CurrentPopupState = null;
        internal static LayerSelectPopupWindow sPopupWindow = null;

        private const string kButtonText = "";
        private const string kCheckMarkText = "✔";
        private const string kLayerTooltipPretext = "Layer: ";
        private const float kLayerCheckMarkWidth = 16f;
        private const float kLayerEntryHeight = 18f;
        private const float kHeightFudge = 1f;        

        internal static void DrawLayerPreview(GameObject obj, Rect iconPosition)
        {
            if (sLabelStyle == null)
            {
                sLabelStyle = new GUIStyle(EditorStyles.miniLabel);
                sLabelStyle.fontSize = 9;
                sLabelStyle.alignment = TextAnchor.MiddleLeft;

                sButtonStyle = new GUIStyle(GrendelStyles.DropDownStyle);
                sButtonStyle.fontSize = 9;
                sButtonStyle.alignment = TextAnchor.MiddleRight;
                sButtonStyle.margin = sLabelStyle.margin;
                sButtonStyle.padding = sLabelStyle.padding;               

                sButtonStyle.fixedHeight = iconPosition.height - 1f;
                sSelectionRectStyle = new GUIStyle("selectionRect");                
            }


            int layerPreviewControlID = GUIUtility.GetControlID(FocusType.Passive, iconPosition);
            GrendelLayerPreviewPopupState previewState = (GrendelLayerPreviewPopupState)GUIUtility.GetStateObject(typeof(GrendelLayerPreviewPopupState), layerPreviewControlID);
            previewState.IconPosition = iconPosition;
            previewState.Object = obj;

            if (CurrentPopupState != null && CurrentPopupState.GetHashCode() == previewState.GetHashCode())
            {
                previewState.IsExpanded = CurrentPopupState.IsExpanded;
            }

            string layerName = LayerMask.LayerToName(obj.layer);
            string shortName = layerName;

            if (shortName.Length > 3)
            {
                shortName = shortName.Remove(3);
            }

            GUIContent labelContent = new GUIContent(shortName, kLayerTooltipPretext + layerName);

            Color previousColor = GUI.color;
            GUI.color = Color.Lerp(GrendelLayerColours.GetLayerColor(obj.layer), GrendelEditorGUIUtility.CurrentSkinViewColor, 0.35f);

            previewState.IsExpanded = GUI.Toggle(iconPosition, previewState.IsExpanded, kButtonText, sButtonStyle);

            GUI.Label(iconPosition, labelContent, sLabelStyle);
            
            GUI.color = previousColor;

            if (previewState.IsExpanded)
            {
                if (CurrentPopupState == null)
                {
                    CurrentPopupState = previewState;
                    sLayerNames = UnityEditorInternal.InternalEditorUtility.layers;
                    CreatePopup(iconPosition);
                }
                else if (CurrentPopupState.GetHashCode() != previewState.GetHashCode())
                {
                    CurrentPopupState.IsExpanded = false;
                    CurrentPopupState = previewState;
                   
                    if (sPopupWindow != null)
                    {
                        sPopupWindow.Close();
                    }

                    sLayerNames = UnityEditorInternal.InternalEditorUtility.layers;
                    CreatePopup(iconPosition);
                }
                else if (CurrentPopupState.GetHashCode() == previewState.GetHashCode())
                {
                    if (!CurrentPopupState.IsExpanded || sPopupWindow == null)
                    {
                        CurrentPopupState.IsExpanded = true;

                        if (sPopupWindow != null)
                        {
                            sPopupWindow.Close();
                        }

                        sLayerNames = UnityEditorInternal.InternalEditorUtility.layers;
                        CreatePopup(iconPosition);
                    }
                }
            }
            else if (!previewState.IsExpanded)
            {
                if (CurrentPopupState == null)
                {

                }
                else if (CurrentPopupState.GetHashCode() == previewState.GetHashCode() && CurrentPopupState.IsExpanded)
                {
                    CurrentPopupState.IsExpanded = false;
                    CurrentPopupState = null;

                    if (sPopupWindow != null)
                    {
                        sPopupWindow.Close();
                    }
                }
            }
        }

        public static void CreatePopup(Rect iconPosition)
        {
            float windowWidth = 128f;
            float windowHeight = kLayerEntryHeight * (sLayerNames.Length);
            Rect windowPos = new Rect(iconPosition);
            
            windowPos.width = windowWidth;
            windowPos.height = windowHeight;

            Vector2 windowCentre = new Vector2(windowPos.x, iconPosition.center.y);

            windowCentre.y = Mathf.Clamp(windowPos.center.y, 0f, (Screen.height - (windowHeight * 0.5f)) - (EditorGUIUtility.singleLineHeight * 2));

            windowPos.center = GUIUtility.GUIToScreenPoint(windowCentre);
            windowPos.x -= windowWidth * 0.5f;

            sPopupWindow = EditorWindow.CreateInstance<LayerSelectPopupWindow>();
            sPopupWindow.position = windowPos;
            sPopupWindow.wantsMouseMove = true;
            sPopupWindow.ShowPopup();
            sPopupWindow.Focus();
        }

        public static void DrawPopup(GrendelLayerPreviewPopupState state)
        {
            if (state == null)
            {
                return;
            }

            Event currentEvent = Event.current;
            Rect iconPosition = state.IconPosition;
            int layer = state.Object.layer;

            float windowWidth = 128f;           

            Rect layerRect;
            Rect checkmarkRect;
            Rect lastRect;
            Rect longLayerRect; 

            int returnLayer = layer;

            GUILayout.BeginVertical();
          
            for (int i = 0; i < sLayerNames.Length; i++)
            {
                GUILayout.BeginHorizontal(GUILayout.Height(kLayerEntryHeight));

                GUILayout.Label(layer == LayerMask.NameToLayer(sLayerNames[i]) ? kCheckMarkText : " ", GUILayout.Width(kLayerCheckMarkWidth), GUILayout.Height(kLayerEntryHeight));

                lastRect = checkmarkRect = GUILayoutUtility.GetLastRect();

                GrendelGUI.ShadedGUILine(new Rect(lastRect.x + kLayerCheckMarkWidth, lastRect.y - 4, 1, EditorGUIUtility.singleLineHeight + 1), Color.white, Color.gray, Vector2.one);

                layerRect = GUILayoutUtility.GetRect(windowWidth - kLayerCheckMarkWidth, kLayerEntryHeight);
                longLayerRect = new Rect(layerRect);
                longLayerRect.width = windowWidth;
                longLayerRect.x -= kLayerCheckMarkWidth;

                GUI.color = GrendelLayerColours.GetLayerColor(LayerMask.NameToLayer(sLayerNames[i]));

                GUI.DrawTexture(layerRect, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill);
                GUI.Label(layerRect, sLayerNames[i]);

                if (longLayerRect.Contains(currentEvent.mousePosition))
                {
                    GUI.color = Color.cyan;
                    GUI.Box(layerRect, string.Empty, sSelectionRectStyle);

                    if (LayerMask.NameToLayer(sLayerNames[i]) != layer)
                    {
                        GUI.color = Color.gray;
                        GUI.Label(checkmarkRect, kCheckMarkText, EditorStyles.whiteLabel);
                    }                    

                    if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
                    {
                        returnLayer = LayerMask.NameToLayer(sLayerNames[i]);
                        currentEvent.Use();
                        state.IsExpanded = false;
                        sPopupWindow.Close();
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();                
            }

            GUILayout.EndVertical();

            GUI.color = Color.white;

            state.Object.layer = returnLayer; //TODO: Undo functionality
        }  
    }

    internal class LayerSelectPopupWindow : EditorWindow
    {
        private void OnEnable()
        {
            if (GrendelHierarchyLayerPreview.CurrentPopupState == null && this != null)
            {
                DestroyImmediate(this, true);
            }
        }

        internal void OnGUI()
        {
            GrendelHierarchyLayerPreview.DrawPopup(GrendelHierarchyLayerPreview.CurrentPopupState);
            Repaint();
        }

        private void OnLostFocus()
        {
            GrendelHierarchyLayerPreview.CurrentPopupState.IsExpanded = false;
            
            Close();
        }
    }

    internal class GrendelLayerPreviewPopupState
    {
        public bool IsExpanded = false;
        public Rect IconPosition;
        public GameObject Object = null;
    }
}
