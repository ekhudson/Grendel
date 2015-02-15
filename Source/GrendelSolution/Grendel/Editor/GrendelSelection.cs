using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace Grendel.Editor
{
    [InitializeOnLoad]
    public static class GrendelSelection
    {
        private static List<GameObject> sCurrentlySelectedObjects = new List<GameObject>();

        public static List<GameObject> SelectedGameObjects
        {
            get { return sCurrentlySelectedObjects; }
        }

        static GrendelSelection()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        public static void OnEditorUpdate()
        {
            sCurrentlySelectedObjects = new List<GameObject>(Selection.gameObjects);
        }
    }
}
