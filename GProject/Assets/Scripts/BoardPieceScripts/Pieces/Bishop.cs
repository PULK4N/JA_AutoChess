using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public Bishop()
    {
        _piece = Enums.Piece.Bishop;
    }
    private Enums.Piece _piece;
    public new Enums.Piece PieceType { get => _piece; }

    private bool _toggled = false;
    public new bool Toggled { get => _toggled; }
    public new void Toggle()
    {
        _toggled = !_toggled;
    }

    public class BishopBuff : Buff
    {
        public override float AddStack()
        {
            return 0;
        }
    }

    public override Buff GrantBuff()
    {
        Unit.Properties properties = new Unit.Properties();
        if (_toggled)
            properties.StartingMana = float.MaxValue;
        else
            properties.Mana = -50;

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<BishopBuff>();
        BishopBuff buff = gameObject.GetComponent<BishopBuff>();
        buff.Initialize(properties, float.PositiveInfinity, 1);

        return buff;
    }


}
