using System.Collections.Generic;

public abstract class Unit
{
    public struct Properties
    {
        public int Range;
        public float AttackSpeed;
        public float AttackDamage;
        public float AbilityPower;

        public float Health;
        public float Mana;
        public float Armor;
        public float MagicResist;
        public float Shield;
        public float DamageReduction;
        public float DamageReductionPercentage;

        public float LifeSteal;
        public float SpellWamp;
        //for special cases
        public byte IsInvounrable;
        public byte HasDamageReturn;

        public List<Buff> Buffs;

        public Enums.Deity Deity;
        public Enums.Mythology Mythology;

        public int Cost;
        public bool IsUntargetable;

        public delegate void MaxHpIncrease(float health);
        public event MaxHpIncrease OnMaxHpIncrease;
        public delegate void MaxManaIncrease(float health);
        public event MaxHpIncrease OnMaxManaIncrease;
        public static Properties operator +(Properties property, Properties buff)
        {
            property.Range += buff.Range;
            property.AttackSpeed += buff.AttackSpeed;
            property.AttackDamage += buff.AttackDamage;
            property.AbilityPower += buff.AbilityPower;

            property.Health += buff.Health;
            if (buff.Health > 0)
                property.OnMaxHpIncrease(buff.Health);
            property.Mana += buff.Mana;
            if (buff.Mana < 0)
                property.OnMaxHpIncrease(buff.Mana);
            property.Armor += buff.Armor;
            property.MagicResist += buff.MagicResist;
            property.Shield += buff.Shield;
            property.DamageReduction += buff.DamageReduction;
            property.DamageReductionPercentage += buff.DamageReductionPercentage;

            property.LifeSteal += buff.LifeSteal;
            property.SpellWamp += buff.SpellWamp;

            property.IsInvounrable += buff.IsInvounrable;
            property.HasDamageReturn += buff.HasDamageReturn;

            return property;
        }

        public static Properties operator -(Properties property, Properties buff)
        {
            property.Range -= buff.Range;
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

    public Unit()
    {
        Stats.OnMaxHpIncrease += hp => CurrentHealth += hp;
        Stats.OnMaxManaIncrease += mana => CurrentMana += mana;
    }
    public Properties Stats;
    public float CurrentHealth;
    public float CurrentMana;
    public Enums.DamageType AutoAttackDamageType;

    public abstract Attack AutoAttack();

    public abstract Attack Ability();

}