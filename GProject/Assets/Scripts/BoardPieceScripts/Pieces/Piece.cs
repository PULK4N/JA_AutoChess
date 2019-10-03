using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece
{
    private Enums.Piece _piece;
    public Enums.Piece PieceType { get => _piece; set => _piece = value; }

    private Buff buff;
    public abstract Buff GrantBuff();

    private bool _toggled = false;

    public bool Toggled { get => _toggled; }

    public void Toggle()
    {
        _toggled = !_toggled;
    }
}
