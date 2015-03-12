using System;

using UnityEngine;
using UnityEditor;

namespace Grendel.Editor
{
    internal static class GrendelHierarchyPreview
    {
        private static GUIContent sGameObjectIcon = EditorGUIUtility.IconContent("GameObject Icon");
        private static GUIContent sPrefabIcon = EditorGUIUtility.IconContent("Prefab Icon");
        private static GUIContent sPrefabNormalIcon = EditorGUIUtility.IconContent("PrefabNormal Icon");
        private static GUIContent sPrefabModelIcon = EditorGUIUtility.IconContent("PrefabModel Icon");

        private static GUIStyle sCustomIconStyle = null;

        private const float kDividerWidth = 1f;
        private const float kBrokenPrefabOpacity = 0.50f;

        private static void SetupStyle()
        {
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

            GUIContent typeIcon = new GUIContent();
            GUIContent customIcon = new GUIContent();

            SerializedObject obj = new SerializedObject(gameObject);

            customIcon = EditorGUIUtility.ObjectContent(gameObject, gameObject.GetType());
            
            if (typeIcon.image == null)
            {
                PrefabType prefabType = PrefabUtility.GetPrefabType(gameObject);

                switch (prefabType)
                {
                    case PrefabType.None:

                        typeIcon = sGameObjectIcon;

                    break;

                    case PrefabType.Prefab:

                        typeIcon = sPrefabNormalIcon;

                    break;

                    case PrefabType.PrefabInstance:

                        typeIcon = sPrefabNormalIcon;

                    break;

                    case PrefabType.DisconnectedPrefabInstance:

                        GUI.color = Color.Lerp(Color.white, Color.clear, kBrokenPrefabOpacity);
                        typeIcon = sPrefabNormalIcon;

                    break;

                    case PrefabType.ModelPrefab:
                    case PrefabType.ModelPrefabInstance:
                    case PrefabType.DisconnectedModelPrefabInstance:

                        typeIcon = sPrefabModelIcon;

                    break;

                    case PrefabType.MissingPrefabInstance:

                        GUI.color = Color.red;
                        typeIcon = sPrefabIcon;

                    break;
                }                               
            }

            if (typeIcon.image != null && gameObject.transform.childCount == 0)
            {
                GUI.DrawTexture(iconPosition, typeIcon.image);
            }

            if (customIcon != null)
            {
                rowPosition.x -= ((GrendelHierarchyView.kIconWidth + GrendelHierarchyView.kIconBufferWidth) * 2) + GrendelHierarchyView.kIconRightMargin + kDividerWidth;
                GUI.Label(rowPosition, customIcon.image, sCustomIconStyle);

                rowPosition.x = rowPosition.x + (rowPosition.width);
                rowPosition.y -= 1;
                rowPosition.width = kDividerWidth;

                GrendelGUI.ShadedGUILine(rowPosition, Color.gray, Color.white, Vector2.one);
            }

            GUI.color = Color.white;
        }
    }
}
