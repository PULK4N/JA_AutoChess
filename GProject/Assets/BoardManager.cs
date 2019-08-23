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
            Debug.Log(hit.point);
        }
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
    }
}
