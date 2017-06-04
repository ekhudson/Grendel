using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    
    public static class GrendelPreferencesHierarchy
    {
        public static GrendelPreferencesItem sLayerPreviewEnabled = new GrendelPreferencesItem("GRN_LayerPreviewEnabled", true, "Enable Layer Preview", "Preview the gameobject's layer in the hierachy.");
        public static GrendelPreferencesItem sComponentPreviewEnabled = new GrendelPreferencesItem("GRN_ComponentPreviewEnabled", false, "Enable Component Preview", "Preview the first component on the gameobject in the hierarchy.");
        public static GrendelPreferencesItem sOddRowColourEnabled = new GrendelPreferencesItem("GRN_OddRowColourEnabled", false, "Enable Odd Row Color", "Colorize odd rows for greater clarity.");
        public static GrendelPreferencesItem sTreeViewEnabled = new GrendelPreferencesItem("GRN_TreeViewEnabled", true, "Enable Branch View", "Visualize child/parent branches in the hierachy.");
        public static GrendelPreferencesItem sOddRowColor = new GrendelPreferencesItem("GRN_OddRowColor", Color.magenta, "Odd Row Color", "Color to use in colorizing odd rows.");

        [PreferenceItem("Grendel\nHierarchy")]
        public static void PreferencesGUI()
        {
            sTreeViewEnabled.BoolValue = EditorGUILayout.ToggleLeft(sTreeViewEnabled.PrefGUIContent, sTreeViewEnabled.BoolValue);
            sComponentPreviewEnabled.BoolValue = EditorGUILayout.ToggleLeft(sComponentPreviewEnabled.PrefGUIContent, sComponentPreviewEnabled.BoolValue);
            sLayerPreviewEnabled.BoolValue = EditorGUILayout.ToggleLeft(sLayerPreviewEnabled.PrefGUIContent, sLayerPreviewEnabled.BoolValue);

            sOddRowColourEnabled.BoolValue = EditorGUILayout.ToggleLeft(sOddRowColourEnabled.PrefGUIContent, sOddRowColourEnabled.BoolValue);

            bool previousGUIState = GUI.enabled;

            GUI.enabled = sOddRowColourEnabled.BoolValue;

            sOddRowColor.ColorValue = EditorGUILayout.ColorField(sOddRowColor.PrefGUIContent, sOddRowColor.ColorValue);

            GUI.enabled = previousGUIState;

            EditorApplication.RepaintHierarchyWindow();
        }        
    }

    public class GrendelPreferencesItem
    {
        private string mPrefKey = string.Empty;
        private GUIContent mPrefGUIContent = new GUIContent(string.Empty, string.Empty);
        private bool mDefaultBoolValue;
        private int mDefaultIntValue;
        private float mDefaultFloatValue;
        private string mDefaultStringValue;
        private Color mDefaultColorValue;

        public GrendelPreferencesItem(string key, int defaultValue, string label, string tooltip)
        {
            InitPreferencesItem(key, defaultValue, label, tooltip);
        }

        public GrendelPreferencesItem(string key, float defaultValue, string label, string tooltip)
        {
            InitPreferencesItem(key, defaultValue, label, tooltip);
        }

        public GrendelPreferencesItem(string key, bool defaultValue, string label, string tooltip)
        {
            InitPreferencesItem(key, defaultValue, label, tooltip);
        }

        public GrendelPreferencesItem(string key, string defaultValue, string label, string tooltip)
        {
            InitPreferencesItem(key, defaultValue, label, tooltip);
        }

        public GrendelPreferencesItem(string key, Color defaultValue, string label, string tooltip)
        {
            InitPreferencesItem(key, defaultValue, label, tooltip);
        }

        private void InitPreferencesItem(string key, int defaultValue, string label, string tooltip)
        {
            mPrefKey = key;
            mDefaultIntValue = defaultValue;
            mPrefGUIContent.text = label;
            mPrefGUIContent.tooltip = tooltip;
        }

        private void InitPreferencesItem(string key, bool defaultValue, string label, string tooltip)
        {
            mPrefKey = key;
            mDefaultBoolValue = defaultValue;
            mPrefGUIContent.text = label;
            mPrefGUIContent.tooltip = tooltip;
        }

        private void InitPreferencesItem(string key, float defaultValue, string label, string tooltip)
        {
            mPrefKey = key;
            mDefaultFloatValue = defaultValue;
            mPrefGUIContent.text = label;
            mPrefGUIContent.tooltip = tooltip;
        }

        private void InitPreferencesItem(string key, string defaultValue, string label, string tooltip)
        {
            mPrefKey = key;
            mDefaultStringValue = defaultValue;
            mPrefGUIContent.text = label;
            mPrefGUIContent.tooltip = tooltip;
        }

        private void InitPreferencesItem(string key, Color defaultValue, string label, string tooltip)
        {
            mPrefKey = key;
            mDefaultColorValue = defaultValue;
            mPrefGUIContent.text = label;
            mPrefGUIContent.tooltip = tooltip;
        }

        public GUIContent PrefGUIContent { get { return mPrefGUIContent; } }

        public int IntValue
        {
            get
            {
                if (string.IsNullOrEmpty(mPrefKey))
                {
                    return -1;
                }
                else
                {
                    return EditorPrefs.GetInt(mPrefKey, int.Parse(mDefaultIntValue.ToString()));
                }
            }
            set
            {
                if (string.IsNullOrEmpty(mPrefKey))
                {
                    return;
                }
                else
                {
                    EditorPrefs.SetInt(mPrefKey, value);
                }
            }
        }

        public float FloatValue
        {
            get
            {
                if (string.IsNullOrEmpty(mPrefKey))
                {
                    return -1f;
                }
                else
                {
                    return EditorPrefs.GetFloat(mPrefKey, float.Parse(mDefaultFloatValue.ToString()));
                }
            }
            set
            {
                if (string.IsNullOrEmpty(mPrefKey))
                {
                    return;
                }
                else
                {
                    EditorPrefs.SetFloat(mPrefKey, value);
                }
            }
        }

        public bool BoolValue
        {
            get
            {
                if (string.IsNullOrEmpty(mPrefKey))
                {
                    return false;
                }
                else
                {
                    return EditorPrefs.GetBool(mPrefKey, bool.Parse(mDefaultBoolValue.ToString()));
                }
            }
            set
            {
                if (string.IsNullOrEmpty(mPrefKey))
                {
                    return;
                }
                else
                {
                    EditorPrefs.SetBool(mPrefKey, value);
                }
            }
        }

        public string StringValue
        {
            get
            {
                if (string.IsNullOrEmpty(mPrefKey))
                {
                    return string.Empty;
                }
                else
                {
                    return EditorPrefs.GetString(mPrefKey, mDefaultStringValue.ToString());
                }
            }
            set
            {
                if (string.IsNullOrEmpty(mPrefKey))
                {
                    return;
                }
                else
                {
                    EditorPrefs.SetString(mPrefKey, value);
                }
            }
        }

        public Color ColorValue
        {
            get
            {
                if (string.IsNullOrEmpty(mPrefKey))
                {
                    return Color.white;
                }
                else
                {
                    string colorString = EditorPrefs.GetString(mPrefKey, mDefaultColorValue.ToString());
                    return ConvertStringToColor(colorString);
                }
            }
            set
            {
                if (string.IsNullOrEmpty(mPrefKey))
                {
                    return;
                }
                else
                {
                    string colorString = ConvertColorToString(value);
                    EditorPrefs.SetString(mPrefKey, colorString);
                }
            }
        }

        internal static Color ConvertStringToColor(string colorString)
        {
            Color colorToReturn = Color.white;
            string[] components = colorString.Split(';');

            if (components.Length == 5)
            {
                float red = 1f;
                float green = 1f;
                float blue = 1f;
                float alpha = 1f;
                float.TryParse(components[1], out red);
                float.TryParse(components[2], out green);
                float.TryParse(components[3], out blue);
                float.TryParse(components[4], out alpha);
                colorToReturn = new Color(red, green, blue, alpha);
            }

            return colorToReturn;
        }

        internal static string ConvertColorToString(Color color)
        {
            string colorString = string.Format("X;{0};{1};{2};{3}", color.r, color.g, color.b, color.a);
            return colorString;
        }
    }
}
