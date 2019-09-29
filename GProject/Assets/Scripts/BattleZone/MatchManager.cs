using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
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
    }

    private int _preparationDuration;
    private float _preparationStartTime = 0;
    private int _battleDuration;
    private float _battleStartTime = 0;
    public Enums.MatchState MatchState;

    private void ChangeState()
    {
        switch(MatchState)
        {
            case Enums.MatchState.Battle:
                _preparationStartTime = Time.time;
                MatchState = Enums.MatchState.Preparation;
                break;
            case Enums.MatchState.Preparation:
                _battleStartTime = Time.time;
                MatchState = Enums.MatchState.Battle;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= Mathf.Max(_preparationStartTime + _preparationDuration,
                                                    _battleStartTime + _battleDuration))
            ChangeState();
    }
}
