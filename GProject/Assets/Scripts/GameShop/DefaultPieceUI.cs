using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultPieceUI : MonoBehaviour
{
    public Button BtnPawn;
    public Button BtnBishop;
    public Button BtnKnight;
    public Button BtnRook;
    public Button BtnQueen;
    private List<Button> buttons;

    private Enums.Piece _defaultPiece;
    public Enums.Piece DefaultPiece { get => _defaultPiece; }

    public delegate void DefaultPieceClick(Enums.Piece piece);
    public event DefaultPieceClick OnDefaultPieceClick;

    public void Start()
    {
        buttons = new List<Button>();
        buttons.Add(BtnPawn);
        buttons.Add(BtnBishop);
        buttons.Add(BtnKnight);
        buttons.Add(BtnRook);
        buttons.Add(BtnQueen);
    }

    private void ButtonClicked(Button button, Enums.Piece piece)
    {
        if (_defaultPiece != piece)
        {
            _defaultPiece = piece;
            button.image.color = Color.cyan;
            foreach (Button b in buttons)
            {
                if (b != button)
                    b.image.color = Color.white;
            }
        }
        else
        {
            _defaultPiece = Enums.Piece.none;
            button.image.color = Color.white;
        }
        OnDefaultPieceClick(_defaultPiece);
    }

    public void PawnClicked()
    {
        ButtonClicked(BtnPawn, Enums.Piece.Pawn);
    }

    public void BishopClicked()
    {
        ButtonClicked(BtnBishop, Enums.Piece.Bishop);
    }

    public void KnightClicked()
    {
        ButtonClicked(BtnKnight, Enums.Piece.Knight);
    }

    public void RookClicked()
    {
        ButtonClicked(BtnRook, Enums.Piece.Rook);
    }

    public void QueenClicked()
    {
        ButtonClicked(BtnQueen, Enums.Piece.Queen);
    }
}
