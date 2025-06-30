using System.Collections.Generic;
using UnityEngine;

public class MoveGroup
{
    Game game;
    public void Init(Game game)
    {
        this.game = game;
    }

    public void OnFireFinish(List<Vector2Int> poses)
    {
        // Debug.Log("OnFireFinish");

        BoardData boardData = this.game.gameData.boardData;

        var frees = new List<CellData>();
        foreach (Vector2Int pos in poses)
        {
            frees.Add(boardData.Take(pos.x, pos.y));
        }

        int freeIndex = 0;

        for (int i = 0; i < boardData.width; i++)
        {
            for (int j = 0; j < boardData.height; j++)
            {
                if (boardData.At(i, j) == null)
                {
                    bool found = false;
                    for (int j2 = j + 1; j2 < boardData.height; j2++)
                    {
                        // (i, j2) -> (i, j)
                        if (boardData.At(i, j2) != null)
                        {
                            boardData.Put(i, j, boardData.Take(i, j2));
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Debug.Assert(frees.Count > 0);
                        CellData free = frees[frees.Count - 1];
                        frees.RemoveAt(frees.Count - 1);
                        boardData.Put(i, j, free);
                    }
                }
            }
        }
    }
}