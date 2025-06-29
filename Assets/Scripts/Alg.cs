using UnityEngine;
using System.Collections.Generic;
using System.Text;

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
        board.previewLists.Clear();

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
                Propagate(board, 0, j, "L");
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
                Propagate(board, board.width - 1, j, "R");
            }
        }

        for (int j = board.height - 1; j >= 0; j--)
        {
            for (int i = 0; i < board.width; i++)
            {
                CellData cellData = board.At(i, j);
                if (!cellData.linkedLR || cellData.linkedLRHandled)
                {
                    continue;
                }
                cellData.linkedLRHandled = true;

                List<Vector2Int> previewList = new List<Vector2Int>();
                previewList.Add(new Vector2Int(i, j));
                board.previewLists.Add(previewList);

                Propagate(board, i, j, "LR");
            }
        }

        //
        var sb = new StringBuilder();
        for (int i = 0; i < board.previewLists.Count; i++)
        {
            sb.Append($"[{i}]");
            foreach (var p in board.previewLists[i])
            {
                sb.Append($"({p.x},{p.y}) ");
            }
        }
        UnityEngine.Debug.Log(sb);
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
            if (what == "L")
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
            else if (what == "R")
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
            else if (what == "LR")
            {
                if (!cell.linkedLR||cell.linkedLRHandled)
                {
                    continue;
                }

                cell.linkedLRHandled = true;

                board.currentPreviewList.Add(new Vector2Int(x, y));
                Propagate(board, x, y, what);
            }
        }
    }
}