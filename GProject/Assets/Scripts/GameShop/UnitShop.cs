using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UnitShop
{
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

        for(int i=0;i<units.Count;++i)
        {
            GameObject unit = units[i];
            Sprite unitImage = Resources.Load(unit.GetComponent<Unit>().GetType().Name + "/pictures/ShopImage", typeof(Sprite)) as Sprite;
            GameObject.Find("UnitImage" + i).GetComponent<Image>().sprite = unitImage;
            GameObject.Find("UnitImage" + i).SetActive(true);
        }
    }

    public static bool BuyUnit(string unitName)
    {
        if (MatchManager.Instance.MatchState != Enums.MatchState.Preparation)
            return false;
        //To-Do: if player has enough gold, if not return false
        foreach(GameObject unit in units)
        {
            if (unit.GetComponent<Unit>().GetType().Name == unitName)
            {
                // To-Do: remove gold from player
                GameObject.Find("ChessBoard").GetComponent<BoardManager>().SpawnFigure(unit);
                units.Remove(unit);
                return true;
            }
        }
        return false;
    }

    public static void SellUnit(GameObject figure)
    {
        // return gold to player
        //figure.GetComponent<Figure>().Cost
        GameObject unit = figure.GetComponent<Figure>().Unit.gameObject;
        FigureManager.Disassemble(figure);
        ReturnUnitToPool(unit);
    }

    public static void ReturnUnitToPool(GameObject unit)
    {
        unit.SetActive(false);
        UnitsPool.PutUnitInPool(unit);
    }
}
