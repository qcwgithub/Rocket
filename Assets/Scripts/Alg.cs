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
        board.previewGroupDatas.Clear();

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

                var group = new PreviewGroupData();
                group.poses.Add(new Vector2Int(i, j));
                board.previewGroupDatas.Add(group);

                Propagate(board, i, j, "LR");
            }
        }

        //
#if UNITY_EDITOR
        for (int i = 0; i < board.previewGroupDatas.Count; i++)
        {
            var sb = new StringBuilder();
            sb.Append($"[{i}]");
            foreach (var p in board.previewGroupDatas[i].poses)
            {
                sb.Append($"({p.x},{p.y}) ");
            }
            UnityEngine.Debug.Log(sb);
        }
#endif
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

                if (cell.shape.GetSettings().linkedOffsets.Contains(-offset))
                {
                    // UnityEngine.Debug.Log($"{center_x},{center_y}->{x} {y}");
                    cell.linkedL = true;
                    Propagate(board, x, y, what);
                }
            }
            else if (what == "R")
            {
                if (cell.linkedR)
                {
                    continue;
                }

                if (cell.shape.GetSettings().linkedOffsets.Contains(-offset))
                {
                    // UnityEngine.Debug.Log($"{center_x},{center_y}->{x} {y}");
                    cell.linkedR = true;
                    Propagate(board, x, y, what);
                }
            }
            else if (what == "LR")
            {
                if (!cell.linkedLR || cell.linkedLRHandled)
                {
                    continue;
                }

                if (cell.shape.GetSettings().linkedOffsets.Contains(-offset))
                {
                    cell.linkedLRHandled = true;

                    board.currentPreviewGroupData.poses.Add(new Vector2Int(x, y));
                    Propagate(board, x, y, what);
                }
            }
        }
    }
}