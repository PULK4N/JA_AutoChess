using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigurePieceToggle : MonoBehaviour
{
    public delegate void PieceToggleClick();
    public event PieceToggleClick OnPieceToggleClick;
    public void OnClicked()
    {
        OnPieceToggleClick();
    }
}
