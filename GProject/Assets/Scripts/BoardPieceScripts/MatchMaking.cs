using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMaking : MonoBehaviour
{
    private List<GameObject> PreviousListOfMatches = new List<GameObject>();

    public List<GameObject> GenerateMatches(List<GameObject> ListOfPlayers)
    {
        List<GameObject> CoppiedListOfPlayers = new List<GameObject>(ListOfPlayers);
        List<GameObject> ListOfMatches = new List<GameObject>();
        int j = 0;

        System.Random random = new System.Random(System.DateTime.Now.Millisecond);
        GameObject player = CoppiedListOfPlayers[0];

        while (CoppiedListOfPlayers.Count > 0)
        {
            GameObject enemy;
            GameObject lastEnemy;
            CoppiedListOfPlayers.Remove(player);
            if (PreviousListOfMatches.Count > 0)
            {
                if (PreviousListOfMatches.Contains(player))
                {
                    j = PreviousListOfMatches.IndexOf(player);
                }
                if (PreviousListOfMatches.Count == j + 1)
                {
                    j = -1;
                }
                lastEnemy = PreviousListOfMatches[j + 1];
                if (CoppiedListOfPlayers.Contains(lastEnemy))
                {
                    CoppiedListOfPlayers.Remove(lastEnemy);
                    enemy = CoppiedListOfPlayers[random.Next(CoppiedListOfPlayers.Count)];
                    CoppiedListOfPlayers.Add(lastEnemy);
                }
                else
                {
                    if (CoppiedListOfPlayers.Count == 0)
                    {
                        ListOfMatches.Add(player);
                        continue;
                    }
                    enemy = CoppiedListOfPlayers[random.Next(CoppiedListOfPlayers.Count)];
                }
            }
            else
            {
                return ListOfPlayers;
            }
            ListOfMatches.Add(player);
            player = enemy;
        }
        PreviousListOfMatches = ListOfMatches;
        return ListOfMatches;
    }
        
}