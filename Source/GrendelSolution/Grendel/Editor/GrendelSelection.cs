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
        private static List<Transform> sAllAffectedObjects = new List<Transform>();

        public static List<GameObject> SelectedGameObjects
        {
            get { return sCurrentlySelectedObjects; }
        }

        public static bool IsTransformAffectedBySelection(Transform transform)
        {
            sAllAffectedObjects.Clear();

            foreach(GameObject go in sCurrentlySelectedObjects)
            {
                sAllAffectedObjects.AddRange( go.GetComponentsInChildren<Transform>() );
            }

            if (sAllAffectedObjects.Contains(transform))
            {
                return true;
            }
            else
            {
                return false;
            }
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
