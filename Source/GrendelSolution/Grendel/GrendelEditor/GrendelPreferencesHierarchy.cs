using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    
    public static class GrendelPreferencesHierarchy
    {
        public static GrendelPreferencesItem<bool> sLayerPreviewEnabled = new GrendelPreferencesItem<bool>("GRN_LayerPreviewEnabled", true, "Layer Preview", "?");
        public static GrendelPreferencesItem<bool> sComponentPreviewEnabled = new GrendelPreferencesItem<bool>("GRN_ComponentPreviewEnabled", false, "Component Preview", "?");
        public static GrendelPreferencesItem<bool> sBranchViewEnabled = new GrendelPreferencesItem<bool>("GRN_BranchViewEnabled", false, "Branch Preview", "?");
        public static GrendelPreferencesItem<bool> sOddRowColourEnabled = new GrendelPreferencesItem<bool>("GRN_OddRowColourEnabled", false, "Colour Odd Rows", "?");

        [PreferenceItem("Grendel\nHierarchy")]
        public static void PreferencesGUI()
        {
            sOddRowColourEnabled.BoolValue = EditorGUILayout.ToggleLeft(sOddRowColourEnabled.PrefGUIContent, sOddRowColourEnabled.BoolValue);
            sComponentPreviewEnabled.BoolValue = EditorGUILayout.ToggleLeft(sComponentPreviewEnabled.PrefGUIContent, sComponentPreviewEnabled.BoolValue);
            sBranchViewEnabled.BoolValue = EditorGUILayout.ToggleLeft(sBranchViewEnabled.PrefGUIContent, sBranchViewEnabled.BoolValue);
            sLayerPreviewEnabled.BoolValue = EditorGUILayout.ToggleLeft(sLayerPreviewEnabled.PrefGUIContent, sLayerPreviewEnabled.BoolValue);
        }        
    }

    public class GrendelPreferencesItem<T> where T : struct
    {
        private string mPrefKey = string.Empty;
        private GUIContent mPrefGUIContent = new GUIContent(string.Empty, string.Empty);
        private T mDefaultValue;

        public GrendelPreferencesItem(string key, T defaultValue)
        {
            mPrefKey = key;
            mDefaultValue = defaultValue;
        }

        public GrendelPreferencesItem(string key, T defaultValue, string label, string tooltip)
        {
            mPrefKey = key;
            mDefaultValue = defaultValue;
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
                    return EditorPrefs.GetInt(mPrefKey, int.Parse(mDefaultValue.ToString()));
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
                    return EditorPrefs.GetFloat(mPrefKey, float.Parse(mDefaultValue.ToString()));
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
                    return EditorPrefs.GetBool(mPrefKey, bool.Parse(mDefaultValue.ToString()));
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
                    return EditorPrefs.GetString(mPrefKey, mDefaultValue.ToString());
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
    }
}
