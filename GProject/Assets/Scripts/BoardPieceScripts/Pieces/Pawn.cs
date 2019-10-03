using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public Pawn()
    {
        _piece = Enums.Piece.Pawn;
    }
    private Enums.Piece _piece;
    public new Enums.Piece PieceType { get => _piece; }

    public override Buff GrantBuff()
    {
        return null;
    }
}
