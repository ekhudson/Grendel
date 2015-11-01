using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    public class GrendelStyles
    {
        public static GUIStyle ControlHighlightStyle = GetStyle("ControlHighlight");
        public static GUIStyle GizmosButtonStyle = GetStyle("GV Gizmo DropDown");
        public static GUIStyle DropDownStyle = GetStyle("DropDown");
        public static GUIStyle SelectionRect = GetStyle("SelectionRect");

        private static GUIStyle GetStyle(string name)
        {
            return new GUIStyle(name);
        }
    }
}
