using UnityEngine;

public static class Alg
{
    public static void RefreshColor(IBoard board)
    {
        // reset
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                ICell cell = board.At(i, j);
                cell.yellow = false;
                cell.red = false;
            }
        }

        // init yellow
        for (int j = 0; j < board.height; j++)
        {
            ICell cell = board.At(0, j);
            if (cell.shape.GetSettings().linkedL)
            {
                cell.yellow = true;
            }
        }
        for (int j = 0; j < board.height; j++)
        {
            ICell cell = board.At(0, j);
            if (cell.yellow)
            {
                Propagate(board, 0, j, true);
            }
        }

        // init red
        for (int j = 0; j < board.height; j++)
        {
            ICell cell = board.At(board.width - 1, j);
            if (cell.shape.GetSettings().linkedR)
            {
                cell.red = true;
            }
        }
        for (int j = 0; j < board.height; j++)
        {
            ICell cell = board.At(board.width - 1, j);
            if (cell.red)
            {
                Propagate(board, board.width - 1, j, false);
            }
        }
    }

    static void Propagate(IBoard board, int center_x, int center_y, bool isYellow)
    {
        ICell center = board.At(center_x, center_y);
        foreach (Vector2Int offset in center.shape.GetSettings().linkedOffsets)
        {
            int x = center_x + offset.x;
            if (x < 0 || x >= board.width)
            {
                continue;
            }

            int y = center_y + offset.y;
            if (y < 0 || y >= board.height)
            {
                continue;
            }

            ICell cell = board.At(x, y);
            if (isYellow)
            {
                if (cell.yellow)
                {
                    continue;
                }

                foreach (Vector2Int offset2 in cell.shape.GetSettings().linkedOffsets)
                {
                    if (offset2 == -offset)
                    {
                        // UnityEngine.Debug.Log($"{center_x},{center_y}->{x} {y}");
                        cell.yellow = true;
                        Propagate(board, x, y, isYellow);

                        break;
                    }
                }
            }
            else
            {
                if (cell.red)
                {
                    continue;
                }

                foreach (Vector2Int offset2 in cell.shape.GetSettings().linkedOffsets)
                {
                    if (offset2 == -offset)
                    {
                        // UnityEngine.Debug.Log($"{center_x},{center_y}->{x} {y}");
                        cell.red = true;
                        Propagate(board, x, y, isYellow);

                        break;
                    }
                }
            }
        }
    }
}