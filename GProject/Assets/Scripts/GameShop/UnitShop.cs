using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UnitShop
{
    public static bool ShopLocked;
    public static void Reroll()
    {
        foreach(GameObject unit in units)
        {
            ReturnUnitToPool(unit);
        }
        PlaceNewUnitsInShop();
    }

    private static List<GameObject> units = new List<GameObject>();
    public static void PlaceNewUnitsInShop()
    {
        units = UnitsPool.GenerateUnits(MatchManager.Instance.Level);

        if (units == null)
            return;

        GameObject.Find("UnitPick").GetComponent<ShopUI>().PlaceNewUnits(units);
    }

    public static bool BuyUnit(GameObject unit, Enums.Piece piece)
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        int cost = CostCalculator.Calculate(unit.GetComponent<Unit>(), piece);
        if (player.Pawns < cost)
            return false;
        if (!GameObject.Find("ChessBoard").GetComponent<BoardManager>().SpawnFigure(unit, piece))
            return false;
        player.Pawns -= cost;
        units.Remove(unit);
        return true;
    }

    public static void SellUnit(GameObject figure)
    {
        if (MatchManager.Instance.MatchState != Enums.MatchState.Preparation)
            return;
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.Pawns += figure.GetComponent<Figure>().Cost;
        GameObject unit = figure.GetComponent<Figure>().Unit.gameObject;
        FigureManager.Disassemble(figure);
        unit.SetActive(false);
        ReturnUnitToPool(unit);
    }

    public static void ReturnUnitToPool(GameObject unit)
    {
        UnitsPool.PutUnitInPool(unit);
    }
}
