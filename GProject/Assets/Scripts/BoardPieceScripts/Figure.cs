using System.Drawing;
using UnityEngine;

public class Figure : MonoBehaviour
{
    public struct Place
    {
        public int Row;
        public int Column;
    }

    public Place Position;
    public bool Untargetable;

    private Unit unit;
    private Piece piece;
    private int cost;

    public int Range { get { return unit.Stats.Range; } }

    public string Owner { get; set; }

    public void AttackOrMove()
    {

    }

    public void Die()
    {
        this.gameObject.SetActive(false);
        Untargetable = true;
        OnDeath(this);
    }

    public delegate void _delegate(Figure sender);

    public event _delegate OnDeath;
}
