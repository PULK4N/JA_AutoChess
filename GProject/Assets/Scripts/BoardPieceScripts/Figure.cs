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

    public int Range { get { return 1/*unit.Stats.Range*/; } }

    public string Owner { get; }

    public void AttackOrMove()
    {

    }
}
