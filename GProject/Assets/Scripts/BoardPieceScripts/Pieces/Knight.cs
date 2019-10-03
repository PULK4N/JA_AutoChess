using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public Knight()
    {
        _piece = Enums.Piece.Knight;
    }
    private Enums.Piece _piece;
    public new Enums.Piece PieceType { get => _piece; }

    private bool _toggled = false;
    public new bool Toggled { get => _toggled; }
    public new void Toggle()
    {
        _toggled = !_toggled;
    }

    public class KnightBuff : Buff
    {
        public override float AddStack()
        {
            return 0;
        }
    }

    public override Buff GrantBuff()
    {
        Unit.Properties properties = new Unit.Properties();
        properties.AttackSpeed = 50;
        if (!_toggled)
            properties.Range = 2;

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<KnightBuff>();
        KnightBuff buff = gameObject.GetComponent<KnightBuff>();
        buff.Initialize(properties, float.PositiveInfinity, 1);

        return buff;
    }
}
