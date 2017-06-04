using System;
using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    [CustomEditor(typeof(GrendelFolderComponent))]
    public class GrendelFolderEditor : Editor
    {
        [MenuItem("GameObject/Create New Folder &#f", false, 20)]
        public static void CreateFolder()
        {
            GameObject newFolder = new GameObject("New Folder");
            newFolder.AddComponent<GrendelFolderComponent>();
            newFolder.transform.parent = Selection.activeTransform;
            Selection.activeTransform = newFolder.transform;
        }

        private GrendelFolderComponent Target
        {
            get
            {
                return target as GrendelFolderComponent;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(Target);
                EditorApplication.RepaintHierarchyWindow();
            }
        }
    }
}
