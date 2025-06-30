
using System;
using UnityEngine;

public class CellStateFire : CellState
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

    public override void MyUpdate(float dt)
    {
        if (this.firing)
        {
            this.fireTimer += dt;
            float t = Mathf.Clamp01(this.fireTimer / 0.2f);

            this.cell.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            if (t >= 1f)
            {
                this.FinishFire();
            }
        }
    }

    public void FinishFire()
    {
        Debug.Assert(this.firing);
        this.firing = false;
        this.cell.transform.localScale = Vector3.one;
        this.cell.Idle();
        this.onFireFinish?.Invoke(this.cell);
    }
}
