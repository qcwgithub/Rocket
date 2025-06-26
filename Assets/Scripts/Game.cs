using UnityEngine;

public class Game : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BoardData boardData = sc.board.boardData;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log(mousePos);

            float x = mousePos.x;
            float y = mousePos.y;
            if (x > -boardData.width * 0.5f && x < boardData.width * 0.5f)
            {
                if (y > -boardData.height * 0.5f && y < boardData.height * 0.5f)
                {
                    int i = (int)(x - -boardData.width * 0.5f);
                    int j = (int)(y - -boardData.height * 0.5f);

                    sc.board.OnClick(i, j, ClickAction.RotateCCW);
                    // Debug.Log($"({i},{j})");
                    // CellData cell = boardData.At(i, j);
                    // cell.shape = cell.shape.GetSettings().rotateCCW;

                    // sc.board.At(i, j).ApplyShape();
                    // sc.board.RefreshColors();
                }
            }
        }
    }
}