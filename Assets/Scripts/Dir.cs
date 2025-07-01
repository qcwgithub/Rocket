using UnityEngine;

public enum Dir
{
    L,
    R,
    T,
    B,

    Count,
}

public static class DirExt
{
    public static Dir Reverse(this Dir e)
    {
        switch (e)
        {
            case Dir.L:
                return Dir.R;
            case Dir.R:
                return Dir.L;
            case Dir.T:
                return Dir.B;
            case Dir.B:
            default:
                return Dir.T;
        }
    }
    public static Dir FromOffset(Vector2Int offset)
    {
        int x = offset.x;
        int y = offset.y;

        Debug.Assert(x != 0 || y != 0);
        Debug.Assert(x == 0 || y == 0);

        if (y == 0)
        {
            return x < 0 ? Dir.L : Dir.R;
        }
        else
        {
            return y > 0 ? Dir.T : Dir.B;
        }
    }

    public static Vector2Int ToOffset(this Dir e)
    {
        switch (e)
        {
            case Dir.L:
                return Vector2Int.left;
            case Dir.R:
                return Vector2Int.right;
            case Dir.T:
                return Vector2Int.up;
            case Dir.B:
            default:
                return Vector2Int.down;
        }
    }
}