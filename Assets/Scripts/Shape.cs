using UnityEngine;

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

    Count,
}

public class ShapeSettings
{
    public Vector2Int[] linkedOffsets;
    public bool linkedL;
    public bool linkedR;
    public Shape rotateCW;
    public Shape rotateCCW;
}

public static class ShapeExt
{
    static ShapeSettings[] s_settings;
    public static ShapeSettings GetSettings(this Shape e)
    {
        if (s_settings == null)
        {
            s_settings = new ShapeSettings[(int)Shape.Count];
            for (int i = 0; i < (int)Shape.Count; i++)
            {
                s_settings[i] = CreateSettings((Shape)i);
            }
        }
        return s_settings[(int)e];
    }

    static ShapeSettings CreateSettings(this Shape e)
    {
        switch (e)
        {
            case Shape.L:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.left,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.R:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.right,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.T:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.up,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.B:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.down,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.LR:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.left,
                        Vector2Int.right,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.LT:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.left,
                        Vector2Int.up,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.LB:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.left,
                        Vector2Int.down,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.RT:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.right,
                        Vector2Int.up,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.RB:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.right,
                        Vector2Int.down,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.TB:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.up,
                        Vector2Int.down,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.LRT:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.left,
                        Vector2Int.right,
                        Vector2Int.up,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.LRB:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.left,
                        Vector2Int.right,
                        Vector2Int.down,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.LTB:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.left,
                        Vector2Int.up,
                        Vector2Int.down,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.RTB:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.right,
                        Vector2Int.up,
                        Vector2Int.down,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
            case Shape.LRTB:
            default:
                return new ShapeSettings
                {
                    linkedOffsets = new Vector2Int[]
                    {
                        Vector2Int.left,
                        Vector2Int.right,
                        Vector2Int.up,
                        Vector2Int.down,
                    },
                    linkedL = e.Linked_L(),
                    linkedR = e.Linked_R(),
                    rotateCW = e.RotateCW(),
                    rotateCCW = e.RotateCCW(),
                };
        }
    }

    static bool Linked_L(this Shape e)
    {
        switch (e)
        {
            case Shape.L:
                return true;
            case Shape.R:
                return false;
            case Shape.T:
                return false;
            case Shape.B:
                return false;
            case Shape.LR:
                return true;
            case Shape.LT:
                return true;
            case Shape.LB:
                return true;
            case Shape.RT:
                return false;
            case Shape.RB:
                return false;
            case Shape.TB:
                return false;
            case Shape.LRT:
                return true;
            case Shape.LRB:
                return true;
            case Shape.LTB:
                return true;
            case Shape.RTB:
                return false;
            case Shape.LRTB:
            default:
                return true;
        }
    }

    static bool Linked_R(this Shape e)
    {
        switch (e)
        {
            case Shape.L:
                return false;
            case Shape.R:
                return true;
            case Shape.T:
                return false;
            case Shape.B:
                return false;
            case Shape.LR:
                return true;
            case Shape.LT:
                return false;
            case Shape.LB:
                return false;
            case Shape.RT:
                return true;
            case Shape.RB:
                return true;
            case Shape.TB:
                return false;
            case Shape.LRT:
                return true;
            case Shape.LRB:
                return true;
            case Shape.LTB:
                return false;
            case Shape.RTB:
                return true;
            case Shape.LRTB:
            default:
                return true;
        }
    }

    static Shape RotateCW(this Shape e)
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

    static Shape RotateCCW(this Shape e)
    {
        switch (e)
        {
            case Shape.L:
                return Shape.B;
            case Shape.R:
                return Shape.T;
            case Shape.T:
                return Shape.L;
            case Shape.B:
                return Shape.R;
            case Shape.LR:
                return Shape.TB;
            case Shape.LT:
                return Shape.LB;
            case Shape.LB:
                return Shape.RB;
            case Shape.RT:
                return Shape.LT;
            case Shape.RB:
                return Shape.RT;
            case Shape.TB:
                return Shape.LR;
            case Shape.LRT:
                return Shape.LTB;
            case Shape.LRB:
                return Shape.RTB;
            case Shape.LTB:
                return Shape.LRB;
            case Shape.RTB:
                return Shape.LRT;
            case Shape.LRTB:
            default:
                return Shape.LRTB;
        }
    }
}