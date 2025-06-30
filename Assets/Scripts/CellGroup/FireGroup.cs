using System;
using System.Collections.Generic;
using UnityEngine;
public class FireGroup
{
    Game game;
    public void Init(Game game)
    {
        this.game = game;
        this.firing = false;
    }
    public bool firing;
    public List<Vector2Int> poses = new List<Vector2Int>();
    public Action<List<Vector2Int>> onFinish;
    public void Start(List<Vector2Int> poses, Action<List<Vector2Int>> onFinish)
    {
        Debug.Assert(!this.firing);

        if (!this.firing)
        {
            this.firing = true;
            this.poses.Clear();
            this.poses.AddRange(poses);
            this.onFinish = onFinish;

            for (int i = 0; i < this.poses.Count; i++)
            {
                Vector2Int pos = this.poses[i];
                Cell cell = this.game.board.At(pos.x, pos.y);
                cell.Fire(this.OnCellFireFinish);
            }
        }
    }

    void OnCellFireFinish(Cell _cell)
    {
        Debug.Assert(this.firing);
        if (this.firing)
        {
            for (int i = 0; i < this.poses.Count; i++)
            {
                Vector2Int pos = this.poses[i];
                Cell cell = this.game.board.At(pos.x, pos.y);
                if (cell.firing)
                {
                    return;
                }
            }

            this.firing = false;
            this.onFinish?.Invoke(this.poses);
        }
    }

    // public void Cancel()
    // {
    //     Debug.Assert(this.firing);
    //     if (this.firing)
    //     {
    //         this.firing = false;
    //     }
    // }
}