using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    public class GrendelEditorIcons
    {
        public static Texture2D WarningSymbolSmall = GetIcon("console.warnicon.sml");
        public static Texture2D EyeOpen = GetIcon("animationvisibilitytoggleon");
        public static Texture2D EyeClosed = GetIcon("animationvisibilitytoggleoff");
        public static Texture2D FolderIcon = GetIcon("folder icon");
        public static Texture2D FolderEmptyIcon = GetIcon("folderempty icon");

        private static Texture2D GetIcon(string iconName)
        {
            return EditorGUIUtility.FindTexture(iconName);
        }
    }
}
