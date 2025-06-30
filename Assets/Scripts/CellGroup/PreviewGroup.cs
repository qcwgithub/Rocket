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

    void OnCellPreviewFinish(Cell _cell)
    {
        Debug.Assert(this.previewing);
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

    public void Cancel()
    {
        Debug.LogWarning("PreviewGroup.Cancel");
        Debug.Assert(this.previewing);
        this.previewing = false;

        for (int i = 0; i < this.poses.Count; i++)
        {
            Vector2Int pos = this.poses[i];
            Cell cell = this.game.board.At(pos.x, pos.y);
            if (cell.previewing)
            {
                cell.statePreview.CancelPreview();
            }
        }
    }

    bool StillValid(List<PreviewGroupData> previewGroupDatas, out PreviewGroupData curr_previewGroupData)
    {
        curr_previewGroupData = null;
        if (previewGroupDatas.Count == 0)
        {
            return false;
        }

        var posesL = new List<Vector2Int>();
        var posesR = new List<Vector2Int>();
        foreach (Vector2Int pos in this.poses)
        {
            if (pos.x == 0)
            {
                posesL.Add(pos);
            }
            else if (pos.y == this.game.gameData.boardData.width - 1)
            {
                posesR.Add(pos);
            }
        }
        Debug.Assert(posesL.Count > 0);
        Debug.Assert(posesR.Count > 0);

        //
        foreach (PreviewGroupData previewGroupData in previewGroupDatas)
        {
            bool containsAnyL = false;
            foreach (Vector2Int pos in posesL)
            {
                if (previewGroupData.poses.Contains(pos))
                {
                    containsAnyL = true;
                    break;
                }
            }

            if (!containsAnyL)
            {
                continue;
            }

            bool containsAnyR = false;
            foreach (Vector2Int pos in posesR)
            {
                if (previewGroupData.poses.Contains(pos))
                {
                    containsAnyR = true;
                    break;
                }
            }

            if (!containsAnyR)
            {
                continue;
            }

            curr_previewGroupData = previewGroupData;
            break;
        }

        if (curr_previewGroupData == null)
        {
            return false;
        }
        return true;
    }

    public void UpdatePreview(List<PreviewGroupData> previewGroupDatas)
    {
        Debug.Assert(this.previewing);
        if (!this.previewing)
        {
            return;
        }

        if (!this.StillValid(previewGroupDatas, out PreviewGroupData curr_previewGroupData))
        {
            this.Cancel();
            return;
        }

        // -preview
        foreach (Vector2Int pos in this.poses)
        {
            if (!curr_previewGroupData.poses.Contains(pos))
            {
                Cell cell = this.game.board.At(pos.x, pos.y);
                if (cell.previewing)
                {
                    cell.statePreview.CancelPreview();
                }
            }
        }

        this.poses.Clear();
        this.poses.AddRange(curr_previewGroupData.poses);

        // +preview
        foreach (Vector2Int pos in this.poses)
        {
            Cell cell = this.game.board.At(pos.x, pos.y);
            if (!cell.previewing)
            {
                cell.Preview(this.OnCellPreviewFinish);
            }
        }
    }
}