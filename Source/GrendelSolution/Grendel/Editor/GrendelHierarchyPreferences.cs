using System;

using UnityEngine;
using UnityEditor;

namespace Grendel.Editor
{
    internal class GrendelHierarchyPreferences
    {

        /// <summary>
        /// Grendel Hierarchy Enabled
        /// </summary>
        private const string kHierarchyEnabledKey = "GrendelHierarchy_HierarchyEnabled";
        private const bool kHierarchyEnabledDefault = true;

        internal static bool GrendelHierarchyEnabled
        {
            get
            {
                return EditorPrefs.GetBool(kHierarchyEnabledKey, kHierarchyEnabledDefault);                
            }
            set
            {
                EditorPrefs.SetBool(kHierarchyEnabledKey, value);
            }
        }

        /// <summary>
        /// Odd Row Colors
        /// </summary>
        private const string kOddRowColorRKey = "GrendelHierarchy_OddRowColorR";
        private const string kOddRowColorGKey = "GrendelHierarchy_OddRowColorG";
        private const string kOddRowColorBKey = "GrendelHierarchy_OddRowColorB";
        private const string kOddRowColorAKey = "GrendelHierarchy_OddRowColorA";
        private const Color kOddRowColorDefault = Color.Lerp(Color.magenta, GrendelEditorGUIUtility.CurrentSkinViewColor, 0.95f);

        internal static Color OddRowColor
        {
            get
            {
                float r = EditorPrefs.GetFloat(kOddRowColorRKey, kOddRowColorDefault.r);
                float g = EditorPrefs.GetFloat(kOddRowColorGKey, kOddRowColorDefault.g);
                float b = EditorPrefs.GetFloat(kOddRowColorBKey, kOddRowColorDefault.b);
                float a = EditorPrefs.GetFloat(kOddRowColorAKey, kOddRowColorDefault.a);

                return new Color(r, g, b, a);
            }
            set
            {
                EditorPrefs.GetFloat(kOddRowColorRKey, kOddRowColorDefault.r);
                EditorPrefs.GetFloat(kOddRowColorGKey, kOddRowColorDefault.g);
                EditorPrefs.GetFloat(kOddRowColorBKey, kOddRowColorDefault.b);
                float a = EditorPrefs.GetFloat(kOddRowColorAKey, kOddRowColorDefault.a);
            }
        }

        [PreferenceItem("Grendel Hierarchy")]
        public static void PreferencesGUI()
        {
            bool hierarchyEnabled = GrendelHierarchyEnabled;

            hierarchyEnabled = GUILayout.Toggle(hierarchyEnabled, "Grendel Hierarchy Enabled");

            if (hierarchyEnabled != GrendelHierarchyEnabled)
            {
                GrendelHierarchyEnabled = hierarchyEnabled;
                GrendelHierarchyView.SetHierarchyEnabled(GrendelHierarchyEnabled);
                EditorApplication.RepaintHierarchyWindow();
            }
        }
    }
}
