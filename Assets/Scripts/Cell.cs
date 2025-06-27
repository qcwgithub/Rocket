using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Cell : MonoBehaviour, ICell
{
    public SpriteRenderer spriteRenderer;
    public BoardData boardData;
    public int x { get; set; }
    public int y { get; set; }
    public bool yellow { get; set; }
    public bool red { get; set; }
    public Shape shape { get; set; }
    public CellState state;
    public void Init(BoardData boardData, int x, int y, CellState state)
    {
        this.boardData = boardData;
        this.x = x;
        this.y = y;
        this.state = state;
        // this.name = $"({cell.x},{cell.y}) {cell.shape}";
        this.ApplyName();

        this.ApplyShape();
    }

    void ApplyName()
    {
        this.name = $"({this.x},{this.y}) {this.boardData.At(this.x, this.y).shape}";
    }

    public void ApplyShape()
    {
        CellData cellData = this.boardData.At(this.x, this.y);
        this.spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/V2/" + cellData.shape);
    }

    public void ApplyColor()
    {
        CellData cellData = this.boardData.At(this.x, this.y);
        if (cellData.green)
        {
            this.spriteRenderer.color = Color.green;
        }
        else if (cellData.yellow)
        {
            this.spriteRenderer.color = Color.yellow;
        }
        else if (cellData.red)
        {
            this.spriteRenderer.color = Color.red;
        }
        else
        {
            this.spriteRenderer.color = Color.white;
        }
    }

    public void EnterState(CellState state)
    {
        switch (state)
        {
            case CellState.Falling:
                break;
            case CellState.Still:
                break;
            case CellState.Rotating:
                this.spriteRenderer.color = Color.white;
                break;
            case CellState.Warn:
                break;
            case CellState.Locked:
                break;
        }
    }

    bool rotating;
    float rotateTimer;
    Quaternion startRotation;
    Quaternion targetRotation;
    public void PlayRotateAnimation(string what)
    {
        this.rotating = true;
        this.rotateTimer = 0f;
        this.startRotation = this.transform.rotation;
        this.targetRotation = this.startRotation * Quaternion.Euler(0f, 0f, what == "cw" ? -90f : 90f);
    }

    void Update()
    {
        if (this.rotating)
        {
            this.rotateTimer += Time.deltaTime;
            float t = Mathf.Clamp01(this.rotateTimer / 0.2f);
            this.transform.rotation = Quaternion.Lerp(this.startRotation, this.targetRotation, t);
            if (t >= 1f)
            {
                this.rotating = false;
            }
        }
    }
}