using System;
using UnityEngine;

public class CellStateRotate : CellState
{
    public override bool CanRotate()
    {
        return true;
    }

    public override bool OverrideSpriteShape(out Shape shape)
    {
        if (this.rotating)
        {
            shape = this.overrideShape;
            return true;
        }
        shape = default;
        return false;
    }

    public bool rotating;
    public RotateDir rotateDir;
    float rotateTimer;
    Quaternion startRotation;
    Quaternion targetRotation;
    Action<Cell, RotateDir> onRotateFinish;
    Shape overrideShape;
    public void Rotate(RotateDir rotateDir, Action<Cell, RotateDir> onFinish)
    {
        Debug.Assert(!this.rotating);
        this.rotating = true;
        this.rotateDir = rotateDir;
        this.rotateTimer = 0f;
        this.startRotation = this.cell.transform.rotation;
        this.targetRotation = this.startRotation * Quaternion.Euler(0f, 0f, rotateDir == RotateDir.CW ? -90f : 90f);
        this.onRotateFinish = onFinish;

        CellData cellData = this.cell.game.gameData.boardData.At(this.cell.x, this.cell.y);
        this.overrideShape = cellData.shape;

        cellData.forbidLink = true;
        cellData.shape = rotateDir == RotateDir.CW
            ? cellData.shape.GetSettings().rotateCW
            : cellData.shape.GetSettings().rotateCCW;
    }

    public override void MyUpdate(float dt)
    {
        if (this.rotating)
        {
            this.rotateTimer += dt;
            float t = Mathf.Clamp01(this.rotateTimer / 0.2f);
            this.cell.transform.rotation = Quaternion.Lerp(this.startRotation, this.targetRotation, t);
            if (t >= 1f)
            {
                this.FinishRotate();
            }
        }
    }

    public void FinishRotate()
    {
        Debug.Assert(this.rotating);
        this.rotating = false;

        CellData cellData = this.cell.game.gameData.boardData.At(this.cell.x, this.cell.y);
        cellData.forbidLink = false;

        this.cell.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        this.cell.Idle();

        // this.cell.Refresh();

        this.onRotateFinish?.Invoke(this.cell, this.rotateDir);
    }
}
