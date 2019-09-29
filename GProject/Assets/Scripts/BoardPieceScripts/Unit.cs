using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public struct Properties
    {
        public int Range;
        public int AbilityRange;
        public float AttackSpeed;
        public float AttackDamage;
        public float AbilityPower;
        public Enums.DamageType AutoAttackDamageType;
        public Enums.DamageType AbilityDamageType;

        public float Health;
        public float Mana;
        public float Armor;
        public float MagicResist;
        public float Shield;
        public float DamageReduction;
        public float DamageReductionPercentage;

        public float LifeSteal;
        public float SpellWamp;
        //negative status
        public byte Stunned;
        public byte Silenced;
        public byte Disarmed;
        //for special cases
        public byte IsInvounrable;
        public byte HasDamageReturn;

        public Enums.Deity Deity;
        public Enums.Mythology Mythology;

        public int Cost;

        public delegate void MaxHpIncrease(float health);
        public event MaxHpIncrease OnMaxHpIncrease;
        public delegate void MaxManaIncrease(float health);
        public event MaxHpIncrease OnMaxManaIncrease;
        public static Properties operator +(Properties property, Properties buff)
        {
            property.Range += buff.Range;
            property.AbilityRange += buff.Range;                         // Range also buffs ability range
            property.AttackSpeed *= (100 + buff.AttackSpeed) / 100;      // Attack speed adds to itself in percentage
            property.AttackDamage += buff.AttackDamage;
            property.AbilityPower += buff.AbilityPower;
            if (buff.AutoAttackDamageType != Enums.DamageType.none)
                property.AutoAttackDamageType = buff.AutoAttackDamageType;
            if (buff.AbilityDamageType != Enums.DamageType.none)
                property.AbilityDamageType = buff.AbilityDamageType;

            property.Health += buff.Health;
            if (buff.Health > 0)
                property.OnMaxHpIncrease(buff.Health);
            property.Mana += buff.Mana;
            property.Armor += buff.Armor;
            property.MagicResist += buff.MagicResist;
            property.Shield += buff.Shield;
            property.DamageReduction += buff.DamageReduction;
            property.DamageReductionPercentage += buff.DamageReductionPercentage;

            property.LifeSteal += buff.LifeSteal;
            property.SpellWamp += buff.SpellWamp;

            property.Stunned += buff.Stunned;
            property.Silenced += buff.Silenced;
            property.Disarmed += buff.Disarmed;

            property.IsInvounrable += buff.IsInvounrable;
            property.HasDamageReturn += buff.HasDamageReturn;

            return property;
        }

        public static Properties operator -(Properties property, Properties buff)
        {
            property.Range -= buff.Range;
            property.AbilityRange -= buff.Range;                       // Range also buffs ability range
            property.AttackSpeed /= 1 + buff.AttackSpeed;              // Attack speed adds to itself in percentage
            property.AttackDamage -= buff.AttackDamage;
            property.AbilityPower -= buff.AbilityPower;

            property.Health -= buff.Health;
            property.Mana -= buff.Mana;
            property.Armor -= buff.Armor;
            property.MagicResist -= buff.MagicResist;
            property.Shield -= buff.Shield;
            property.DamageReduction -= buff.DamageReduction;
            property.DamageReductionPercentage -= buff.DamageReductionPercentage;

            property.LifeSteal -= buff.LifeSteal;
            property.SpellWamp -= buff.SpellWamp;

            property.IsInvounrable -= buff.IsInvounrable;
            property.HasDamageReturn -= buff.HasDamageReturn;

            return property;
        }
    }

    public void Start()
    {
        Stats.OnMaxHpIncrease += hp => CurrentHealth += hp;
        _baseHealth = CurrentHealth = Stats.Health;
    }
    public Properties Stats;
    public float CurrentHealth;
    public float CurrentMana;

    private float _baseHealth;
    public float BaseHealth { get => _baseHealth; }

    public abstract Attack AutoAttack();

    public abstract Attack Ability();

    public List<Buff> Buffs;

    public abstract void MakeMeAOneStar();
    public abstract void MakeMeATwoStar();
    public abstract void MakeMeAThreeStar();
}