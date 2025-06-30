using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveGroup
{
    Game game;
    public void Init(Game game)
    {
        this.game = game;
    }

    Action<Cell> onCellMoveFinish;
    public void Move(List<Vector2Int> poses, Action<Cell> onCellMoveFinish)
    {
        // Debug.Log("OnFireFinish");
        this.onCellMoveFinish = onCellMoveFinish;

        // poses.Sort((a, b) => a.y - b.y); // y 小的在前

        Board board = this.game.board;
        BoardData boardData = this.game.gameData.boardData;

        // var frees = new List<CellData>();
        // foreach (Vector2Int pos in poses)
        // {
        //     frees.Add(boardData.Take(pos.x, pos.y));
        // }

        // int freeIndex = 0;

        var emptyY = new HashSet<int>();
        for (int i = 0; i < boardData.width; i++)
        {
            emptyY.Clear();
            foreach (Vector2Int pos in poses)
            {
                if (pos.x == i)
                {
                    emptyY.Add(pos.y);
                }
            }

            float topY = 3f;

            for (int j = 0; j < boardData.height; j++)
            {
                if (!emptyY.Contains(j))
                {
                    continue;
                }

                bool found = false;
                int j2;
                for (j2 = j + 1; j2 < boardData.height; j2++)
                {
                    if (!emptyY.Contains(j2))
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    boardData.Swap(i, j, i, j2);
                    board.Swap(i, j, i, j2);
                    emptyY.Remove(j);
                    emptyY.Add(j2);

                    Cell cell = board.At(i, j);
                    cell.Move(cell.transform.position.y, board.GetPosition(i, j).y, OnCellMoveFinish);
                }
                else
                {
                    CellData cellData = boardData.At(i, j);
                    cellData.shape = this.game.gameData.RandomShape();

                    Cell cell = board.At(i, j);
                    cell.Move(topY, board.GetPosition(i, j).y, this.OnCellMoveFinish);
                    topY += 1.3f;
                }
            }
        }
    }

    void OnCellMoveFinish(Cell cell)
    {
        this.onCellMoveFinish?.Invoke(cell);
    }
}