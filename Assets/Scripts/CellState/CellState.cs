public abstract class CellState
{
    public Cell cell;
    public virtual void Init(Cell cell)
    {
        this.cell = cell;
    }

    public abstract bool AskRotate();
    public abstract bool OverrideSpriteShape(out Shape shape);
    public abstract void MyUpdate(float dt);
}