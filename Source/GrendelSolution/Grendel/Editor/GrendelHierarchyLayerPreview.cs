using System;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Grendel.Editor
{
    internal static class GrendelHierarchyLayerPreview
    {
        private static GUIStyle sLabelStyle;
        private static GUIStyle sButtonStyle;

        private const string kButtonText = "▼";
        private const string kCheckMarkText = "✔";
        private const float kLayerCheckMarkWidth = 16f;
        private const float kLayerEntryHeight = 16f;
        private static string[] sLayerNames = new string[0];

        internal static void DrawLayerPreview(GameObject obj, Rect iconPosition)
        {
            if (sLabelStyle == null)
            {
                sLabelStyle = new GUIStyle(EditorStyles.miniLabel);
                sLabelStyle.fontSize = 9;
                sLabelStyle.alignment = TextAnchor.MiddleLeft;

                sButtonStyle = new GUIStyle(EditorStyles.popup);
                sButtonStyle.fontSize = 9;
                sButtonStyle.alignment = TextAnchor.MiddleRight;
                sButtonStyle.margin = sLabelStyle.margin;
                sButtonStyle.padding = sLabelStyle.padding;
            }

            int layerPreviewControlID = GUIUtility.GetControlID(FocusType.Passive, iconPosition);
            GrendelLayerPreviewPopupState previewState = (GrendelLayerPreviewPopupState)GUIUtility.GetStateObject(typeof(GrendelLayerPreviewPopupState), layerPreviewControlID);
            previewState.IconPosition = iconPosition;
            previewState.Object = obj;
            string layerName = LayerMask.LayerToName(obj.layer);
            string shortName = layerName;

            if (shortName.Length > 3)
            {
                shortName = shortName.Remove(3);
            }

            GUIContent labelContent = new GUIContent(shortName, layerName);

            Color previousColor = GUI.color;
            GUI.color = Color.Lerp(GrendelLayerColours.GetLayerColor(obj.layer), GrendelEditorGUIUtility.CurrentSkinViewColor, 0.35f);

            previewState.IsExpanded = GUI.Toggle(iconPosition, previewState.IsExpanded, string.Empty, sButtonStyle);

            GUI.Label(iconPosition, labelContent, sLabelStyle);
            GUI.color = previousColor;

            if (previewState.IsExpanded && GrendelHierarchyView.CurrentLayerPopupState != previewState)
            {
                GrendelHierarchyView.CurrentLayerPopupState = previewState;             
            }
            else if (!previewState.IsExpanded && GrendelHierarchyView.CurrentLayerPopupState == previewState)
            {
                GrendelHierarchyView.CurrentLayerPopupState = null;
            }
        }

        public static void DrawPopup(GrendelLayerPreviewPopupState state)
        {
            Event currentEvent = Event.current;
            Rect iconPosition = state.IconPosition;
            GameObject obj = state.Object;
            int layer = obj.layer;

            sLayerNames = UnityEditorInternal.InternalEditorUtility.layers;
            float windowWidth = 128f;
            float windowHeight = kLayerEntryHeight * (sLayerNames.Length);
            Rect windowPos = new Rect(iconPosition);

            windowPos.width = windowWidth;
            windowPos.height = windowHeight;
            
            Vector2 windowCentre = new Vector2(windowPos.x, iconPosition.y - (EditorGUIUtility.singleLineHeight * 0.5f));

            windowCentre.y = Mathf.Clamp(windowPos.center.y, 0, Screen.height - windowHeight);

            windowPos.center = windowCentre;

            windowPos.x -= windowWidth * 0.5f;

            bool mouseInBox = windowPos.Contains(currentEvent.mousePosition);

            if (!mouseInBox && Event.current.type == EventType.MouseDown)
            {
                state.IsExpanded = false;
                currentEvent.Use();
                return;
            }

            GUILayout.BeginArea(windowPos, GUI.skin.box);

            Rect layerRect;

            int returnLayer = layer;
            bool mousePressed = false;
            for (int i = 0; i < InternalEditorUtility.layers.Length; i++ )
            {
                GUILayout.BeginHorizontal(GUILayout.Height(kLayerEntryHeight));

                GUILayout.Label(layer == LayerMask.NameToLayer(InternalEditorUtility.layers[i]) ? kCheckMarkText : " ", GUILayout.Width(kLayerCheckMarkWidth), GUILayout.Height(kLayerEntryHeight));

                Rect checkmarkRect;

                Rect lastRect = checkmarkRect = GUILayoutUtility.GetLastRect();

                GrendelGUI.ShadedGUILine(new Rect(lastRect.x + kLayerCheckMarkWidth, lastRect.y - 5, 1, EditorGUIUtility.singleLineHeight + 2), Color.white, Color.gray, Vector2.one);

                GUILayout.BeginHorizontal(GUILayout.Height(kLayerEntryHeight));

                layerRect = GUILayoutUtility.GetRect(windowWidth - kLayerCheckMarkWidth, kLayerEntryHeight);
                Rect longLayerRect = new Rect(layerRect);
                longLayerRect.width = windowWidth;
                longLayerRect.x -= kLayerCheckMarkWidth;

                GUI.color = GrendelLayerColours.GetLayerColor(LayerMask.NameToLayer(InternalEditorUtility.layers[i]));

                GUI.DrawTexture(layerRect, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill);
                GUI.Label(layerRect, InternalEditorUtility.layers[i]);
                
                if (longLayerRect.Contains(currentEvent.mousePosition))
                {
                    GUI.color = Color.cyan;
                    GUI.Box(layerRect, string.Empty, new GUIStyle("selectionRect"));

                    if (LayerMask.NameToLayer(InternalEditorUtility.layers[i]) != layer)
                    {
                        GUI.color = Color.gray;
                        GUI.Label(checkmarkRect, kCheckMarkText, EditorStyles.whiteLabel);
                    }                    

                    if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
                    {
                        mousePressed = true;
                        returnLayer = LayerMask.NameToLayer(InternalEditorUtility.layers[i]);
                    }
                }

                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();
                
                GUILayout.EndHorizontal();

                
            }

            GUI.color = Color.white;
            GUILayout.EndArea();

            state.Object.layer = returnLayer;

            if (mousePressed)
            {
                currentEvent.Use();
            }
        }  
    }    

    internal class GrendelLayerPreviewPopupState
    {
        public bool IsExpanded = false;
        public Rect IconPosition;
        public GameObject Object = null;
    }
}
