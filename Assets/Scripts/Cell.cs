using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[Flags]
public enum CellSyncPart
{
    Name = 1,
    Sprite = 2,
    Color = 4,
    All = -1
}

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
        this.Refresh();
        // this.shape = shape;

        // this.RefreshName();
        // this.RefreshSprite();
    }

    public void ChangeY(int y)
    {
        this.y = y;
    }

    // temp

    int _name_x = -1;
    int _name_y;
    Shape _name_shape;
    void RefreshName(int x, int y, Shape shape)
    {
        if (_name_x != x || _name_y != y || _name_shape != shape)
        {
            _name_x = x;
            _name_y = y;
            _name_shape = shape;
            this.name = $"({x}, {y}) {shape}";
        }
    }

    Shape _sprite_shape = Shape.Count;
    void RefreshSprite(Shape shape)
    {
        if (_sprite_shape != shape)
        {
            this.spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/V2/" + shape);
        }
    }

    bool _color_inited = false;
    bool _color_L = false;
    bool _color_R = false;
    void RefreshColor(bool linkedL, bool linkedR)
    {
        if (!_color_inited || _color_L != linkedL || _color_R != linkedR)
        {
            _color_inited = true;
            _color_L = linkedL;
            _color_R = linkedR;

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
    }

    // public void SetShape(Shape shape)
    // {
    //     this.shape = shape;
    //     this.RefreshName();
    //     this.RefreshSprite();
    // }

    Shape? overrideSpriteShape = null;
    void OverrideSpriteShape(Shape? shape)
    {
        if (shape != null)
        {
            Debug.Assert(this.overrideSpriteShape == null);
        }
        this.overrideSpriteShape = shape;
    }
    public void Refresh()
    {
        CellData cellData = this.boardData.At(this.x, this.y);

        this.RefreshName(this.x, this.y, cellData.shape);
        this.RefreshSprite(this.overrideSpriteShape != null ? this.overrideSpriteShape.Value : cellData.shape);
        this.RefreshColor(cellData.linkedL, cellData.linkedR);
    }

    public bool rotating;
    public RotateDir rotateDir;
    float rotateTimer;
    Quaternion startRotation;
    Quaternion targetRotation;
    Action<Cell, RotateDir> onRotateFinish;
    // Shape rotate_shape;
    public void Rotate(RotateDir rotateDir, Action<Cell, RotateDir> onFinish)
    {
        Debug.Assert(!this.rotating);
        this.rotating = true;
        this.rotateDir = rotateDir;
        this.rotateTimer = 0f;
        this.startRotation = this.transform.rotation;
        this.targetRotation = this.startRotation * Quaternion.Euler(0f, 0f, rotateDir == RotateDir.CW ? -90f : 90f);
        this.onRotateFinish = onFinish;

        CellData cellData = this.boardData.At(this.x, this.y);
        // this.rotate_shape = cellData.shape;

        // !
        // _sprite_shape = cellData.shape;
        // _color_L = false;
        // _color_R = false;
        this.OverrideSpriteShape(cellData.shape);

        cellData.forbidLink = true;
        cellData.shape = rotateDir == RotateDir.CW
            ? cellData.shape.GetSettings().rotateCW
            : cellData.shape.GetSettings().rotateCCW;
    }

    // public void ResetRotation()
    // {
    //     this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    // }

    public void FinishRotate()
    {
        Debug.Assert(this.rotating);

        this.OverrideSpriteShape(null);

        CellData cellData = this.boardData.At(this.x, this.y);
        cellData.forbidLink = false;

        this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        this.Refresh();

        this.rotating = false;
        this.onRotateFinish?.Invoke(this, this.rotateDir);
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

        if (this.moving)
        {
            Vector3 position = this.transform.position;
            position.y -= 2f * dt;
            if (position.y <= this.targetPositionY)
            {
                position.y = this.targetPositionY;
            }
            this.transform.position = position;
            if (position.y <= this.targetPositionY)
            {
                this.FinishMove();
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
        this.previewing = true;
        this.previewTimer = 0f;
        this.zoomIn = true;
        this.onPreviewFinish = onFinish;
    }

    public void FinishPreview()
    {
        Debug.Assert(this.previewing);
        this.previewing = false;
        this.onPreviewFinish?.Invoke(this);
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
        this.firing = true;
        this.fireTimer = 0f;
        this.onFireFinish = onFinish;
    }

    public void FinishFire()
    {
        Debug.Assert(this.firing);
        this.firing = false;
        this.onFireFinish?.Invoke(this);
    }

    public bool moving;
    public float targetPositionY;
    public Action<Cell> onMoveFinish;
    public void Move(float targetPositionY, Action<Cell> onFinish)
    {
        this.moving = true;
        this.targetPositionY = targetPositionY;
        this.onMoveFinish = onFinish;
    }

    public void FinishMove()
    {
        this.moving = false;
        this.onMoveFinish?.Invoke(this);
    }
}