using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public CellData cell;
    public int x;
    public int y;
    public CellState state;
    public void Init(CellData cell, int x, int y, CellState state)
    {
        this.cell = cell;
        this.x = x;
        this.y = y;
        this.state = state;
        // this.name = $"({cell.x},{cell.y}) {cell.shape}";

        this.ApplyShape();
    }

    public void ApplyShape()
    {
        this.spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/" + cell.shape);
    }

    public void ApplyColor()
    {
        if (this.cell.green)
        {
            this.spriteRenderer.color = Color.green;
        }
        else if (this.cell.yellow)
        {
            this.spriteRenderer.color = Color.yellow;
        }
        else if (this.cell.red)
        {
            this.spriteRenderer.color = Color.red;
        }
        else
        {
            this.spriteRenderer.color = Color.white;
        }
    }

    public void OnClick(ClickAction action)
    {
        switch (this.state)
        {
            case CellState.Falling:
                break;
            case CellState.Still:
            case CellState.Warn:
                {
                    this.cell.shape = this.cell.shape.GetSettings().rotateCCW;
                    this.ApplyShape();
                }
                break;
            case CellState.Rotating:
                break;
            case CellState.Locked:
                break;
        }
    }

    public void EnterState(CellState state)
    {
        switch (state)
        {
            case CellState.Falling:
                break;
            case CellState.Still:
                break;
            case CellState.Rotating:
                this.spriteRenderer.color = Color.white;
                break;
            case CellState.Warn:
                break;
            case CellState.Locked:
                break;
        }
    }
}