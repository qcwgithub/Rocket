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
    public Game game;
    public int x;
    public int y;
    public CellStateIdle stateIdle = new CellStateIdle();
    public CellStateRotate stateRotate = new CellStateRotate();
    public CellStatePreview statePreview = new CellStatePreview();
    public CellStateFire stateFire = new CellStateFire();
    public CellStateMove stateMove = new CellStateMove();
    public CellState state;
    public void Init(Game game, int x, int y)
    {
        this.game = game;
        this.x = x;
        this.y = y;

        this.stateIdle.Init(this);
        this.stateRotate.Init(this);
        this.statePreview.Init(this);
        this.stateFire.Init(this);
        this.stateMove.Init(this);

        this.state = this.stateIdle;

        this.Refresh();
    }

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

    public void Refresh()
    {
        CellData cellData = this.game.gameData.boardData.At(this.x, this.y);

        this.RefreshName(this.x, this.y, cellData.shape);
        this.RefreshSprite(this.state.OverrideSpriteShape(out Shape overrideShape) ? overrideShape : cellData.shape);
        this.RefreshColor(cellData.linkedL, cellData.linkedR);
    }

    public void MyUpdate(float dt)
    {
        this.state.MyUpdate(dt);
    }
    public bool rotating
    {
        get
        {
            return this.state == this.stateRotate && this.stateRotate.rotating;
        }
    }
    public bool firing
    {
        get
        {
            return this.state == this.stateFire && this.stateFire.firing;
        }
    }
    public bool previewing
    {
        get
        {
            return this.state == this.statePreview && this.statePreview.previewing;
        }
    }

    public void Rotate(RotateDir rotateDir, Action<Cell, RotateDir> onFinish)
    {
        this.state = this.stateRotate;
        this.stateRotate.Rotate(rotateDir, onFinish);
    }

    public void Fire(Action<Cell> onFinish)
    {
        this.state = this.stateFire;
        this.stateFire.Fire(onFinish);
    }
    public void Preview(float duration, float initTimer, Action<Cell> onFinish)
    {
        this.state = this.statePreview;
        this.statePreview.Preview(duration, initTimer, onFinish);
    }

    public void Move(float fromPositionY, float toPositionY, Action<Cell> onFinish)
    {
        this.state = this.stateMove;
        this.stateMove.Move(fromPositionY, toPositionY, onFinish);
    }

    public void Idle()
    {
        this.state = this.stateIdle;
    }
}