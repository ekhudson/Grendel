using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using Grendel.Extensions;

namespace Grendel.Editor
{
    [InitializeOnLoad]
    public class GameObjectEditorInitializer
    {
        static GameObjectEditorInitializer()
        {
            if (EditorApplication.update != GameObjectEditor.Update)
            {
                EditorApplication.update += GameObjectEditor.Update;
            }
        }
    }

    public class GameObjectEditor : UnityEditor.Editor
    {
        private static HashSet<UnityEngine.Object> sPreviousSelectedGameObjects = new HashSet<UnityEngine.Object>();

        public static void Update()
        {
            // Object[] objects = FilterObjects(Selection.objects);
            Selection.objects = FilterObjects(Selection.objects);


            //if (sPreviousSelectedGameObjects.Count > 0 && Selection.objects.Length > 0)
            //{
            //    if (!sPreviousSelectedGameObjects.SetEquals(objects))
            //    {
            //        Selection.objects = objects;
            //        sPreviousSelectedGameObjects = new HashSet<Object>(objects);
            //    }
            //}
            //else if (Selection.objects.Length > 0)
            //{
            //    Selection.objects = objects;
            //    sPreviousSelectedGameObjects = new HashSet<Object>(objects);
            //}
        }

        private static UnityEngine.Object[] FilterObjects(UnityEngine.Object[] objects)
        {
            List<UnityEngine.Object> newSelection = new List<UnityEngine.Object>(Selection.objects);

            for (int i = newSelection.Count - 1; i >= 0; i--)
            {
                if (newSelection[i] == null)
                {
                    newSelection.RemoveAt(i);
                    continue;
                }

                if ((newSelection[i] as GameObject).IsLocked()) //filter out locked gameobjects
                {
                    newSelection.RemoveAt(i);
                }
            }

            return newSelection.ToArray();
        }

        public void OnSelectionChange()
        {
            Debug.Log("HEY");
        }
    }
}
