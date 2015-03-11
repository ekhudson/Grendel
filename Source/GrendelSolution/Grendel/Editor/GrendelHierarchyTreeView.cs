using System;

using UnityEngine;
using UnityEditor;

namespace Grendel.Editor
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
        private static Color sTreeBranchSelectedColor = Color.blue;

        private static void SetupStyle()
        {
            sTreeViewStyle = new GUIStyle(GUIStyle.none);
            sTreeViewStyle.fontSize = 13;
        }

        internal static void DrawTreeBranch(GameObject gameObject, Rect position, int currentIndentAmount, int previousIndentAmount)
        {
            if (GrendelHierarchyView.CurrentParentCount <= 0)
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

            GrendelGUI.ShadedLabel(position, kTreeItemTop, currentTreeColour, sTreeBranchShadowColor, sTreeViewShadowOffset, sTreeViewStyle);

            if (previousIndentAmount <= currentIndentAmount && previousIndentAmount > 0 && !(sPreviousItemBranchRect.y > position.y))
            {
                Rect pos = new Rect(sPreviousItemBranchRect);
                pos.y -= 1;
                GrendelGUI.ShadedLabel(pos, kTreeItemBottom, currentTreeColour, sTreeBranchShadowColor, sTreeViewShadowOffset, sTreeViewStyle);
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

                for (int j = i - 1; j < currentIndentAmount; j++)
                {
                    if (GrendelSelection.SelectedGameObjects.Contains(GrendelHierarchyView.CurrentParents[j].gameObject))
                    {
                        outerBranchColor = STreeBranchNormalColor;
                        break;
                    }
                }

                if (gameObject.transform.GetSiblingIndex() == gameObject.transform.parent.childCount - 1 &&
                    i == 1)
                {
                    if (gameObject.transform.parent.parent != null && gameObject.transform.parent.GetSiblingIndex() != gameObject.transform.parent.parent.childCount - 1)
                    {
                        position.x += 1;
                        GrendelGUI.ShadedLabel(position, kTreeOuterBranch, outerBranchColor, sTreeBranchShadowColor, sTreeViewShadowOffset, sTreeViewStyle);
                    }
                    else
                    {
                        GrendelGUI.ShadedLabel(position, kTreeItemTop, currentTreeColour, sTreeBranchShadowColor, sTreeViewShadowOffset, sTreeViewStyle);

                        position.x += 9f;
                        position.y += 4f;

                        GrendelGUI.ShadedLabel(position, kTreeEndOfBranch, currentTreeColour, sTreeBranchShadowColor, sTreeViewShadowOffset, sTreeViewStyle);
                    }
                }
                else if (gameObject.transform)
                {
                    position.x += 1;

                    GrendelGUI.ShadedLabel(position, kTreeOuterBranch, outerBranchColor, sTreeBranchShadowColor, sTreeViewShadowOffset, sTreeViewStyle);
                }
            }
        }

    }
}
