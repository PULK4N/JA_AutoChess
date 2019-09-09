using System;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript
{
    #region DeclaringRandAndLists
    System.Random rand = new System.Random(System.Guid.NewGuid().GetHashCode());
    public static List<GameObject> Common = new List<GameObject>();
    public static List<GameObject> UnCommon = new List<GameObject>();
    public static List<GameObject> Rare = new List<GameObject>();
    public static List<GameObject> Epic = new List<GameObject>();
    public static List<GameObject> Legendary = new List<GameObject>();

    public static List<List<GameObject>> UnitTypes = new List<List<GameObject>>{ Common, UnCommon, Rare, Epic, Legendary };
    #endregion

    #region UnitsGatheringMatrix
    int[,] UnitsGatheringMatrix = {
//     Common | UnCommon | Rare  | Epic  | Legendary
        {100,      0,        0,      0,     0 },                //Level 1
        {70,     100,        0,      0,     0 },                //Level 2
        {60,      95,      100,      0,     0 },                //Level 3
        {50,      85,      100,      0,     0 },                //Level 4
        {40,      75,       98,     100,    0 },                //Level 5
        {33,      63,       93,     100,    0 },                //Level 6
        {30,      60,       90,     100,    0 },                //Level 7
        {24,      54,       84,      99,    100  },             //Level 8
        {22,      52,       77,      97,    100  },             //Level 9
        {19,      44,       69,      94,    100  },             //Level 10

        };

    #endregion

    public (GameObject Unit1, GameObject Unit2, GameObject Unit3, GameObject Unit4, GameObject Unit5) GenerateUnits(int level)
    {

        #region OutPutBasedOnLevel
        GameObject Unit1 = GetUnit(rand.Next(100), level);
        GameObject Unit2 = GetUnit(rand.Next(100), level);
        GameObject Unit3 = GetUnit(rand.Next(100), level);
        GameObject Unit4 = GetUnit(rand.Next(100), level);
        GameObject Unit5 = GetUnit(rand.Next(100), level);
        #endregion

        if (Unit1 != null && Unit2 != null && Unit3 != null && Unit4 != null && Unit5 != null)
        {
            return (Unit1, Unit2, Unit3, Unit4, Unit5);
        }
        else
        {
            return (null, null, null, null, null);
        }

    }

    public GameObject GetUnit(int RandomNumber,int level)
    {
        List<GameObject> ListOfUnits = null;

        //Point ListOfUnits to a certain list to aquire units from depending on a level
        for (int i = 0; i < 5; i++)
        {
            if (RandomNumber <= UnitsGatheringMatrix[level, i])
            {
                ListOfUnits = UnitTypes[i];
                break;
            }
        }

        //If it didn't come to an error take an unit from the list
        if (ListOfUnits != null && ListOfUnits.Count != 0)
        {
            RandomNumber = rand.Next(ListOfUnits.Count);
            GameObject Unit = ListOfUnits[RandomNumber];
            return Unit;
        }
        //if list is empty get another unit via recursion
        else if (ListOfUnits.Count == 0)
        {
            return GetUnit(RandomNumber, level);
        }
        return null;
    }
}
