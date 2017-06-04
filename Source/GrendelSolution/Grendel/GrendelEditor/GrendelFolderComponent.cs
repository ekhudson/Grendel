using System;
using UnityEngine;
using UnityEditor;

//#if UNITY_EDITOR
namespace Grendel.GrendelEditor
{
    [System.Serializable]
    public class GrendelFolderComponent : MonoBehaviour
    {
        //private Color mFolderColor;
        public Color FolderColor = Color.cyan;
        //private Transform mTransformReference;

        //internal GrendelFolderComponent()
        //{
        //    //TODO: figure out how to setup the folder so it accepts children but can't be edited            
        //}

        //internal Transform TransformReference
        //{
        //    get
        //    {
        //        if (mTransformReference == null)
        //        {
        //            mTransformReference = transform;
        //        }

        //        return mTransformReference;
        //    }

        //}      
    }
}
//#endif
