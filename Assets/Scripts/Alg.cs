using UnityEngine;

public static class Alg
{
    public static void RefreshColor(BoardData boardData)
    {
        // reset
        for (int i = 0; i < boardData.width; i++)
        {
            for (int j = 0; j < boardData.height; j++)
            {
                CellData cell = boardData.At(i, j);
                cell.yellow = false;
                cell.red = false;
            }
        }

        // init yellow
        for (int j = 0; j < boardData.height; j++)
        {
            CellData cell = boardData.At(0, j);
            if (cell.shape.GetSettings().linkedL)
            {
                cell.yellow = true;
            }
        }
        for (int j = 0; j < boardData.height; j++)
        {
            CellData cell = boardData.At(0, j);
            if (cell.yellow)
            {
                Propagate(boardData, 0, j, true);
            }
        }

        // init red
        for (int j = 0; j < boardData.height; j++)
        {
            CellData cell = boardData.At(boardData.width - 1, j);
            if (cell.shape.GetSettings().linkedR)
            {
                cell.red = true;
            }
        }
        for (int j = 0; j < boardData.height; j++)
        {
            CellData cell = boardData.At(boardData.width - 1, j);
            if (cell.red)
            {
                Propagate(boardData, boardData.width - 1, j, false);
            }
        }
    }

    static void Propagate(BoardData boardData, int center_x, int center_y, bool isYellow)
    {
        CellData center = boardData.At(center_x, center_y);
        foreach (Vector2Int offset in center.shape.GetSettings().linkedOffsets)
        {
            int x = center_x + offset.x;
            if (x < 0 || x >= boardData.width)
            {
                continue;
            }

            int y = center_y + offset.y;
            if (y < 0 || y >= boardData.height)
            {
                continue;
            }

            CellData cell = boardData.At(x, y);
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
                        Propagate(boardData, x, y, isYellow);

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
                        Propagate(boardData, x, y, isYellow);

                        break;
                    }
                }
            }
        }
    }
}