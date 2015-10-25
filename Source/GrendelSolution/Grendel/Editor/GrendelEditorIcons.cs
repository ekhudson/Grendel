using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    public class GrendelEditorIcons
    {
        public static Texture2D WarningSymbolSmall = GetIcon("console.warnicon.sml");
        public static Texture2D EyeOpen = GetIcon("animationvisibilitytoggleon");
        public static Texture2D EyeClosed = GetIcon("animationvisibilitytoggleoff");

        private static Texture2D GetIcon(string iconName)
        {
            return EditorGUIUtility.FindTexture(iconName);
        }
    }
}
