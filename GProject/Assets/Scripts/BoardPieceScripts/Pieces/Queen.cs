using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public Queen()
    {
        _piece = Enums.Piece.Queen;
    }
    private Enums.Piece _piece;
    public new Enums.Piece PieceType { get => _piece; }

    private bool _toggled = false;
    public new bool Toggled { get => _toggled; }
    public new void Toggle()
    {
        _toggled = !_toggled;
    }

    public class QueenBuff : Buff
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
            properties.AbilityPower = 10;
            properties.AttackDamage = 10;
            properties.AttackSpeed = 25;
            properties.Health += 250;
            properties.Mana = -20;
        }
        else
        {
            properties.Shield = 2000;
        }

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<QueenBuff>();
        QueenBuff buff = gameObject.GetComponent<QueenBuff>();
        buff.Initialize(properties, float.PositiveInfinity, 1);

        return buff;
    }
}
