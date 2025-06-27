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
                cell.linkedLRHandled = false;

            }
        }
        board.linkedLRList.Clear();

        // init L
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
                Propagate(board, 0, j, "linkedL");
            }
        }

        // init R
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
                Propagate(board, board.width - 1, j, "linkedR");
            }
        }

        for (int j = board.height - 1; j >= 0; j--)
        {
            for (int i = 0; i < board.width; i++)
            {
                CellData cellData = board.At(i, j);
                if (cellData.forbidLink || !cellData.linkedLR || cellData.linkedLRHandled)
                {
                    continue;
                }

                Propagate(board, i, j, "linkedLR");
            }
        }
    }

    static void Propagate_linkedL(BoardData board, int center_x, int center_y, string what)
    {

    }

    static void Propagate(BoardData board, int center_x, int center_y, string what)
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
            if (what == "linkedL")
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
                        Propagate(board, x, y, what);

                        break;
                    }
                }
            }
            else if (what == "linkedR")
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
                        Propagate(board, x, y, what);

                        break;
                    }
                }
            }
            else if (what == "linkedLR")
            {
                if (!cell.linkedLR)
                {
                    continue;
                }
                
            }
        }
    }
}