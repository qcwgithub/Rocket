public class CellStateIdle : CellState
{
    public override bool AskRotate()
    {
        return true;
    }

    public override bool OverrideSpriteShape(out Shape shape)
    {
        shape = default;
        return false;
    }

    public override void MyUpdate(float dt)
    {
        
    }
}