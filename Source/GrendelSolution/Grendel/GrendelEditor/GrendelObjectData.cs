//#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Grendel.GrendelEditor
{
    [System.Serializable]
    [ExecuteInEditMode]
    internal class GrendelObjectData : MonoBehaviour
    {
        [SerializeField]private bool mIsLocked = false;
        [SerializeField]private bool mIsHidden = false;
        [SerializeField]private string mNote = string.Empty;
        [SerializeField]private int mTrueLayer = 0;
        [SerializeField]private string[] mLabels = new string[0];
        [SerializeField]private int mPreviewComponentIndex = 1;
        private GameObject mGameObjectReference = null;
        public const int kHiddenLayer = 7;

        public bool IsLocked                { get { return mIsLocked; } }
        public bool IsHidden                { get { return mIsHidden; } }
        public string Note                  { get { return mNote; } }
        public string[] Labels              { get { return mLabels; } }
        public int PreviewComponentIndex    { get { return mPreviewComponentIndex; } }
        
        public int TrueLayer                
        { 
            get { return mTrueLayer; } 
        }

        private GameObject GameObjectReference
        {
            get
            {
                if (mGameObjectReference == null)
                {
                    mGameObjectReference = gameObject;
                }

                return mGameObjectReference;
            }
        }

        private void Awake()
        {
            RemoveRenderHandlers();
            AddRenderHandlers();
        }

        public void SetLock(bool isLocked)
        {
            mIsLocked = isLocked;

            if (isLocked)
            {
                GameObjectReference.hideFlags = HideFlags.NotEditable;
                Renderer rend = GameObjectReference.GetComponent<Renderer>();

                if (rend != null)
                {
                    rend.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;
                }
            }
            else
            {
                GameObjectReference.hideFlags = HideFlags.None;
                Renderer rend = gameObject.GetComponent<Renderer>();

                if (rend != null)
                {
                    rend.hideFlags = HideFlags.None;
                }
            }
        }

        public void SetLock(bool isLocked, bool isRecursive)
        {
            SetLock(isLocked);

            if (isRecursive)
            {
                foreach (GrendelObjectData child in GameObjectReference.GetComponentsInChildren(typeof(GrendelObjectData)))
                {
                    if (child != null)
                    {
                        child.SetLock(isLocked);
                    }
                }
            }            
        }

        public void SetHidden(bool isHidden)
        {
            mIsHidden = isHidden;

            if (mIsHidden)
            {
                AddRenderHandlers();

                if (GrendelSelection.SelectedGameObjects.Contains(gameObject))
                {
                    List<GameObject> objects = GrendelSelection.SelectedGameObjects;
                    objects.Remove(gameObject);
                    Selection.objects = objects.ToArray();
                }
            }
            else
            {
                RemoveRenderHandlers();
            }
        }

        public void SetHidden(bool isHidden, bool isRecursive)
        {
            SetHidden(isHidden);

            if (isRecursive)
            {
                GrendelObjectData[] objects = GameObjectReference.GetComponentsInChildren<GrendelObjectData>();

                foreach (GrendelObjectData child in objects)
                {
                    if (child != null)
                    {
                        child.SetHidden(isHidden);
                    }
                }
            }
        }

        public void OnPreCullHandler(Camera cam)
        {
            if (this == null || gameObject == null || cam.name != "SceneCamera")
            {
                return;
            }           

            cam.cullingMask = ~(1 << 7);

            if (gameObject.layer != kHiddenLayer)
            {
                mTrueLayer = gameObject.layer;
            }

            if (mIsHidden && gameObject.layer != kHiddenLayer)
            {
                gameObject.layer = kHiddenLayer;
                
            }
            else if (!mIsHidden && gameObject.layer == kHiddenLayer)
            {
                gameObject.layer = TrueLayer;
            }
        }

        public void OnPostRenderHandler(Camera cam)
        {
            if (this == null || gameObject == null || cam.name != "SceneCamera")
            {
                return;
            }

            if (mIsHidden && gameObject.layer == kHiddenLayer)
            {
                gameObject.layer = mTrueLayer;
            }            
        }

        private void AddRenderHandlers()
        {
            Camera.onPreCull += OnPreCullHandler;
            Camera.onPostRender += OnPostRenderHandler;
        }

        private void RemoveRenderHandlers()
        {
            Camera.onPreCull -= OnPreCullHandler;
            Camera.onPostRender -= OnPostRenderHandler;
        }
    }
}
//#endif
