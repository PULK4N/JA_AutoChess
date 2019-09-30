using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureSell : MonoBehaviour
{
    public void OnClicked(GameObject figure)
    {
        GameObject.Find("ChessBoard").GetComponent<BoardManager>().SellFigure(figure);
    }
}
