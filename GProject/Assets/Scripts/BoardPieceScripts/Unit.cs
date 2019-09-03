public abstract class Unit
{
    public struct Properties
    {
        public float Range;
        public float AttackSpeed;
        public float AttackDamage;
        public float AbilityPower;

        public float Health;
        public float Mana;
        public float Armor;
        public float MagicResist;
        public float Shield;

        public float LifeSteal;
        public float SpellWamp;
        //for special cases
        public byte IsInvounrable;

        public static Properties operator +(Properties property, Properties buff)
        {
            property.Range += buff.Range;
            property.AttackSpeed += buff.AttackSpeed;
            property.AttackDamage += buff.AttackDamage;
            property.AbilityPower += buff.AbilityPower;

            property.Health += buff.Health;
            property.Mana += buff.Mana;
            property.Armor += buff.Armor;
            property.MagicResist += buff.MagicResist;
            property.Shield += buff.Shield;

            property.LifeSteal += buff.LifeSteal;
            property.SpellWamp += buff.SpellWamp;

            property.IsInvounrable++;

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

            property.LifeSteal -= buff.LifeSteal;
            property.SpellWamp -= buff.SpellWamp;

            property.IsInvounrable--;

            return property;
        }
    }
    public Ability Ability;

    
}