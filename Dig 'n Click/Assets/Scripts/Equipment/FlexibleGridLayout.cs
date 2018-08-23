using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : GridLayoutGroup
{
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
        Vector2 target = GetTargetCellVector2();
        SetTargetCellSize(target);
        constraint = Constraint.FixedColumnCount;
        constraintCount = GetColumnCount();
        childAlignment = TextAnchor.MiddleCenter;
    }

    private Vector2 GetTargetCellVector2()
    {
        float cellX = rectTransform.rect.size.x * FillPercent / GetColumnCount();
        float spaceX = rectTransform.rect.size.x * (1 - FillPercent) / (GetColumnCount() + 1);
        Vector2 scaleX = new Vector2(cellX, spaceX);
        float cellY = rectTransform.rect.size.y * FillPercent / GetRowCount();
        float spaceY = rectTransform.rect.size.y * (1 - FillPercent) / (GetRowCount() + 1);
        Vector2 scaleY = new Vector2(cellY, spaceY);
        float target = Mathf.Min(scaleX.x, scaleY.x);
        return (int) scaleX.x == (int) target ? scaleX : scaleY; //cast to int to avoid loss of precision
    }

    private void SetTargetCellSize(Vector2 target)
    {
        cellSize = new Vector2(target.x, target.x);
        spacing = new Vector2(target.y, target.y);
        padding = new RectOffset((int) target.y, (int) target.y, (int) target.y, (int) target.y);
    }

    private int GetColumnCount()
    {
        return transform.parent.gameObject.GetComponent<EquipmentUI>().ColumnCount;
    }

    private int GetRowCount()
    {
        return transform.parent.gameObject.GetComponent<EquipmentUI>().RowCount;
    }
}