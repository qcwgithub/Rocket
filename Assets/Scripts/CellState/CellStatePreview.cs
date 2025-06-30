using System;
using UnityEngine;

public class CellStatePreview : CellState
{
    public override bool CanRotate()
    {
        return false;
    }

    public override bool OverrideSpriteShape(out Shape shape)
    {
        shape = default;
        return false;
    }
    
    public bool previewing;
    float previewTimer;
    bool zoomIn;
    Action<Cell> onPreviewFinish;
    public void Preview(Action<Cell> onFinish)
    {
        // Debug.LogWarning($"CellStatePreview.Preview ({this.cell.x}, {this.cell.y})");
        Debug.Assert(!this.previewing);
        this.previewing = true;
        this.previewTimer = 0f;
        this.zoomIn = true;
        this.onPreviewFinish = onFinish;
    }

    public override void MyUpdate(float dt)
    {
        if (this.previewing)
        {
            this.previewTimer += dt;
            float t = Mathf.Clamp01(this.previewTimer / 0.2f);

            if (this.zoomIn)
            {
                this.cell.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.5f, 1.5f, 1f), t);
                if (t >= 1f)
                {
                    this.previewTimer = 0f;
                    this.zoomIn = false;
                }
            }
            else
            {
                this.cell.transform.localScale = Vector3.Lerp(new Vector3(1.5f, 1.5f, 1f), Vector3.one, t);
                if (t >= 1f)
                {
                    this.FinishPreview();
                }
            }
        }
    }

    public void FinishPreview()
    {
        Debug.Assert(this.previewing);
        this.previewing = false;
        this.cell.Idle();
        this.onPreviewFinish?.Invoke(this.cell);
    }

    public void CancelPreview()
    {
        // Debug.LogWarning($"CellStatePreview.CancelPreview ({this.cell.x}, {this.cell.y})");
        Debug.Assert(this.previewing);
        this.previewing = false;
        this.cell.transform.localScale = Vector3.one;
        this.cell.Idle();
    }
}

