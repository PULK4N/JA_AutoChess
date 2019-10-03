using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    GameObject PlayerPrefab;
    [SerializeField]
    GameObject Board;
    public BoardManager BoardManager;
    string Name;
    public int Pawns;
    int Health;
    //poslati sve ova cuda i onda ako is local player da se setuje

    public void Start()
    {
        PlayerPrefab = this.gameObject;
        BoardManager = Board.GetComponent<BoardManager>();
    }
}  
