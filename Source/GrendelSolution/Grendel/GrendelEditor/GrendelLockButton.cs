using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    internal class GrendelLockButton
    {
        private static string sLockButtonStyleName = "IN LockButton";
        private static GUIStyle sLockButtonStyle = null;
        private static GUIStyle sLockedLabelStyle = null;
        private static Color sLockedLabelColor = Color.Lerp(Color.yellow, GrendelEditorGUIUtility.CurrentSkinViewColor, 0.45f);
        
        internal static void DrawLockButton(GrendelObjectData obj, Rect iconPosition)
        {
            if (obj == null)
            {
                return;
            }

            if (sLockButtonStyle == null)
            {
                sLockButtonStyle = sLockButtonStyleName;

                sLockedLabelStyle = new GUIStyle(GUI.skin.label);
                sLockedLabelStyle.fontStyle = FontStyle.Bold;
            }

            bool locked = obj.IsLocked;

            if (!locked && Event.current.type == EventType.Repaint)
            {
                GUI.enabled = false;
                sLockButtonStyle.Draw(iconPosition, false, false, false, false);
                GUI.enabled = true;
            }

            locked = GUI.Toggle(iconPosition, locked, string.Empty, locked ? sLockButtonStyle : GUIStyle.none);

            sLockButtonStyle.normal.textColor = Color.gray;

            if (locked != obj.IsLocked)
            {
                obj.SetLock(locked, true);
                EditorUtility.SetDirty(obj);
                SceneView.RepaintAll();

                if (locked && Selection.Contains(obj))
                {
                    List<GameObject> selectedObjects = new List<GameObject>(Selection.gameObjects);
                    selectedObjects.Remove(obj.gameObject);
                    Selection.objects = selectedObjects.ToArray();
                }
            }

            GUI.color = Color.white;
        }   
    }
}
