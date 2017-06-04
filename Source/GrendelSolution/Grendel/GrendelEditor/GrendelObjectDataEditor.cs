using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    [CustomEditor(typeof(GrendelObjectData))]
    internal class GrendelObjectDataEditor : Editor
    {
        private Component[] mComponents;


        public GrendelObjectData Target
        {
            get
            {
                return target as GrendelObjectData;
            }
        }
        

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Label("Layer: " + Target.gameObject.layer);

            mComponents = Target.gameObject.GetComponents(typeof(Component));

            foreach (Component comp in mComponents)
            {
                if (comp == null)
                {
                    continue;
                }

                GUILayout.Label(comp.GetType().Name);
            }

            GUILayout.Label("Hide Flags: " + Target.hideFlags.ToString());
        }

        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected | GizmoType.Selected)]
        public static void OnDrawGizmos(GrendelObjectData grendelObject, GizmoType gizmoType)
        {
            
        }
    }
}
