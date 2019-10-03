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

    public enum Synergy
    {
        none




    }

    public enum SynergyType
    {
        Mythology,
        Deity
    }

    public enum SynergyBuffTarget
    {
        AllAllies,
        SynergyHolders
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
        none,
        Physical,
        Magical,
        True
    }

    public enum MatchState
    {
        Preparation,
        Battle
    }

    public enum Piece
    {
        none,
        Pawn,
        Bishop,
        Knight,
        Rook,
        Queen
    }
}
