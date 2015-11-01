using System;

using UnityEngine;

namespace Grendel.GrendelEditor
{
    internal static class GrendelColor
    {
        internal static Color Orange = new Color(1.0f, 0.65f, 0.0f);
        internal static Color DarkOrange = new Color(1.0f, 0.35f, 0.0f);
        internal static Color LightOrange = new Color(1.0f, 0.75f, 0.0f);
        internal static Color Purple = new Color(0.6f, 0.0f, 1.0f);
        internal static Color DarkPurple = new Color(0.4f, 0.0f, 0.65f);
        internal static Color LightPurple = new Color(0.75f, 0.5f, 0.9f);
        internal static Color Pink = new Color(0.9f, 0.5f, 0.95f);
        internal static Color DarkPink = new Color(0.75f, 0.45f, 0.8f);
        internal static Color LightPink = new Color(0.95f, 0.75f, 1f);
        internal static Color Olive = new Color(0.4f, 0.5f, 0.36f);
        internal static Color OliveDark = new Color(0.26f, 0.4f, 0.22f);
        internal static Color OliveLight = new Color(0.55f, 0.67f, 0.5f);

        internal static Color Mauve = new Color(0.7f, 0.63f, 0.9f);
        internal static Color Steel = new Color(0.5f, 0.55f, 0.7f);
        internal static Color Teal = new Color(0.5f, 0.67f, 0.65f);
        internal static Color Maroon = new Color(0.6f, 0.2f, 0.12f);
        internal static Color Rose = new Color(0.87f, 0.65f, 0.61f);
        internal static Color Denim = new Color(0.36f, 0.42f, 0.61f);
        internal static Color Canary = new Color(0.91f, 0.74f, 0.0f);
        internal static Color Gold = new Color(0.7f, 0.67f, 0.11f);
        internal static Color Copper = new Color(0.8f, 0.52f, 0.39f);
        internal static Color Sand = new Color(0.7f, 0.612f, 0.5f);
        internal static Color Peach = new Color(0.934f, 0.618f, 0.5f);
        internal static Color Blueberry = new Color(0.0f, 0.025f, 0.52f);
        internal static Color Aquamarine = new Color(0.0f, 0.55f, 0.415f);

        internal static Color GrayLight = new Color(0.73f, 0.73f, 0.73f);

        private static Color sReusableColor = Color.white;

        internal static Color ModifyAlpha(Color color, float alpha)
        {
            sReusableColor = color;
            color.a = alpha;
            return color;
        }



    }
}
