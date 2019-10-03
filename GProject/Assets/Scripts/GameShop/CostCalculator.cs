using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CostCalculator
{
    public static int Calculate(Unit unit, Enums.Piece piece)
    {
        switch(piece)
        {
            case Enums.Piece.Pawn:
                if (unit.Cost <= 8)
                    return unit.Cost;
                return 0;
            case Enums.Piece.Bishop:
                if (unit.Cost <= 3)
                    return 3;
                if (unit.Cost <= 6)
                    return 6;
                return 0;
            case Enums.Piece.Knight:
                if (unit.Cost <= 4)
                    return 4;
                if (unit.Cost <= 8)
                    return 8;
                return 0;
            case Enums.Piece.Rook:
                if (unit.Cost <= 5)
                    return 5;
                if (unit.Cost <= 10)
                    return 10;
                return 0;
            case Enums.Piece.Queen:
                return 8;
        }
        return 0;
    }
}
