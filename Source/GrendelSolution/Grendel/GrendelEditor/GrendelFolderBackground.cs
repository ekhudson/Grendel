using System;
using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    internal static class GrendelFolderBackground
    {
        private static Gradient sBackgroundGradient = null;

        private static GradientColorKey[] sBGColorKey = new GradientColorKey[]
        {
            new GradientColorKey(Color.white, 0f),
            new GradientColorKey(Color.white, 1f),
        };

        private static GradientAlphaKey[] sBGAlphaKey = new GradientAlphaKey[]
        {
            new GradientAlphaKey(0.8f, 0f),
            new GradientAlphaKey(0.8f, 0.25f),
            new GradientAlphaKey(0.4f, 0.55f),
            new GradientAlphaKey(0f, 0.80f),
        };

        private static Texture2D sFolderBGTexture = null;

        private const int kTextureHeight = 1;
        private const int kTextureWidth = 64;

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
                Debug.Log(time.ToString() + " : " + sBackgroundGradient.Evaluate(time).ToString());
            }

            sFolderBGTexture.SetPixels(pixels);
            sFolderBGTexture.Apply();
        }
    }
}
