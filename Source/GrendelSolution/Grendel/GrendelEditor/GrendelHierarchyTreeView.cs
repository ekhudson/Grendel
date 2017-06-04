using System;

using UnityEngine;
using UnityEditor;

namespace Grendel.GrendelEditor
{
    internal static class GrendelHierarchyTreeView
    {
        private static GUIStyle sTreeViewStyle = null;
        private const float kTreeViewOffset = 24f;
        private static Vector2 sTreeViewShadowOffset = new Vector2(1, 1);
        private const string kTreeItemTop = "└";
        private const string kTreeItemBottom = "┌";
        private const string kTreeOuterBranch = "¦";
        private const string kTreeEndOfBranch = "ˉ";
        private static Rect sPreviousItemBranchRect = new Rect();
        private static Color sTreeBranchShadowColor = Color.white;
        private static Color STreeBranchNormalColor = Color.gray;
        private static Color sTreeBranchSelectedColor = new Color(0.15f, 0.45f, 1f, 1.0f);

        private static void SetupStyle()
        {
            sTreeViewStyle = new GUIStyle(GUIStyle.none);
            sTreeViewStyle.fontSize = 13;
        }

        internal static void DrawTreeBranch(GameObject gameObject, Rect position, int currentIndentAmount, int previousIndentAmount)
        {
            if (GrendelHierarchyView.CurrentParentCount <= 0 || gameObject == null)
            {
                return;
            }

            if (sTreeViewStyle == null)
            {
                SetupStyle();
            }

            position.x -= kTreeViewOffset;
            position.y -= 2;

            bool parentSelected = false;

            foreach (Transform parent in GrendelHierarchyView.CurrentParents)
            {
                if (GrendelSelection.SelectedGameObjects.Contains(parent.gameObject))
                {
                    parentSelected = true;
                }
            }

            Color currentTreeColour = parentSelected ? sTreeBranchSelectedColor: STreeBranchNormalColor;
            Color shadowColor = sTreeBranchShadowColor;

            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                currentTreeColour *= GrendelEditorGUIUtility.PlaymodeTintColor;
                shadowColor *= GrendelEditorGUIUtility.PlaymodeTintColor;
            }

            GrendelGUI.ShadedLabel(position, kTreeItemTop, currentTreeColour, shadowColor, sTreeViewShadowOffset, sTreeViewStyle);

            if (previousIndentAmount <= currentIndentAmount && previousIndentAmount > 0 && !(sPreviousItemBranchRect.y > position.y))
            {
                if (gameObject.transform.GetSiblingIndex() == 0 && gameObject.transform.parent.GetSiblingIndex() + 1 == gameObject.transform.parent.parent.childCount)
                {
                    //do nothing
                }
                else
                {
                    Rect pos = new Rect(sPreviousItemBranchRect);
                    pos.y -= 1;
                    GrendelGUI.ShadedLabel(pos, kTreeItemBottom, currentTreeColour, shadowColor, sTreeViewShadowOffset, sTreeViewStyle);
                }

            }

            sPreviousItemBranchRect = new Rect(position);

            if (currentIndentAmount <= 1)
            {
                return;
            }

            float xPos = position.x;
            float yPos = position.y;

            for (int i = 1; i < currentIndentAmount; i++)
            {
                position.x = (xPos - (GrendelHierarchyView.kIndentWidth * i));
                position.y = yPos;

                Color outerBranchColor = STreeBranchNormalColor;

                if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying)
                {
                    outerBranchColor *= GrendelEditorGUIUtility.PlaymodeTintColor;
                }

                if (GrendelHierarchyView.CurrentParents[i] != null)
                {
                    if ((i - 1 >= 0) && (GrendelHierarchyView.CurrentParents[i - 1].parent != null && (GrendelHierarchyView.CurrentParents[i - 1].GetSiblingIndex() + 1 < GrendelHierarchyView.CurrentParents[i - 1].parent.childCount)))
                    {
                        if (GrendelSelection.IsTransformAffectedBySelection(GrendelHierarchyView.CurrentParents[i - 1]))
                        {
                            outerBranchColor = sTreeBranchSelectedColor;
                        }

                        position.x += 1;

                        GrendelGUI.ShadedLabel(position, kTreeOuterBranch, outerBranchColor, shadowColor, sTreeViewShadowOffset, sTreeViewStyle);
                    }
                }
            }
        }

    }
}
