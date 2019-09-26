using System.Collections.Generic;
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

    public string Owner { get; set; }

    public int Range { get { return unit.Stats.Range; } }
    public float CurrentHealth { get { return unit.CurrentHealth; } }

    private Figure _target;
    public Figure Target { get => _target; }

    private float _lastAttacked = 0;
    public void AttackOrMove()
    {
        if (Time.realtimeSinceStartup < _lastAttacked + unit.Stats.AttackSpeed * 1000)
            return;

        if (unit.Stats.Stunned > 0)
            return;

        _lastAttacked = Time.realtimeSinceStartup;

        if (unit.CurrentMana >= unit.Stats.Mana && unit.Stats.Silenced <= 0)
        {
            if (Dijkstra.EnemyInsideRange(this, unit.Stats.AbilityRange) == null)
                EnemyOutsideRange();
            Attack attack =unit.Ability();
            return;
        }
        if(!_target.Untargetable && Dijkstra.IsEnemyInRange(this, _target))
        {
            if (unit.Stats.Disarmed <= 0)
            {
                int maxManaGainPerAttack = 10;
                unit.CurrentMana += Mathf.Min(maxManaGainPerAttack, unit.Stats.Health / unit.Stats.AttackDamage);
                Attack attack=unit.AutoAttack();
                return;
            }
        }
        else
        {
            Figure nextTarget = Dijkstra.EnemyInsideRange(this, Range);
            if (nextTarget == null)
                EnemyOutsideRange();
            else
                _target = nextTarget;
        }
    }

    private void EnemyOutsideRange()
    {
        Point nextPosition = Dijkstra.FindNextStep(this);
        OnMove(this, nextPosition.X, nextPosition.Y);
        Position.Row = nextPosition.X;
        Position.Column = nextPosition.Y;
    }

    public delegate void Move(Figure sender, int nextRow, int nextColumn);
    public event Move OnMove;

    public float TakeDamage(Enums.DamageType damageType, float damage, out float damageReturn, List<Buff> buffs = null)
    {
        damageReturn = 0;
        float damageDealt = TakeDamage(damageType, damage, buffs);
        if (unit.Stats.HasDamageReturn > 0)
            damageReturn = damage - damageDealt;
        return damageDealt;
    }

    public float TakeDamage(Enums.DamageType damageType, float damage, List<Buff> buffs = null)
    {
        float damageDealt;

        if (Untargetable || unit.Stats.IsInvounrable > 0)
            return 0;

        damageDealt = CalculateDamage(damageType, damage);
        float damageExceedsShield = unit.Stats.Shield -= damage;
        if (damageExceedsShield > 0)
            unit.CurrentHealth -= damageExceedsShield;

        if (unit.CurrentHealth <= 0)
            Die();

        unit.CurrentMana += Mathf.Min(50, unit.Stats.Mana / 100 * (damage / unit.BaseHealth));

        if (buffs != null)
            foreach (Buff buff in buffs)
                AddBuff(buff);

            return damageDealt;
    }

    private float CalculateDamage(Enums.DamageType damageType, float damage)
    {
        float damageDealt;
        switch (damageType)
        {
            case Enums.DamageType.Magical:
                damageDealt = damage * 30 / (30 + unit.Stats.MagicResist);
                break;
            case Enums.DamageType.Physical:
                damageDealt = damage * 30 / (30 + unit.Stats.Armor);
                break;
            default:
                damageDealt = damage;
                break;
        }
        damageDealt -= unit.Stats.DamageReduction;
        damageDealt *= (100 - unit.Stats.DamageReductionPercentage) / 100;
        return damageDealt;
    }

    private void AddBuff(Buff buff)
    {
        foreach (Buff alreadyAppliedBuff in unit.Buffs)
            if (alreadyAppliedBuff.GetType() == buff.GetType())
            {
                float damage = alreadyAppliedBuff.AddStack();
                TakeDamage(alreadyAppliedBuff.DamageType, damage);
                return;
            }
        unit.Buffs.Add(buff);
        unit.Stats += buff.BuffStats;
        buff.OnExpire += o => unit.Stats -= o.BuffStats;
    }

    public int DPS { get; set; }

    public void DamageFeedback(Enums.DamageType damageType, float damage)
    {
        DPS += (int)damage;
        switch(damageType)
        {
            case Enums.DamageType.Physical:
                if (unit.Stats.LifeSteal > 0)
                    Heal(damage * unit.Stats.LifeSteal / 100);
                break;
            case Enums.DamageType.Magical:
                if (unit.Stats.SpellWamp > 0)
                    Heal(damage * unit.Stats.SpellWamp / 100);
                break;
            default:
                break;
        }
    }

    public void Heal(float health, List<Buff> buffs = null)
    {
        unit.CurrentHealth += health;
        if (unit.CurrentHealth > unit.Stats.Health)
            unit.CurrentHealth = unit.Stats.Health;
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
        Untargetable = true;
        OnDeath(this);
    }

    private Place _matchStartingPosition;
    public Place MatchStartingPosition { get => _matchStartingPosition; }
    public void Restart()
    {
        Untargetable = true;
        unit.Buffs.ForEach(b => b.Dispell());
        unit.Buffs.Clear();
        OnMove(this, _matchStartingPosition.Row, _matchStartingPosition.Column);
        Position.Row = _matchStartingPosition.Row;
        Position.Column = _matchStartingPosition.Column;
        unit.CurrentHealth = unit.Stats.Health;
        unit.CurrentMana = 0;
    }

    public void PrepareForBattle()
    {
        Untargetable = false;
    }

    public delegate void Death(Figure sender);
    public event Death OnDeath;
}
