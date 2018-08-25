using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : GridLayoutGroup
{
    public int ColumnCount;
    public float FillPercent;

    public override void SetLayoutHorizontal()
    {
        UpdateCellSize();
        base.SetLayoutHorizontal();
    }

    public override void SetLayoutVertical()
    {
        UpdateCellSize();
        base.SetLayoutVertical();
    }

    private void UpdateCellSize()
    {
        float cellX = rectTransform.rect.size.x * FillPercent / ColumnCount;
        float spaceX = rectTransform.rect.size.x * (1 - FillPercent) / (ColumnCount - 1);
        cellSize = new Vector2(cellX, cellX);
        spacing = new Vector2(spaceX, spaceX);
        constraint = Constraint.FixedColumnCount;
        constraintCount = ColumnCount;
        childAlignment = TextAnchor.UpperCenter;
    }
}