using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece
{
    private Buff buff;
    public abstract void GrantBuff();
    public abstract void RemoveBuff();

    private bool _toggled = false;

    public bool Toggled { get => _toggled; }
    public void Toggle()
    {
        _toggled = !_toggled;
    }
}
