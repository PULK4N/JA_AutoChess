﻿using System.Collections.Generic;
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

    public Unit Unit;
    public Piece Piece;
    public FigureUIManager FigureUIManager;
    public int Cost;

    public Enums.Synergy Mythology { get => Unit.Stats.Synergies[0]; }
    public Enums.Synergy Diety { get => Unit.Stats.Synergies[1]; }
    public Enums.Synergy AdditionalDiety { get => Unit.Stats.Synergies.Count > 2 ? Unit.Stats.Synergies[2] : Enums.Synergy.none; }

    public string Owner { get; set; }

    public int Range { get { return Unit.Stats.Range; } }
    public float CurrentHealth { get { return Unit.CurrentHealth; } }

    private Figure _target;
    public Figure Target { get => _target; }

    public void CarryEnemyColors()
    {
        FigureUIManager.CarryEnemyColors();
    }

    public delegate void Sell(GameObject figure);
    public event Sell OnSell;
    public void Start()
    {
        FigureUIManager.OnFigureSellClick += () => OnSell(this.gameObject);
        FigureUIManager.OnPieceToggleClick += toggle => Piece.Toggle();
        FigureUIManager.SetPieceToggleText(Piece.PieceType);
        FigureUIManager.SetSpellTooltip(Unit.GetAbilityDescription());
        FigureUIManager.SetSpellImage(Unit);
    }

    public void Update()
    {
        AttackOrMove();
        UpdateHealthAndMana();
    }

    public void UpdateHealthAndMana()
    {
        FigureUIManager.SetHealth(Unit.Stats.Health / Unit.CurrentHealth);
        FigureUIManager.SetMana(Unit.Stats.Mana / Unit.CurrentMana);
    }

    private bool _startOfMatch = false;
    private float _lastAttacked = 0;
    public void AttackOrMove()
    {
        if (MatchManager.Instance.MatchState == Enums.MatchState.Preparation)
            return;

        if (Untargetable)
            return;

        if(Piece.PieceType==Enums.Piece.Knight && _startOfMatch)
        {
            Point destination = Dijkstra.KnightJumpOnStart(this);
            OnMove(this, destination.X, destination.Y);
            Position.Row = destination.X;
            Position.Column = destination.Y;
        }

        if (Time.time <_lastAttacked + Unit.Stats.AttackSpeed)
            return;

        if (Unit.Stats.Stunned > 0)
            return;

        _lastAttacked = Time.time;

        if (Unit.CurrentMana >= Unit.Stats.Mana && Unit.Stats.Silenced <= 0)
        {
            if (Dijkstra.EnemyInsideRange(this, Unit.Stats.AbilityRange) == null)
                EnemyOutsideRange();
            Attack attack =Unit.Ability();
            return;
        }
        if (_target != null && !_target.Untargetable && Dijkstra.IsEnemyInRange(this, _target))
        {
            if (Unit.Stats.Disarmed <= 0)
            {
                int maxManaGainPerAttack = 10;
                Unit.CurrentMana += Mathf.Min(maxManaGainPerAttack, Unit.Stats.Health / Unit.Stats.AttackDamage);
                Attack attack = Unit.AutoAttack();
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
        Point nextPosition;
        if (Piece.PieceType != Enums.Piece.Knight)
            nextPosition = Dijkstra.FindNextStep(this);
        else
            nextPosition = Dijkstra.FindNextStep(this, true);
        if (nextPosition.X < 0 || nextPosition.Y < 0)
            return;
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
        if (Unit.Stats.HasDamageReturn > 0)
            damageReturn = damage - damageDealt;

        return damageDealt;
    }

    public float TakeDamage(Enums.DamageType damageType, float damage, List<Buff> buffs = null)
    {
        float damageDealt;

        if (Untargetable || Unit.Stats.IsInvounrable > 0)
            return 0;

        damageDealt = CalculateDamage(damageType, damage);
        float damageExceedsShield = Unit.Stats.Shield -= damage;
        if (damageExceedsShield > 0)
            Unit.CurrentHealth -= damageExceedsShield;

        if (Unit.CurrentHealth <= 0)
            Die();

        Unit.CurrentMana += Mathf.Min(50, Unit.Stats.Mana / 100 * (damage / Unit.BaseHealth));

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
                damageDealt = damage * 30 / (30 + Unit.Stats.MagicResist);
                break;
            case Enums.DamageType.Physical:
                damageDealt = damage * 30 / (30 + Unit.Stats.Armor);
                break;
            default:
                damageDealt = damage;
                break;
        }
        damageDealt -= Unit.Stats.DamageReduction;
        damageDealt *= (100 - Unit.Stats.DamageReductionPercentage) / 100;
        return damageDealt;
    }

    private void AddBuff(Buff buff)
    {
        foreach (Buff alreadyAppliedBuff in Unit.Buffs)
            if (alreadyAppliedBuff.GetType() == buff.GetType())
            {
                float damage = alreadyAppliedBuff.AddStack();
                TakeDamage(alreadyAppliedBuff.DamageType, damage);
                return;
            }
        Unit.Buffs.Add(buff);
        Unit.Stats += buff.BuffStats;
        buff.OnExpire += o => Unit.Stats -= o.BuffStats;
    }

    public int DPS { get; set; }

    public void DamageFeedback(Enums.DamageType damageType, float damage)
    {
        DPS += (int)damage;
        switch(damageType)
        {
            case Enums.DamageType.Physical:
                if (Unit.Stats.LifeSteal > 0)
                    Heal(damage * Unit.Stats.LifeSteal / 100);
                break;
            case Enums.DamageType.Magical:
                if (Unit.Stats.SpellWamp > 0)
                    Heal(damage * Unit.Stats.SpellWamp / 100);
                break;
            default:
                break;
        }
    }

    public void Heal(float health, List<Buff> buffs = null)
    {
        Unit.CurrentHealth += health;
        if (Unit.CurrentHealth > Unit.Stats.Health)
            Unit.CurrentHealth = Unit.Stats.Health;
    }

    public delegate void Death(Figure sender);
    public event Death OnDeath;
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
        Unit.Buffs.ForEach(b => b.Dispell());
        Unit.Buffs.Clear();
        OnMove(this, _matchStartingPosition.Row, _matchStartingPosition.Column);
        Position.Row = _matchStartingPosition.Row;
        Position.Column = _matchStartingPosition.Column;
        Unit.CurrentHealth = Unit.Stats.Health;
        Unit.CurrentMana = Unit.Stats.StartingMana;
        FigureUIManager.EnableToggle();
    }

    public void PrepareForBattle()
    {
        Untargetable = false;
        _matchStartingPosition.Row = Position.Row;
        _matchStartingPosition.Column = Position.Column;
        Buff pieceBuff = Piece.GrantBuff();
        if (pieceBuff != null)
            AddBuff(pieceBuff);
        List<Buff> buffs = SynergyManager.GetBuffsForFigure(this);
        foreach (Buff buff in buffs)
            AddBuff(buff);
        Unit.CurrentHealth = Unit.Stats.Health;
        Unit.CurrentMana = Unit.Stats.StartingMana;
        FigureUIManager.ToggleDisable();
    }
}
