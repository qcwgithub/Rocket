using System;
using UnityEngine;

public class CellStateMove : CellState
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


    public bool moving;
    public float targetPositionY;
    public Action<Cell> onMoveFinish;
    public void Move(float targetPositionY, Action<Cell> onFinish)
    {
        this.moving = true;
        this.targetPositionY = targetPositionY;
        this.onMoveFinish = onFinish;
    }

    public override void MyUpdate(float dt)
    {
        if (this.moving)
        {
            Vector3 position = this.cell.transform.position;
            position.y -= 2f * dt;
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
        this.cell.Idle();
        this.onMoveFinish?.Invoke(this.cell);
    }
}

