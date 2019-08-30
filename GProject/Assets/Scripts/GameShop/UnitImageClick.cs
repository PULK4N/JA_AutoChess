using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitImageClick : MonoBehaviour
{
    public void OnClicked(Image image)
    {
        GameObject.Find("ChessBoard").GetComponent<BoardManager>().SpawnFigure(image.sprite.name);
        image.enabled = false;
    }
}
