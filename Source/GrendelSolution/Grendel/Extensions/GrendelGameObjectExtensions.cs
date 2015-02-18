using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace Grendel.Extensions
{
    public static class GrendelGameObjectExtensions
    {
        public static bool IsLocked(this GameObject gameObject)
        {
            return (gameObject.hideFlags & HideFlags.NotEditable) != 0;
        }

        public static void SetLock(this GameObject gameObject, bool locked, bool recursive)
        {
            if (locked)
            {
                if (!gameObject.IsLocked())
                {
                    gameObject.hideFlags |= (HideFlags.NotEditable | HideFlags.HideInInspector);

                    if (recursive && gameObject.transform.childCount > 0)
                    {
                        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

                        foreach (Transform child in children)
                        {
                            child.gameObject.hideFlags |= HideFlags.NotEditable | HideFlags.HideInInspector;
                        }
                    }
                }
            }
            else if (gameObject.IsLocked())
            {
                gameObject.hideFlags &= ~(HideFlags.NotEditable | HideFlags.HideInInspector);

                if (recursive && gameObject.transform.childCount > 0)
                {
                    Transform[] children = gameObject.GetComponentsInChildren<Transform>();

                    foreach (Transform child in children)
                    {
                        child.gameObject.hideFlags &= ~HideFlags.NotEditable | ~HideFlags.HideInInspector;
                    }
                }
            }
        }
    }
}
