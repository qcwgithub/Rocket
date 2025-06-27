using System.Collections.Generic;
using UnityEngine;

public enum Shape
{
    Empty,
    L,
    R,
    T,
    B,
    LR,
    LT,
    LB,
    RT,
    RB,
    TB,
    LRT,
    LRB,
    LTB,
    RTB,
    LRTB,

    Count,
}

public class ShapeSettings
{
    public List<Vector2Int> linkedOffsets;
    public bool linkedL;
    public bool linkedR;
    public bool linkedT;
    public bool linkedB;
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
        ShapeSettings settings = new ShapeSettings();
        settings.linkedOffsets = new List<Vector2Int>();

        if (settings.linkedL = e.Linked_L())
        {
            settings.linkedOffsets.Add(Vector2Int.left);
        }
        if (settings.linkedR = e.Linked_R())
        {
            settings.linkedOffsets.Add(Vector2Int.right);
        }
        if (settings.linkedT = e.Linked_T())
        {
            settings.linkedOffsets.Add(Vector2Int.up);
        }
        if (settings.linkedB = e.Linked_B())
        {
            settings.linkedOffsets.Add(Vector2Int.down);
        }
        settings.rotateCW = e.RotateCW();
        settings.rotateCCW = e.RotateCCW();
        return settings;
    }

    static bool Linked_L(this Shape e)
    {
        switch (e)
        {
            case Shape.Empty:
                return false;
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
            case Shape.Empty:
                return false;
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

    static bool Linked_T(this Shape e)
    {
        switch (e)
        {
            case Shape.Empty:
                return false;
            case Shape.L:
                return false;
            case Shape.R:
                return false;
            case Shape.T:
                return true;
            case Shape.B:
                return false;
            case Shape.LR:
                return false;
            case Shape.LT:
                return true;
            case Shape.LB:
                return false;
            case Shape.RT:
                return true;
            case Shape.RB:
                return false;
            case Shape.TB:
                return true;
            case Shape.LRT:
                return true;
            case Shape.LRB:
                return false;
            case Shape.LTB:
                return true;
            case Shape.RTB:
                return true;
            case Shape.LRTB:
            default:
                return true;
        }
    }

    static bool Linked_B(this Shape e)
    {
        switch (e)
        {
            case Shape.Empty:
                return false;
            case Shape.L:
                return false;
            case Shape.R:
                return false;
            case Shape.T:
                return false;
            case Shape.B:
                return true;
            case Shape.LR:
                return false;
            case Shape.LT:
                return false;
            case Shape.LB:
                return true;
            case Shape.RT:
                return false;
            case Shape.RB:
                return true;
            case Shape.TB:
                return true;
            case Shape.LRT:
                return false;
            case Shape.LRB:
                return true;
            case Shape.LTB:
                return true;
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
            case Shape.Empty:
                return Shape.Empty;
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
            case Shape.Empty:
                return Shape.Empty;
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