using System.Collections.Generic;
using UnityEngine;

public class MyInput
{
    Game game;
    public void Init(Game game)
    {
        this.game = game;
    }

    Vector2Int GetMousePos()
    {
        Board board = this.game.board;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float x = mousePos.x;
        float y = mousePos.y;

        int i = (int)(x - -board.width * 0.5f);
        int j = (int)(y - -board.height * 0.5f);
        return new Vector2Int(i, j);

        // if (x > -board.width * 0.5f && x < board.width * 0.5f)
        // {
        //     if (y > -board.height * 0.5f && y < board.height * 0.5f)
        //     {

        //         pos.x = i;
        //         pos.y = j;

        //         return true;
        //     }
        // }

        // return false;
    }

    void MyUpdate_R(float dt)
    {
        bool clickR = Input.GetMouseButtonDown(1);
        if (!clickR)
        {
            return;
        }

        Vector2Int pos = this.GetMousePos();
        if (this.game.board.boardData.InRange(pos))
        {
            this.game.OnClick(pos.x, pos.y, RotateDir.CW);
        }
    }

    List<Vector2Int> swipePoses = new List<Vector2Int>();
    List<Dir?> swipeDirs = new List<Dir?>();
    bool leftDown;
    void MyUpdate_L(float dt)
    {
        if (Input.GetMouseButtonUp(0))
        {
            this.leftDown = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            this.leftDown = true;
        }

        if (!this.leftDown)
        {
            this.swipePoses.Clear();
            this.swipeDirs.Clear();
            return;
        }

        Vector2Int pos = this.GetMousePos();
        bool inRange = this.game.board.boardData.InRange(pos);

        if (this.swipePoses.Count == 0)
        {
            if (inRange)
            {
                this.swipePoses.Add(pos);
                this.swipeDirs.Add(null);
            }
            return;
        }

        Vector2Int prevPos = this.swipePoses[this.swipePoses.Count - 1];
        if (prevPos == pos)
        {
            return;
        }

        Dir? prevDir = this.swipeDirs[this.swipeDirs.Count - 1];

        Dir dir = DirExt.FromOffset(pos - prevPos);
        this.game.OnSwipe(prevDir, prevPos, dir);

        if (inRange)
        {
            this.swipePoses.Add(pos);
            this.swipeDirs.Add(dir);
        }
        else
        {
            this.swipePoses.Clear();
            this.swipeDirs.Clear();
        }
    }

    public void MyUpdate(float dt)
    {
        this.MyUpdate_L(dt);
        this.MyUpdate_R(dt);
    }
}