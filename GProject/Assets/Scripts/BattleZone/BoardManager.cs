using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    #region Tile Width and Offset
    private const float _TileWidth = 100.0f;

    public float TileWidth
    {
        get { return _TileWidth; }
    }

    private const float TILE_OFFSET = 0.5f;
    #endregion

    private class BoardTiles
    {
        public Vector3 Position;
        //public Figure Figure;

        GameObject boardTileObject;
        public BoardTiles() { }

        public BoardTiles(Vector3 Position, int typeOfTile = 0)
        {
            this.Position = Position;
            UnityEngine.Object tile = Resources.Load("Tile/" + (typeOfTile == 0 ? "Tile" : "BenchTile"));
            this.Position.y += 0.1f;
            boardTileObject = (GameObject)GameObject.Instantiate(tile, this.Position, Quaternion.identity);
            boardTileObject.transform.localScale = new Vector3(0.1f * BoxSize, 1, 0.1f * BoxSize);
            ChangeColor(BoardTiles.DefaultColor);
        }

        public static Color DefaultColor = Color.grey;
        public void ChangeColor(Color color)
        {
            this.boardTileObject.GetComponent<Renderer>().material.color = color;
        }

        private static BoardTiles[,] _boardTiles = new BoardTiles[9, 8];

        public BoardTiles this[int i, int j]
        {
            get { return _boardTiles[i + 1, j]; }
            set { _boardTiles[i + 1, j] = value; }
        }
    }
    private BoardTiles boardTiles;
    private BoardTiles _lastBoardTilePassed;

    public class FigureSet
    {
        private Figure[,] _figures = new Figure[9, 8];

        public Figure this[int i, int j]
        {
            get { return _figures[i + 1, j]; }
            set { _figures[i + 1, j] = value; }
        }
    }

    public string Owner { get; }

    [SerializeField]
    private static int BoxSize = 10;

    private int selectionRow = -1;
    private int selectionColumn = -1;

    public FigureSet Figures { get; set; }
    private Figure _selectedFigure;

    public List<GameObject> ChessFigurePrefabs;
    private List<GameObject> _activeChessFigures;

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    public void SpawnFigure(GameObject unit)
    {
        for (int i = 0; i < 8; ++i)
        {
            if (Figures[-1, i] == null)
            {
                GameObject FigureOnBoard = FigureManager.CreateFigure(unit);
                FigureOnBoard.GetComponent<Figure>().Untargetable = true;
                FigureOnBoard.GetComponent<Figure>().Owner = "a"+i;   // To-Do: change owner to board.owner
                FigureOnBoard.GetComponent<Figure>().OnDeath += f => Figures[f.Position.Row, f.Position.Column] = null;
                FigureOnBoard.GetComponent<Figure>().OnMove += (f, nextRow, nextColumn) =>
                {
                    Figures[f.Position.Row, f.Position.Column] = null;
                    Figures[nextRow, nextColumn] = f;
                    f.gameObject.transform.position = GetTileCenter(nextRow, nextColumn);
                };

                ChessFigurePrefabs.Add(FigureOnBoard);
                SpawnChessFigure(ChessFigurePrefabs.Count - 1, -1, i);
                break;
            }
        }
    }

    public void SellFigure(GameObject figure)
    {
        _activeChessFigures.Remove(figure);
        Figures[figure.GetComponent<Figure>().Position.Row, figure.GetComponent<Figure>().Position.Column] = null;
        UnitShop.SellUnit(figure);
    }

    public void PrepareForBattle()
    {
        foreach (GameObject figurePrefab in ChessFigurePrefabs)
        {
            Figure figure = figurePrefab.GetComponent<Figure>();
            figure.PrepareForBattle();
        }
    }

    public void RecoverFromBattle()
    {
        foreach (GameObject figurePrefab in ChessFigurePrefabs)
        {
            Figure figure = figurePrefab.GetComponent<Figure>();
            figure.Restart();
        }
        SpawnAllChessFigures();
    }

    private void SpawnChessFigure(int index, int row, int column)
    {
        // quarterion - for orientation, change if needed (default Quaternion.identity)
        Figures[row, column] = ChessFigurePrefabs[index].GetComponent<Figure>();
        ChessFigurePrefabs[index].transform.position = GetTileCenter(row, column);
        Figures[row, column].Position.Row = row;
        Figures[row, column].Position.Column = column;
        _activeChessFigures.Add(ChessFigurePrefabs[index]);
    }

    private void SpawnAllChessFigures()
    {
        int i = 0;
        foreach(GameObject figurePrefab in ChessFigurePrefabs)
        {
            figurePrefab.SetActive(true);
            Figure figure = figurePrefab.GetComponent<Figure>();
            SpawnChessFigure(i++, figure.Position.Row, figure.Position.Column);
        }
    }
    
    private void Start()
    {
        boardTiles = new BoardTiles();
        SetUpTheTiles();

        Figures = new FigureSet();

        _activeChessFigures = new List<GameObject>();

        Dijkstra.SetGraph(Figures);

        MatchManager.Instance.OnStateChage += matchState =>
        {
            if (matchState == Enums.MatchState.Preparation)
                RecoverFromBattle();
            if (matchState == Enums.MatchState.Battle)
                PrepareForBattle();
        };
    }

    private void Update()
    {
        DrawChessBoard();
        UpdateSelection();

        if (MatchManager.Instance.MatchState != Enums.MatchState.Preparation)
            return;

        if (Input.GetKeyDown("up"))
        {

        }

        if (Input.GetKeyDown("down"))
        {

        }

        FigurePlacement();
    }

    private void FigurePlacement()
    {
        SelectFigureCommand();
        MoveFigureWhileSelected();
        PlaceFigure();
    }

    private void SelectFigureCommand()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionColumn >= 0 && selectionRow >= -1)
            {
                if (_selectedFigure == null)
                {
                    SelectFigure(selectionRow, selectionColumn);
                    _lastBoardTilePassed = boardTiles[selectionRow, selectionColumn];
                }
            }
        }
    }

    private void MoveFigureWhileSelected()
    {
        if (Input.GetMouseButton(0))
        {
            if (selectionColumn >= 0 && selectionRow >= -1)
            {
                if (_selectedFigure != null)
                {
                    if (selectionRow > 3)
                    {
                        selectionRow = 3;
                    }
                    boardTiles[selectionRow, selectionColumn].ChangeColor(Color.green);
                    _selectedFigure.transform.position = GetTileCenter(selectionRow, selectionColumn);
                }
                if (boardTiles[selectionRow, selectionColumn] != _lastBoardTilePassed)
                {
                    _lastBoardTilePassed.ChangeColor(BoardTiles.DefaultColor);
                    _lastBoardTilePassed = boardTiles[selectionRow, selectionColumn];
                }
            }
        }
    }

    private void PlaceFigure()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (selectionColumn >= 0 && selectionRow >= -1)
            {
                if (selectionRow > 3)
                {
                    selectionRow = 3;
                }
                if (_selectedFigure != null)
                    MoveFigure(selectionRow, selectionColumn);
            }
        }
    }

    private void SelectFigure(int row, int column)
    {
        if (Figures[row, column] == null)
            return;
        //if(figure is allowed to be moved)    -if it's a phase for moving pieces, ...
        //...
        //if (Figures[x, y].Owner != this.Owner)
        //    return;

        _selectedFigure = Figures[row, column];
    }

    private void MoveFigure(int row, int column)
    {
        if (IsPositionAllowed(row, column))
        {
            Figures[_selectedFigure.Position.Row, _selectedFigure.Position.Column] = null;

            if (Figures[row, column] != null)
            {
                Figure figureToSwap = Figures[row, column];
                figureToSwap.transform.position = GetTileCenter(_selectedFigure.Position.Row, _selectedFigure.Position.Column);
                Figures[_selectedFigure.Position.Row, _selectedFigure.Position.Column] = figureToSwap;

                figureToSwap.Position.Row = _selectedFigure.Position.Row;
                figureToSwap.Position.Column = _selectedFigure.Position.Column;
                figureToSwap.Untargetable = row == -1;
            }

            Figures[row, column] = _selectedFigure;
            _selectedFigure.transform.position = GetTileCenter(row, column);
            _selectedFigure.Position.Row = row;
            _selectedFigure.Position.Column = column;
            _selectedFigure.Untargetable = row == -1;

            boardTiles[selectionRow, selectionColumn].ChangeColor(BoardTiles.DefaultColor);
        }

        _selectedFigure = null;
    }

    private bool IsPositionAllowed(int row, int column)
    {
        return true;
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f * BoxSize, LayerMask.GetMask("ChessPlane")))
        {
            selectionColumn = (int)hit.point.x / BoxSize;
            selectionRow = (int)Math.Floor((hit.point.z / BoxSize));
        }
        else
        {
            selectionColumn = -1;
            selectionRow = -1;
        }
    }

    private Vector3 GetTileCenter(int row, int column)
    {
        Vector3 origin = Vector3.zero;
        origin.z += (BoxSize * row) + BoxSize / 2;
        origin.x += (BoxSize * column) + BoxSize / 2;
        return origin;
    }

    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8 * BoxSize;
        Vector3 heightLine = Vector3.forward * 9 * BoxSize;

        for (int row = -1; row <= 8; row++)
        {
            Vector3 start = Vector3.forward * row * BoxSize;
            Debug.DrawLine(start, start + widthLine);
            for (int column = 0; column <= 8; column++)
            {
                start = Vector3.right * column * BoxSize;
                start.z -= 1 * BoxSize;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        if (selectionColumn >= 0 && selectionRow >= -1)
        {
            Debug.DrawLine(
                (Vector3.forward * selectionRow + Vector3.right * selectionColumn) * BoxSize,
                (Vector3.forward * (selectionRow + 1) + Vector3.right * (selectionColumn + 1)) * BoxSize);

            Debug.DrawLine(
                (Vector3.forward * (selectionRow + 1) + Vector3.right * selectionColumn) * BoxSize,
                (Vector3.forward * selectionRow + Vector3.right * (selectionColumn + 1)) * BoxSize);
        }
    }

    private void SetUpTheTiles()
    {
        for (int row = -1; row < 8; row++)
        {
            for (int column = 0; column < 8; column++)
            {
                boardTiles[row, column] = new BoardTiles(GetTileCenter(row, column), row == -1 ? 1 : 0);
            }
        }
    }
}
