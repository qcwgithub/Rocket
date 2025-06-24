public enum Shape
{
    L,      // 0
    R,      // 1
    T,      // 2
    B,      // 3
    LR,     // 4
    LT,     // 5
    LB,     // 6
    RT,     // 7
    RB,     // 8
    TB,     // 9
    LRT,    // 10
    LRB,    // 11
    LTB,    // 12
    RTB,    // 13
    LRTB,   // 14
}

public static class ShapeExt
{
    // Clockwise
    public static Shape Rotate(this Shape e)
    {
        switch (e)
        {
            case Shape.L:
                return Shape.T;
            case Shape.R:
                return Shape.B;
            case Shape.T:
                return Shape.R;
            case Shape.B:
                return Shape.L;
            case Shape.LR:
                return Shape.TB;
            case Shape.LT:
                return Shape.RT;
            case Shape.LB:
                return Shape.LT;
            case Shape.RT:
                return Shape.RB;
            case Shape.RB:
                return Shape.LB;
            case Shape.TB:
                return Shape.LR;
            case Shape.LRT:
                return Shape.RTB;
            case Shape.LRB:
                return Shape.LTB;
            case Shape.LTB:
                return Shape.LRT;
            case Shape.RTB:
                return Shape.LRB;
            case Shape.LRTB:
            default:
                return Shape.LRTB;
        }
    }
}