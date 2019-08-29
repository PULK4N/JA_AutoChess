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

    [SerializeField]
    private int BoxSize = 10;

    private int selectionX = -1;
    private int selectionY = -1;

    public Figure[,] Figures { get; set; }
    //private Unit selectedUnit;

    public List<GameObject> ChessFigurePrefabs;
    private List<GameObject> _activeChessFigures;

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    private void Start()
    {
        Sprite myImage = Resources.Load("thor", typeof(Sprite)) as Sprite;
        GameObject.Find("Unit1_image").GetComponent<Image>().sprite = myImage;


        GameObject FigureOnBoard = new GameObject();

        GameObject unitPrefab = Resources.Load("footman_Red", typeof(GameObject)) as GameObject;
        GameObject go = Instantiate(unitPrefab, GetTileCenter(0, 0), Quaternion.identity) as GameObject;
        go.transform.SetParent(FigureOnBoard.transform);
        GameObject unitUI = Resources.Load("Figure", typeof(GameObject)) as GameObject;
        GameObject go1 = Instantiate(unitUI, GetTileCenter(0, 0), Quaternion.identity) as GameObject;
        go1.transform.SetParent(FigureOnBoard.transform);

        //myPrefab.AddComponent<Rigidbody>();
        //myPrefab.AddComponent<MeshFilter>();
        //myPrefab.AddComponent<BoxCollider>();
        //myPrefab.AddComponent<MeshRenderer>();

        ChessFigurePrefabs.Add(FigureOnBoard);
        SpawnChessFigure(0, 1, 1);
    }

    private void Update()
    {
        DrawChessBoard();
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,25.0f*BoxSize, LayerMask.GetMask("ChessPlane")))
        {
            //Debug.Log(hit.point);
            selectionX = (int)hit.point.x / BoxSize;
            selectionY = (int)hit.point.z / BoxSize;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }

    }

    private void SpawnChessFigure(int index, int row, int column)
    {
        // quarterion - for orientation, change if needed (default Quaternion.identity)
        //Figures[row, column] = go.GetComponent<Figure>();
        _activeChessFigures = new List<GameObject>();
        _activeChessFigures.Add(ChessFigurePrefabs[index]);
    }

    private void SpawnAllChessFigures()
    {
        _activeChessFigures = new List<GameObject>();
        Figures = new Figure[8, 8];

        //SpawnChessman(0, GetTileCenter(1,1));
    }

    private Vector3 GetTileCenter(int row, int column)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (BoxSize * column) + BoxSize / 2;
        origin.z += (BoxSize * row) + BoxSize / 2;
        return origin;
    }

    private void DrawChessBoard()
    {


        Vector3 widthLine = Vector3.right * 8 *BoxSize;
        Vector3 heightLine = Vector3.forward * 8 *BoxSize;

        for(int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i*BoxSize;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0;  j <=8;  j++)
            {
                start = Vector3.right * j*BoxSize;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        if(selectionX >=0 && selectionY >= 0)
        {
            Debug.DrawLine(
                (Vector3.forward * selectionY + Vector3.right * selectionX) * BoxSize,
                (Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1))* BoxSize);

            Debug.DrawLine(
            (Vector3.forward * (selectionY+1) + Vector3.right * selectionX) * BoxSize,
            (Vector3.forward * selectionY + Vector3.right * (selectionX + 1)) * BoxSize);
        }
    }
}
