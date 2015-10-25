using System;
using System.Collections.Generic;

using UnityEngine;

namespace Grendel.GrendelEditor
{
    [System.Serializable]
    internal class GrendelObjectData : MonoBehaviour
    {
        [SerializeField]private bool mIsLocked = false;
        [SerializeField]private bool mIsHidden = false;
        [SerializeField]private string mNote = string.Empty;
        [SerializeField]private string[] mLabels = new string[0];
        private GameObject mGameObjectReference = null;

        public bool IsLocked        { get { return mIsLocked; } }
        public bool IsHidden        { get { return mIsHidden; } }
        public string Note          { get { return mNote; } }
        public string[] Labels      { get { return mLabels; } }

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

        public void SetLock(bool isLocked)
        {
            SetLock(isLocked, false);
        }

        public void SetLock(bool isLocked, bool isRecursive)
        {
            mIsLocked = isLocked;
        }

        public void SetHidden(bool isHidden)
        {
            SetHidden(isHidden, false);
        }

        public void SetHidden(bool isHidden, bool isRecursive)
        {
            mIsHidden = isHidden;

            GameObjectReference.SetActive(!isHidden);
        }
    }
}
