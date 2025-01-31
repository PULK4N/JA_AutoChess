﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> PlayerObjects;
    public List<Player> Players;
    void Awake()
    {
        SetupSingleton();
    }

    #region Singleton
    public static MatchManager Instance;
    /// <summary>
    /// Creates a single instance of this object
    /// </summary>
    private void SetupSingleton()
    {
        if (MatchManager.Instance != null && MatchManager.Instance != this)
        {
            Destroy(this);
            return;
        }

        MatchManager.Instance = this;
        GameObject.DontDestroyOnLoad(transform.root.gameObject);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _preparationDuration = 30;
        _battleDuration = 60;
        _round = 1;

        PlayerObjects = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach (GameObject player in PlayerObjects)
            Players.Add(player.GetComponent<Player>());
    }

    public Text Text;

    private int _round;
    public int Level { get => _round < 3 ? _round : (_round - 3) / 5 + 1; }
    public int Round { get => _round; }

    private int _preparationDuration;
    private float _preparationStartTime = 0;
    private int _battleDuration;
    private float _battleStartTime = float.NegativeInfinity;

    public Enums.MatchState MatchState;

    public delegate void StateChange(Enums.MatchState matchState);
    public event StateChange OnStateChage;

    private void MatchPlayers()
    {
        List<Player> players = MatchMaking.GenerateMatches(Players);
        for (int i = 0; i < players.Count-1; ++i)
            players[i + 1].BoardManager.SpawnEnemyFigures(players[i].BoardManager.CopyActiveFigures());
        players[0].BoardManager.SpawnEnemyFigures(players[players.Count - 1].BoardManager.CopyActiveFigures());
    }

    private void ChangeState()
    {
        switch(MatchState)
        {
            case Enums.MatchState.Battle:
                _preparationStartTime = Time.time;
                MatchState = Enums.MatchState.Preparation;
                ++_round;
                if (!UnitShop.ShopLocked)
                    UnitShop.Reroll();
                OnStateChage(MatchState);
                break;
            case Enums.MatchState.Preparation:
                _battleStartTime = Time.time;
                MatchState = Enums.MatchState.Battle;
                MatchPlayers();
                OnStateChage(MatchState);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= Mathf.Max(_preparationStartTime + _preparationDuration,
                                                    _battleStartTime + _battleDuration))
            ChangeState();

        int secondsLeft = (int)(Mathf.Max(_battleStartTime + _battleDuration, _preparationStartTime + _preparationDuration) - Time.time);
        Text.text = MatchState.ToString() + ": " + secondsLeft;

        //if (MatchState == Enums.MatchState.Battle && secondsLeft == _battleDuration - 2)
        //    OnStateChage(MatchState);
    }
}
