using System;
using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    internal static class GrendelFolder
    {
        private static Gradient sBackgroundGradient = null;
        private static GUIStyle sLabelStyle = null;

        internal static Color[] sColorFolderOptions = new Color[]
        {
            Color.clear,
            Color.cyan,
            Color.green,
            Color.magenta,
            GrendelColor.Canary,
            GrendelColor.LightOrange,
            GrendelColor.Denim,
            GrendelColor.LightPink,
            GrendelColor.Maroon,
        };

        private static GradientColorKey[] sBGColorKey = new GradientColorKey[]
        {
            new GradientColorKey(Color.white, 0f),
            new GradientColorKey(Color.white, 1f),
        };

        private static GradientAlphaKey[] sBGAlphaKey = new GradientAlphaKey[]
        {
            new GradientAlphaKey(1f, 0f),
            new GradientAlphaKey(1f, 0.25f),
            new GradientAlphaKey(0.3f, 0.55f),
            new GradientAlphaKey(0f, 0.70f),
        };

        private static Texture2D sFolderBGTexture = null;

        private const int kTextureHeight = 1;
        private const int kTextureWidth = 128;

        public static Texture2D FolderBGTexture
        {
            get
            {
                if (sFolderBGTexture == null)
                {
                    SetupFolderTexture();
                }

                return sFolderBGTexture;
            }
        }

        private static void SetupFolderTexture()
        {
            sFolderBGTexture = new Texture2D(kTextureWidth, kTextureHeight, TextureFormat.RGBA32, false, true); 

            sBackgroundGradient = new Gradient();
            sBackgroundGradient.colorKeys = sBGColorKey;
            sBackgroundGradient.alphaKeys = sBGAlphaKey;

            Color[] pixels = sFolderBGTexture.GetPixels();

            for (int i = 0; i < kTextureWidth; i++)
            {
                float time = (i + 1) / (float)kTextureWidth;
                pixels[i] = sBackgroundGradient.Evaluate( time );
            }

            sFolderBGTexture.SetPixels(pixels);
            sFolderBGTexture.Apply();
        }

        public static void DrawFolder(Rect pos, GrendelFolderComponent folder, Color rowColor)
        {
            if (sLabelStyle == null)
            {
                sLabelStyle = new GUIStyle(EditorStyles.label);
                sLabelStyle.alignment = TextAnchor.MiddleLeft;
            }

            Rect position = new Rect(pos);
            position.width -= GrendelHierarchyView.kIconWidth;
            position.x += GrendelHierarchyView.kIconWidth;
            GrendelHierarchyView.DrawBackground(position, rowColor);

            position.width -= GrendelHierarchyView.kIconWidth;
            position.x += GrendelHierarchyView.kIconWidth;
            GrendelHierarchyView.DrawBackground(position, folder.FolderColor, GrendelFolder.FolderBGTexture);
            
            position.x -= GrendelHierarchyView.kIconWidth;
            position.x += GrendelHierarchyView.kIconBufferWidth * 2;
            position.width += GrendelHierarchyView.kIconWidth;
            GUI.Label(position, folder.transform.name, sLabelStyle);

            position.width = GrendelHierarchyView.kIconWidth;
            position.x -= GrendelHierarchyView.kIconWidth;
            position.x -= GrendelHierarchyView.kIconBufferWidth * 1.5f;

            GUI.DrawTexture(position, folder.transform.childCount > 0 ? GrendelEditorIcons.FolderIcon : GrendelEditorIcons.FolderEmptyIcon);
        }
    }
}
