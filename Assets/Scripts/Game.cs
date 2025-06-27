using UnityEngine;

public class Game : MonoBehaviour
{
    public Board board;
    public GameData gameData;
    public void Init(GameData gameData)
    {
        this.gameData = gameData;
        this.board.Init(gameData.boardData);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BoardData boardData = this.board.boardData;

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

                    // this.board.OnClick(i, j, ClickAction.RotateCCW);
                    this.OnClick(i, j, ClickAction.RotateCCW);
                    // Debug.Log($"({i},{j})");
                    // CellData cell = boardData.At(i, j);
                    // cell.shape = cell.shape.GetSettings().rotateCCW;

                    // sc.board.At(i, j).ApplyShape();
                    // sc.board.RefreshColors();
                }
            }
        }
    }

    void OnClick(int i, int j, ClickAction action)
    {
        Cell cell = this.board.At(i, j);
        if (cell.state == CellState.Still || cell.state == CellState.Warn)
        {
            // logic
            CellData cellData = this.board.boardData.At(i, j);
            this.gameData.SetShape(i, j, cellData.shape.GetSettings().rotateCCW);

            cell.PlayRotateAnimation("ccw");

            //
            cell.ApplyShape();
            cell.ApplyColor();
        }
    }
}