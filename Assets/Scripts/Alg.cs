using UnityEngine;

public static class Alg
{
    public static void RefreshLink(BoardData board)
    {
        // reset
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                CellData cell = board.At(i, j);
                cell.linkedL = false;
                cell.linkedR = false;
            }
        }

        // init yellow
        for (int j = 0; j < board.height; j++)
        {
            CellData cell = board.At(0, j);
            if (cell.forbidLink)
            {
                continue;
            }
            if (cell.shape.GetSettings().linkedL)
            {
                cell.linkedL = true;
            }
        }
        for (int j = 0; j < board.height; j++)
        {
            CellData cell = board.At(0, j);
            if (cell.linkedL)
            {
                Propagate(board, 0, j, true);
            }
        }

        // init red
        for (int j = 0; j < board.height; j++)
        {
            CellData cell = board.At(board.width - 1, j);
            if (cell.forbidLink)
            {
                continue;
            }
            if (cell.shape.GetSettings().linkedR)
            {
                cell.linkedR = true;
            }
        }
        for (int j = 0; j < board.height; j++)
        {
            CellData cell = board.At(board.width - 1, j);
            if (cell.linkedR)
            {
                Propagate(board, board.width - 1, j, false);
            }
        }
    }

    static void Propagate(BoardData board, int center_x, int center_y, bool isL)
    {
        CellData center = board.At(center_x, center_y);
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

            CellData cell = board.At(x, y);
            if (cell.forbidLink)
            {
                continue;
            }
            if (isL)
            {
                if (cell.linkedL)
                {
                    continue;
                }

                foreach (Vector2Int offset2 in cell.shape.GetSettings().linkedOffsets)
                {
                    if (offset2 == -offset)
                    {
                        // UnityEngine.Debug.Log($"{center_x},{center_y}->{x} {y}");
                        cell.linkedL = true;
                        Propagate(board, x, y, isL);

                        break;
                    }
                }
            }
            else
            {
                if (cell.linkedR)
                {
                    continue;
                }

                foreach (Vector2Int offset2 in cell.shape.GetSettings().linkedOffsets)
                {
                    if (offset2 == -offset)
                    {
                        // UnityEngine.Debug.Log($"{center_x},{center_y}->{x} {y}");
                        cell.linkedR = true;
                        Propagate(board, x, y, isL);

                        break;
                    }
                }
            }
        }
    }
}