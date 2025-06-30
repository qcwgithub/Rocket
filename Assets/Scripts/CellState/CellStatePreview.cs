using System;
using UnityEngine;

public class CellStatePreview : CellState
{
    public override bool AskRotate()
    {
        if (this.previewing)
        {
            this.CancelPreview();
        }
        return true;
    }

    public override bool OverrideSpriteShape(out Shape shape)
    {
        shape = default;
        return false;
    }

    public bool previewing;
    float duration_half;
    float previewTimer;
    bool zoomIn;
    Action<Cell> onPreviewFinish;
    public void Preview(float duration, float initTimer, Action<Cell> onFinish)
    {
        // Debug.LogWarning($"CellStatePreview.Preview ({this.cell.x}, {this.cell.y})");
        Debug.Assert(!this.previewing, $"{this.cell.x} {this.cell.y}");
        this.previewing = true;
        this.duration_half = duration * 0.5f;
        if (initTimer < duration * 0.5f)
        {
            this.previewTimer = initTimer;
            this.zoomIn = true;
        }
        else
        {
            this.previewTimer = initTimer - duration * 0.5f;
            this.zoomIn = false;
            this.Refresh1(out float _);
        }
        this.onPreviewFinish = onFinish;
    }

    void Refresh1(out float t)
    {
        t = Mathf.Clamp01(this.previewTimer / this.duration_half);
        if (this.zoomIn)
        {
            this.cell.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.5f, 1.5f, 1f), t);
        }
        else
        {
            this.cell.transform.localScale = Vector3.Lerp(new Vector3(1.5f, 1.5f, 1f), Vector3.one, t);
        }
    }

    public override void MyUpdate(float dt)
    {
        if (this.previewing)
        {
            this.previewTimer += dt;

            this.Refresh1(out float t);

            if (this.zoomIn)
            {
                if (t >= 1f)
                {
                    this.previewTimer = 0f;
                    this.zoomIn = false;
                }
            }
            else
            {
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

