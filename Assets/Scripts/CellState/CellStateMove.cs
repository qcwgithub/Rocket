using System;
using UnityEngine;

public class CellStateMove : CellState
{
    public override bool AskRotate()
    {
        return false;
    }

    public override bool OverrideSpriteShape(out Shape shape)
    {
        shape = default;
        return false;
    }

    public bool moving;
    public float targetPositionY;
    public Action<Cell> onMoveFinish;

    public void Move(float fromPositionY, float toPositionY, Action<Cell> onFinish)
    {
        this.moving = true;

        Vector3 position = this.cell.transform.position;
        position.y = fromPositionY;
        this.cell.transform.position = position;

        this.targetPositionY = toPositionY;
        this.onMoveFinish = onFinish;

        CellData cellData = this.cell.game.gameData.boardData.At(this.cell.x, this.cell.y);
        cellData.forbidLink = true;
    }

    public override void MyUpdate(float dt)
    {
        if (this.moving)
        {
            Vector3 position = this.cell.transform.position;
            position.y -= 4f * dt;
            if (position.y <= this.targetPositionY)
            {
                position.y = this.targetPositionY;
            }
            this.cell.transform.position = position;
            if (position.y <= this.targetPositionY)
            {
                this.FinishMove();
            }
        }
    }

    public void FinishMove()
    {
        this.moving = false;
        CellData cellData = this.cell.game.gameData.boardData.At(this.cell.x, this.cell.y);
        cellData.forbidLink = false;
        this.cell.Idle();
        this.onMoveFinish?.Invoke(this.cell);
    }
}

