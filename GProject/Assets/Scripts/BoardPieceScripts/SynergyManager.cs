using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SynergyManager 
{
    public static List<Synergy> Synergies;
    public static List<List<Figure>> FiguresInSynergy;

    public static void AddFigure(Figure figure)
    {
        foreach(Enums.Synergy synergy in figure.Unit.Stats.Synergies)
        {
            FiguresInSynergy[(int)synergy].Add(figure);
        }
    }

    public static void RemoveFigure(Figure figure)
    {
        foreach (Enums.Synergy synergy in figure.Unit.Stats.Synergies)
        {
            FiguresInSynergy[(int)synergy].Remove(figure);
        }
    }

    public static List<Buff> GetBuffsForFigure(Figure figure)
    {
        List<Buff> buffs = new List<Buff>();
        foreach (Enums.Synergy synergy in figure.Unit.Stats.Synergies)
        {
            int cost = 0;
            foreach (Figure f in FiguresInSynergy[(int)synergy])
                cost += f.Cost;
            buffs.Add(Synergies[(int)synergy].GrantBlessing(cost));
        }
        return buffs;
    }

    public static List<Figure> FiguresWithSameSynergies(Figure figure)
    {
        List<Figure> figures = new List<Figure>();
        foreach (Enums.Synergy synergy in figure.Unit.Stats.Synergies)
        {
            figures.AddRange(FiguresInSynergy[(int)synergy]);
        }
        return figures;
    }

    public static void Initialize()
    {
        // To-Do: init
    }
}
