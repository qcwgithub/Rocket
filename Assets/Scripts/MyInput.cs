using System.Collections.Generic;
using UnityEngine;

public class MyInput
{
    Game game;
    public void Init(Game game)
    {
        this.game = game;
    }

    List<Vector2Int> swipeHistory = new List<Vector2Int>();
    public void MyUpdate(float dt)
    {
        bool clickL = Input.GetMouseButtonDown(0);
        bool clickR = Input.GetMouseButtonDown(1);
        if (!clickL && !clickR)
        {
            this.swipeHistory.Clear();
            return;
        }

        Board board = this.game.board;

        bool inRange = false;
        Vector2Int pos = Vector2Int.zero;
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float x = mousePos.x;
            float y = mousePos.y;
            if (x > -board.width * 0.5f && x < board.width * 0.5f)
            {
                if (y > -board.height * 0.5f && y < board.height * 0.5f)
                {
                    int i = (int)(x - -board.width * 0.5f);
                    int j = (int)(y - -board.height * 0.5f);

                    inRange = true;
                    pos.x = i;
                    pos.y = j;
                }
            }
        }

        if (!inRange)
        {
            this.swipeHistory.Clear();
            return;
        }

        if (clickR)
        {
            this.game.OnClick(pos.x, pos.y, RotateDir.CW);
            return;
        }

        if (this.swipeHistory.Count == 0)
        {
            this.swipeHistory.Add(pos);
            return;
        }

        Vector2Int last = this.swipeHistory[this.swipeHistory.Count - 1];
        if (last == pos)
        {
            return;
        }

        Dir dir = DirExt.FromOffset(pos - last);
        this.game.OnSwipe(last.x, last.y, dir);

        this.swipeHistory.Add(pos);
    }
}