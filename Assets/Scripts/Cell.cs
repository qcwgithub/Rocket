using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public CellData cell;
    public SpriteRenderer spriteRenderer;
    public void Init(CellData cell)
    {
        this.cell = cell;
        this.name = $"({cell.x},{cell.y}) {cell.shape}";

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
}