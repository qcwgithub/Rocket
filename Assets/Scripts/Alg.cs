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
                Propagate(boardData, cell, true);
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
                Propagate(boardData, cell, false);
            }
        }
    }

    static void Propagate(BoardData boardData, CellData center, bool isYellow)
    {
        foreach (Vector2Int offset in center.shape.GetSettings().linkedOffsets)
        {
            int x = center.x + offset.x;
            if (x < 0 || x >= boardData.width)
            {
                continue;
            }

            int y = center.y + offset.y;
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
                        UnityEngine.Debug.Log($"{center.x},{center.y}->{cell.x} {cell.y}");
                        cell.yellow = true;
                        Propagate(boardData, cell, isYellow);

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
                        Propagate(boardData, cell, isYellow);

                        break;
                    }
                }
            }
        }
    }
}