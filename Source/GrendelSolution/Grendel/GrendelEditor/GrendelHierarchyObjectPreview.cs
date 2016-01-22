using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Grendel.GrendelEditor
{
    internal static class GrendelHierarchyObjectPreview
    {
        private static GUIContent sGameObjectIcon = EditorGUIUtility.IconContent("GameObject Icon");
        private static GUIContent sPrefabIcon = EditorGUIUtility.IconContent("Prefab Icon");
        private static GUIContent sPrefabNormalIcon = EditorGUIUtility.IconContent("PrefabNormal Icon");
        private static GUIContent sPrefabModelIcon = EditorGUIUtility.IconContent("PrefabModel Icon");

        private static GUIStyle sCustomIconStyle = null;
        private static Color sDisabledIconColor = Color.gray;

        private const float kDividerWidth = 1f;
        private const float kBrokenPrefabOpacity = 0.50f;
        private const float kDisabledIconOpacity = 0.50f;

        private static void SetupStyle()
        {
            sDisabledIconColor.a = kDisabledIconOpacity;

            sCustomIconStyle = new GUIStyle(GUI.skin.label);
            sCustomIconStyle.alignment = TextAnchor.MiddleRight;
        }

        //TODO: Look at the first component? Or at least look for some key components
        //like camera, rigidbody, etc.
        //Let the user pick from components on the prefab?
        internal static void DrawPreview(GameObject gameObject, Rect iconPosition, Rect rowPosition)
        {
            if (sCustomIconStyle == null)
            {
                SetupStyle();
            }

            iconPosition.x += 2f;
            iconPosition.y -= 1f;

            GUIContent prefabIcon = new GUIContent();
            GUIContent typeIcon = new GUIContent();
            GUIContent customIcon = new GUIContent();

            typeIcon = new GUIContent(EditorGUIUtility.ObjectContent(null, TryGetFirstNonTransformComponentType(gameObject.GetComponents<Component>())));

            SerializedObject obj = new SerializedObject(gameObject);

            customIcon = new GUIContent(EditorGUIUtility.ObjectContent(gameObject, gameObject.GetType()));
            
            //TODO: This isn't compatible with the main preview anymore, figure out where to put this
            if (prefabIcon.image == null)
            {
                PrefabType prefabType = PrefabUtility.GetPrefabType(gameObject);

                switch (prefabType)
                {
                    case PrefabType.None:

                        prefabIcon = sGameObjectIcon;

                    break;

                    case PrefabType.Prefab:

                        prefabIcon = sPrefabNormalIcon;

                    break;

                    case PrefabType.PrefabInstance:

                        prefabIcon = sPrefabNormalIcon;

                    break;

                    case PrefabType.DisconnectedPrefabInstance:

                        GUI.color = Color.Lerp(Color.white, Color.clear, kBrokenPrefabOpacity);
                        prefabIcon = sPrefabNormalIcon;

                    break;

                    case PrefabType.ModelPrefab:
                    case PrefabType.ModelPrefabInstance:
                    case PrefabType.DisconnectedModelPrefabInstance:

                        prefabIcon = sPrefabModelIcon;

                    break;

                    case PrefabType.MissingPrefabInstance:

                        GUI.color = Color.red;
                        prefabIcon = sPrefabIcon;

                    break;
                }                               
            }

            Color previousGUIColor = GUI.color;

            if (!gameObject.activeInHierarchy)
            {
                GUI.color = sDisabledIconColor;
            }

            if (prefabIcon.image != null && gameObject.transform.childCount == 0)
            {
                GUI.DrawTexture(iconPosition, prefabIcon.image);
            }

            DrawTypeIcon(rowPosition, customIcon, typeIcon);
                
            GUI.color = previousGUIColor;            
        }

        private static void DrawTypeIcon(Rect position, GUIContent customIcon, GUIContent typeIcon)
        {
                bool hasCustomIcon = false;

                hasCustomIcon = (customIcon.image != sGameObjectIcon.image) &&
                                (customIcon.image != sPrefabIcon.image) &&
                                (customIcon.image != sPrefabModelIcon.image) &&
                                (customIcon.image != sPrefabNormalIcon.image);                

                position.x -= ((GrendelHierarchyView.kIconWidth + GrendelHierarchyView.kIconBufferWidth) * 4) + GrendelHierarchyView.kIconRightMargin + kDividerWidth;
                GUI.Label(position, hasCustomIcon ? customIcon.image : typeIcon.image, sCustomIconStyle);

                position.x = position.x + (position.width);
                position.y -= 1;
                position.width = kDividerWidth;

                GrendelGUI.ShadedGUILine(position, Color.gray, Color.white, Vector2.one);
        }

        private static Type TryGetFirstNonTransformComponentType(Component[] components)
        {
            if (components.Length <= 1 || components == null)
            {
                return typeof(Transform);
            }
            else if (components[1] != null)
            {
                return components[1].GetType();
            }
            else
            {
                return typeof(Component); 
            }
        }
    }
}
