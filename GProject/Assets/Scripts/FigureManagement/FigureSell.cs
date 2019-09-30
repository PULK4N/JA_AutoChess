using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureSell : MonoBehaviour
{
    public delegate void SellClick();
    public event SellClick OnSellClick;
    public void OnClicked()
    {
        OnSellClick();
    }
}
