using UnityEngine;

public static class Alg
{
    public static void RefreshColor(Board board)
    {
        // reset
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                Cell cell = board.At(i, j);
                cell.yellow = false;
                cell.red = false;
            }
        }

        // init yellow
        for (int j = 0; j < board.height; j++)
        {
            Cell cell = board.At(0, j);
            if (cell.shape.GetSettings().linkedL)
            {
                cell.yellow = true;
            }
        }
        for (int j = 0; j < board.height; j++)
        {
            Cell cell = board.At(0, j);
            if (cell.yellow)
            {
                Propagate(board, cell, true);
            }
        }

        // init red
        for (int j = 0; j < board.height; j++)
        {
            Cell cell = board.At(board.width - 1, j);
            if (cell.shape.GetSettings().linkedR)
            {
                cell.red = true;
            }
        }
        for (int j = 0; j < board.height; j++)
        {
            Cell cell = board.At(board.width - 1, j);
            if (cell.red)
            {
                Propagate(board, cell, false);
            }
        }
    }

    static void Propagate(Board board, Cell center, bool isYellow)
    {
        foreach (Vector2Int offset in center.shape.GetSettings().linkedOffsets)
        {
            int x = center.x + offset.x;
            if (x < 0 || x >= board.width)
            {
                continue;
            }

            int y = center.y + offset.y;
            if (y < 0 || y >= board.height)
            {
                continue;
            }

            Cell cell = board.At(x, y);
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
                        UnityEngine.Debug.Log($"{center.x},{center.y}->{cell.x} {cell.y}");
                        cell.yellow = true;
                        Propagate(board, cell, isYellow);

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
                        UnityEngine.Debug.Log($"{center.x},{center.y}->{cell.x} {cell.y}");
                        cell.red = true;
                        Propagate(board, cell, isYellow);

                        break;
                    }
                }
            }
        }
    }
}