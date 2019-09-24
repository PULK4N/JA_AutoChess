public class Enums
{
    public enum GameStates
    {
        Menu,
        Play,
        Defeat,
        Victory
    }
    public enum Levels
    {
        Menu,
        BattleZone
    }

    public enum GameTypes
    {
        OneVsOne,
        EightVsEight,
        PVE,
        None
    }

    public enum Mythology
    {

    }

    public enum Deity
    {



    }

    public enum TargetingSystem
    {
        Target,
        ClosestEnemy,
        HighestEnemyDps,
        LowestAllyHp,
        Self
    }

    public enum DamageType
    {
        Physical,
        Magical,
        True
    }

    public enum AbiliyType
    {
        Target,
        AreaOfEffect,
        Bouncing,
        DamageBehindEnemy
    }
}
