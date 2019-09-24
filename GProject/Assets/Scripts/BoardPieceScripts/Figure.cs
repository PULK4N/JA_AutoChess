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

    private Figure _target;
    public Figure Target { get => _target; }

    public void AttackOrMove()
    {
        int maxManaGainPerAttack = 10;
        //unit.CurrentMana += Mathf.Min(maxManaGainPerAttack, unit.CurrentHealth / unit.Stats.AttackDamage);
    }

    public float TakeDamage(Enums.DamageType damageType, float damage, out float damageReturn)
    {
        float damageDealt;
        damageReturn = 0;

        if (Untargetable || unit.Stats.IsInvounrable > 0)
            return 0;

        damageDealt = CalculateDamage(damageType, damage);
        float damageExceedsShield = unit.Stats.Shield -= damage;
        if (damageExceedsShield > 0)
            unit.CurrentHealth -= damageExceedsShield;

        if (unit.Stats.HasDamageReturn > 0)
            damageReturn = damage - damageDealt;

        if (unit.CurrentHealth <= 0)
            Die();

        unit.CurrentMana += Mathf.Min(50, unit.Stats.Mana / 100 * (damage / unit.Stats.Health - (damageDealt / damage) / 5));

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

    public void Heal(float health)
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

    public delegate void _delegate(Figure sender);

    public event _delegate OnDeath;
}
