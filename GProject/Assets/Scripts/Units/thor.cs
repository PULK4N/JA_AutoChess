using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thor : Unit
{
    public override Attack Ability()
    {
        throw new System.NotImplementedException();
    }

    public override Attack AutoAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void MakeMeAOneStar()
    {
        throw new System.NotImplementedException();
    }

    public override void MakeMeAThreeStar()
    {
        throw new System.NotImplementedException();
    }

    public override void MakeMeATwoStar()
    {
        throw new System.NotImplementedException();
    }

    public new void Start()
    {
        base.Start();
        Stats.Range = 1;
        Stats.Health = 50;
        Stats.Mana = 50;
        Stats.AttackSpeed = 2;
    }
}
