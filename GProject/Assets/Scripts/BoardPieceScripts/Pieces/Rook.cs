using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public Rook()
    {
        _piece = Enums.Piece.Rook;
    }
    private Enums.Piece _piece;
    public new Enums.Piece PieceType { get => _piece; }

    private bool _toggled = false;
    public new bool Toggled { get => _toggled; }
    public new void Toggle()
    {
        _toggled = !_toggled;
    }

    public class RookBuff : Buff
    {
        public override float AddStack()
        {
            return 0;
        }
    }

    public override Buff GrantBuff()
    {
        Unit.Properties properties = new Unit.Properties();
        properties.AttackDamage = 10;
        if (_toggled)
        {
            properties.Armor = 10;
            properties.MagicResist = 10;
        }
        else
        {
            properties.Armor = 30;
            properties.MagicResist = 30;
            properties.Silenced = 1;
        }

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<RookBuff>();
        RookBuff buff = gameObject.GetComponent<RookBuff>();
        buff.Initialize(properties, float.PositiveInfinity, 1);

        return buff;
    }
}
