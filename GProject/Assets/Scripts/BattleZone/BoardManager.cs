using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //public Unit[,] Units { get; set; }
    //private Unit selectedUnit;

    public List<GameObject> ChessUnitsPrefabs;
    private List<GameObject> _activeChessUnits;

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    private void Start()
    {
        
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

    private void SpawnChessman(int index, int row, int column)
    {
        // quarterion - for orientation, change if needed (default Quaternion.identity)
        GameObject go = Instantiate(ChessUnitsPrefabs[index], GetTileCenter(row, column), orientation) as GameObject;
        go.transform.SetParent(transform);
        //Units[row, column] = go.GetComponent<Unit>();
        _activeChessUnits.Add(go);
    }

    private void SpawnAllChessUnits()
    {
        //_activeChessUnits = new List<GameObject>();
        //Units = new Unit[8, 8];

        //SpawnChessman(0, GetTileCenter(1,1));
    }

    private Vector3 GetTileCenter(int row, int column)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TileWidth * column) + TileWidth;
        origin.z += (TileWidth * row) + TileWidth;
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
