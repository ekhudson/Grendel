using System;
using UnityEngine;
using UnityEditor;

//#if UNITY_EDITOR
namespace Grendel.GrendelEditor
{
    public class GrendelFolderComponent : MonoBehaviour
    {
        private Color mFolderColor = Color.clear;   

        public Color FolderColor
        {
            get
            {
                return mFolderColor;
            }
            set
            {
                mFolderColor = value;
            }
        }            
    }
}
//#endif
