using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BoardData boardData;
    public int x;
    public int y;
    public void Init(BoardData boardData, int x, int y)
    {
        this.boardData = boardData;
        this.x = x;
        this.y = y;
        this.Apply();
        // this.shape = shape;

        // this.RefreshName();
        // this.RefreshSprite();
    }

    // temp

    void RefreshName(Shape shape)
    {
        this.name = $"({this.x},{this.y}) {shape}";
    }

    void RefreshSprite(Shape shape)
    {
        this.spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/V2/" + shape);
    }

    void RefreshColor(bool linkedL, bool linkedR)
    {
        if (linkedL && linkedR)
        {
            this.spriteRenderer.color = Color.green;
        }
        else if (linkedL)
        {
            this.spriteRenderer.color = Color.yellow;
        }
        else if (linkedR)
        {
            this.spriteRenderer.color = Color.red;
        }
        else
        {
            this.spriteRenderer.color = Color.white;
        }
    }

    // public void SetShape(Shape shape)
    // {
    //     this.shape = shape;
    //     this.RefreshName();
    //     this.RefreshSprite();
    // }

    public void Apply()
    {
        CellData cellData = this.boardData.At(this.x, this.y);
        // this.shape = cellData.shape;
        // this.linkedL = cellData.linkedL;
        // this.linkedR = cellData.linkedR;

        this.RefreshName(cellData.shape);
        this.RefreshSprite(cellData.shape);
        this.RefreshColor(cellData.linkedL, cellData.linkedR);
    }

    public bool rotating;
    public RotateDir rotateDir;
    float rotateTimer;
    Quaternion startRotation;
    Quaternion targetRotation;
    Action<Cell, RotateDir> onRotateFinish;
    public void Rotate(RotateDir rotateDir, Action<Cell, RotateDir> onFinish)
    {
        this.rotating = true;
        this.rotateDir = rotateDir;
        this.rotateTimer = 0f;
        this.startRotation = this.transform.rotation;
        this.targetRotation = this.startRotation * Quaternion.Euler(0f, 0f, rotateDir == RotateDir.CW ? -90f : 90f);
        this.onRotateFinish = onFinish;
    }

    public void ResetRotation()
    {
        this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void FinishRotate()
    {
        Debug.Assert(this.rotating);
        if (this.rotating)
        {
            this.rotating = false;
            this.onRotateFinish?.Invoke(this, this.rotateDir);
        }
    }

    public void MyUpdate(float dt)
    {
        if (this.rotating)
        {
            this.rotateTimer += dt;
            float t = Mathf.Clamp01(this.rotateTimer / 0.2f);
            this.transform.rotation = Quaternion.Lerp(this.startRotation, this.targetRotation, t);
            if (t >= 1f)
            {
                this.FinishRotate();
            }
        }

        if (this.previewing)
        {
            this.previewTimer += dt;
            float t = Mathf.Clamp01(this.previewTimer / 3f);

            if (this.zoomIn)
            {
                this.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.5f, 1.5f, 1f), t);
                if (t >= 1f)
                {
                    this.previewTimer = 0f;
                    this.zoomIn = false;
                }
            }
            else
            {
                this.transform.localScale = Vector3.Lerp(new Vector3(1.5f, 1.5f, 1f), Vector3.one, t);
                if (t >= 1f)
                {
                    this.FinishPreview();
                }
            }
        }

        if (this.firing)
        {
            this.fireTimer += dt;
            float t = Mathf.Clamp01(this.fireTimer / 3f);

            this.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            if (t >= 1f)
            {
                this.FinishFire();
            }
        }
    }

    public bool previewing;
    float previewTimer;
    bool zoomIn;
    Action<Cell> onPreviewFinish;
    public void Preview(Action<Cell> onFinish)
    {
        Debug.Assert(!this.previewing);
        if (!this.previewing)
        {
            this.previewing = true;
            this.previewTimer = 0f;
            this.zoomIn = true;
            this.onPreviewFinish = onFinish;
        }
    }

    public void FinishPreview()
    {
        Debug.Assert(this.previewing);
        if (this.previewing)
        {
            this.previewing = false;
            this.onPreviewFinish?.Invoke(this);
        }
    }

    public void CancelPreview()
    {
        Debug.Assert(this.previewing);
        this.previewing = false;
        this.transform.localScale = Vector3.one;
    }

    public bool firing;
    float fireTimer;
    Action<Cell> onFireFinish;
    public void Fire(Action<Cell> onFinish)
    {
        Debug.Assert(!this.firing);
        if (!this.firing)
        {
            this.firing = true;
            this.fireTimer = 0f;
            this.onFireFinish = onFinish;
        }
    }

    public void FinishFire()
    {
        Debug.Assert(this.firing);
        if (this.firing)
        {
            this.firing = false;
            this.onFireFinish?.Invoke(this);
        }
    }
}