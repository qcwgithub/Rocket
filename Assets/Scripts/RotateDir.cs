public enum RotateDir
{
    CW,
    CCW,
    CW2,
    CCW2,

    Count,
}

public static class RotateDirExt
{
    public static float ToRotateAngle(this RotateDir e)
    {
        switch (e)
        {
            case RotateDir.CW:
                return -90f;
            case RotateDir.CCW:
                return 90f;
            case RotateDir.CW2:
                return -180f;
            case RotateDir.CCW2:
            default:
                return 180f;
        }
    }
}