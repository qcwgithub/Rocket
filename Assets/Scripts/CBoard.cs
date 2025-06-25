using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CBoard : MonoBehaviour
{
    public Board board;
    public CCell cellTemplate;
    // List<CCell> children = new List<CCell>();
    CCell[,] cells;

    public CCell At(int x, int y)
    {
        return this.cells[x, y];
    }

    public void Init(Board board)
    {
        this.board = board;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }

        while (this.transform.childCount < board.width * board.height)
        {
            CCell cell = Instantiate(this.cellTemplate);
            cell.transform.parent = this.transform;
        }

        this.cells = new CCell[board.width, board.height];

        int index = 0;
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                CCell cell = this.cells[i, j] = this.transform.GetChild(index++).GetComponent<CCell>();
                cell.gameObject.SetActive(true);
                // cell.gameObject.name = $"({i},{j})";
                cell.Init(board.At(i, j));

                cell.transform.position = new Vector3(
                    -board.width * 0.5f + 0.5f + i,
                    -board.height * 0.5f + 0.5f + j,
                    0f
                );
            }
        }
        this.RefreshColors();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log(mousePos);

            float x = mousePos.x;
            float y = mousePos.y;
            if (x > -this.board.width * 0.5f && x < this.board.width * 0.5f)
            {
                if (y > -this.board.height * 0.5f && y < this.board.height * 0.5f)
                {
                    int i = (int)(x - -this.board.width * 0.5f);
                    int j = (int)(y - -this.board.height * 0.5f);
                    // Debug.Log($"({i},{j})");
                    Cell cell = this.board.At(i, j);
                    cell.shape = cell.shape.GetSettings().rotateCCW;
                    this.At(i, j).ApplyShape();
                    this.RefreshColors();
                }
            }
        }
    }

    void RefreshColors()
    {
        Alg.RefreshColor(this.board);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                this.At(i, j).ApplyColor();
            }
        }
    }
}