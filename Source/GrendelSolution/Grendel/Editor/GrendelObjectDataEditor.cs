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

            mComponents = Target.gameObject.GetComponents(typeof(Component));

            foreach (Component comp in mComponents)
            {
                GUILayout.Label(comp.GetType().Name);
            }
        }
    }
}
