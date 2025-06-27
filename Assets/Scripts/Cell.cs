using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Cell : MonoBehaviour, ICell
{
    public SpriteRenderer spriteRenderer;
    public int x { get; set; }
    public int y { get; set; }
    public bool yellow { get; set; }
    public bool red { get; set; }
    public bool green
    {
        get
        {
            return this.yellow && this.red;
        }
    }
    public Shape shape { get; set; }
    public CellState state;
    public void Init(int x, int y, CellState state, Shape shape)
    {
        this.x = x;
        this.y = y;
        this.state = state;
        this.shape = shape;
    }

    void ApplyName()
    {
        this.name = $"({this.x},{this.y}) {this.shape}";
    }

    public void ApplyShape()
    {
        this.spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/V2/" + this.shape);
    }

    public void ApplyColor()
    {
        if (this.green)
        {
            this.spriteRenderer.color = Color.green;
        }
        else if (this.yellow)
        {
            this.spriteRenderer.color = Color.yellow;
        }
        else if (this.red)
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

    public bool rotating;
    float rotateTimer;
    Quaternion startRotation;
    Quaternion targetRotation;
    Action<Cell> onRotateFinish;
    public void Rotate(string what, Action<Cell> onFinish)
    {
        this.rotating = true;
        this.rotateTimer = 0f;
        this.startRotation = this.transform.rotation;
        this.targetRotation = this.startRotation * Quaternion.Euler(0f, 0f, what == "cw" ? -90f : 90f);
        this.onRotateFinish = onFinish;
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
                this.rotating = false;
                this.onRotateFinish?.Invoke(this);
            }
        }
    }
}