using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

//TODO: I may not actually be using this class anymore
namespace Grendel.GrendelEditor
{
    [InitializeOnLoad]
    internal static class GrendelHierarchy
    {
        private static Dictionary<int, GrendelObjectData> mHierarchyObjectDict = new Dictionary<int, GrendelObjectData>();

        static GrendelHierarchy()
        {
            EditorApplication.hierarchyWindowChanged += OnHierarchyWindowChanged;
        }

        private static void OnHierarchyWindowChanged()
        {

        }

        internal static bool IsObjectHidden(int instanceID)
        {
            if (mHierarchyObjectDict.ContainsKey(instanceID))
            {
                return mHierarchyObjectDict[instanceID].IsHidden;
            }
            else
            {
                mHierarchyObjectDict.Add(instanceID, new GrendelObjectData());
                return false;
            }
        }

        internal static bool IsObjectLocked(int instanceID)
        {
            if (mHierarchyObjectDict.ContainsKey(instanceID))
            {
                return mHierarchyObjectDict[instanceID].IsLocked;
            }
            else
            {
                mHierarchyObjectDict.Add(instanceID, new GrendelObjectData());
                return false;
            }
        }

        internal static GrendelObjectData EnsureEntryExists(int instanceID)
        {
            if (mHierarchyObjectDict.ContainsKey(instanceID))
            {
                return mHierarchyObjectDict[instanceID];
            }
            else
            {
                mHierarchyObjectDict.Add(instanceID, new GrendelObjectData());
                return mHierarchyObjectDict[instanceID];
            }
        }

        internal static GrendelObjectData EnsureObjectHasGrendelData(GameObject gameObject)
        {
            GrendelObjectData objectData = gameObject.GetComponent<GrendelObjectData>();

            if (objectData == null)
            {
                objectData = (GrendelObjectData)gameObject.AddComponent<GrendelObjectData>();
                //objectData.hideFlags = HideFlags.HideInInspector; //TODO: Remove for release, exposed for debugging
                EditorUtility.SetDirty(gameObject);
            }

            return objectData;            
        }

        internal static void SetObjectLock(GrendelObjectData objectData, bool isLocked, bool setRecursively)
        {
            bool previousStatus = objectData.IsLocked;

            if (isLocked)
            {
                if (!previousStatus)
                {
                    objectData.gameObject.hideFlags |= (HideFlags.NotEditable);

                    if (setRecursively && objectData.gameObject.transform.childCount > 0)
                    {
                        Transform[] children = objectData.gameObject.GetComponentsInChildren<Transform>();

                        foreach (Transform child in children)
                        {
                            child.gameObject.hideFlags |= (HideFlags.NotEditable);
                            EnsureEntryExists(child.gameObject.GetInstanceID()).SetLock(true);
                        }
                    }
                }
            }
            else if (previousStatus)
            {
                objectData.gameObject.hideFlags &= ~(HideFlags.NotEditable);
                EnsureEntryExists(objectData.gameObject.GetInstanceID()).SetLock(false);

                if (setRecursively && objectData.gameObject.transform.childCount > 0)
                {
                    Transform[] children = objectData.gameObject.GetComponentsInChildren<Transform>();

                    foreach (Transform child in children)
                    {
                        child.gameObject.hideFlags &= ~(HideFlags.NotEditable);
                        EnsureEntryExists(child.gameObject.GetInstanceID()).SetLock(false);
                    }
                }
            }

            SceneView.RepaintAll();
            EditorApplication.RepaintHierarchyWindow();
            HandleUtility.Repaint();
        }

        //[MenuItem("Grendel/Fix Hidden Objects")]
        private static void FixHiddenObjects()
        {
            GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in objs)
            {
                obj.hideFlags &= ~(HideFlags.NotEditable | HideFlags.HideInInspector);
                EnsureEntryExists(obj.GetInstanceID()).SetLock(false);
            }
        }


    }
}
