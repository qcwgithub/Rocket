using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BoardData boardData;
    public int x;
    public int y;
    public CellState state;
    public void Init(BoardData boardData, int x, int y, CellState state)
    {
        this.boardData = boardData;
        this.x = x;
        this.y = y;
        this.state = state;
        // this.name = $"({cell.x},{cell.y}) {cell.shape}";
        this.ApplyName();

        this.ApplyShape();
    }

    void ApplyName()
    {
        this.name = $"({this.x},{this.y}) {this.boardData.At(this.x, this.y).shape}";
    }

    public void ApplyShape()
    {
        CellData cellData = this.boardData.At(this.x, this.y);
        this.spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/V1/" + cellData.shape);
    }

    public void ApplyColor()
    {
        CellData cellData = this.boardData.At(this.x, this.y);
        if (cellData.green)
        {
            this.spriteRenderer.color = Color.green;
        }
        else if (cellData.yellow)
        {
            this.spriteRenderer.color = Color.yellow;
        }
        else if (cellData.red)
        {
            this.spriteRenderer.color = Color.red;
        }
        else
        {
            this.spriteRenderer.color = Color.white;
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