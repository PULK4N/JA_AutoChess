using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEvents : MonoBehaviour
{
    public delegate void GameModeChanged();
    public event GameModeChanged DoGameModeChanged;
}
