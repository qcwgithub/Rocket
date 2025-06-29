using System;
using System.Collections.Generic;
using UnityEngine;

public class PreviewGroup
{
    Game game;
    public void Init(Game game)
    {
        this.game = game;
        this.previewing = false;
    }

    public bool previewing;
    public List<Vector2Int> poses = new List<Vector2Int>();
    Action<List<Vector2Int>> onFinish;
    public void Start(PreviewGroupData previewGroupData, Action<List<Vector2Int>> onFinish)
    {
        Debug.Assert(!this.previewing);
        if (!this.previewing)
        {
            this.previewing = true;
            this.poses.Clear();
            this.poses.AddRange(previewGroupData.poses);
            this.onFinish = onFinish;

            for (int i = 0; i < this.poses.Count; i++)
            {
                Vector2Int pos = this.poses[i];
                Cell cell = this.game.board.At(pos.x, pos.y);
                cell.Preview(this.OnCellPreviewFinish);
            }
        }
    }

    void OnCellPreviewFinish(Cell _cell)
    {
        Debug.Assert(this.previewing);
        if (this.previewing)
        {
            for (int i = 0; i < this.poses.Count; i++)
            {
                Vector2Int pos = this.poses[i];
                Cell cell = this.game.board.At(pos.x, pos.y);
                if (cell.previewing)
                {
                    return;
                }
            }

            this.previewing = false;
            this.onFinish?.Invoke(this.poses);
        }
    }

    public void Cancel()
    {
        Debug.Assert(this.previewing);
        if (this.previewing)
        {
            this.previewing = false;

            for (int i = 0; i < this.poses.Count; i++)
            {
                Vector2Int pos = this.poses[i];
                Cell cell = this.game.board.At(pos.x, pos.y);
                if (cell.previewing)
                {
                    cell.CancelPreview();
                }
            }
        }
    }
}