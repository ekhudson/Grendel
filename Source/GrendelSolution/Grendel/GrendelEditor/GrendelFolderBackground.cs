using System;
using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    internal static class GrendelFolderBackground
    {
        private static Gradient sBackgroundGradient = null;
        private static GradientColorKey[] sBGColorKey = new GradientColorKey[2];
        private static GradientAlphaKey[] sBGAlphaKey = new GradientAlphaKey[2];
        private const float kAlphaMidTime = 0.75f;

        private static void SetupGradient()
        {
            sBackgroundGradient = new Gradient();

        }
    }
}
