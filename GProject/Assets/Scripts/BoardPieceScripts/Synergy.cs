using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synergy
{
    private Enums.SynergyType SynergyType;
    private List<Buff> _blessings;

    private List<int> _minimumNumberOfUnitsForStage;
    // ETC: stage1=2
    //      stage2=4
    //      stage3=6

    public Enums.SynergyBuffTarget SynergyBuffTarget;

    public Buff GrantBlessing(int unitNumber)
    {
        Buff blessing = null;
        for (int i = 0; i < _minimumNumberOfUnitsForStage.Count; ++i)
        {
            if (_minimumNumberOfUnitsForStage[i] > unitNumber)
                return blessing;
            blessing = _blessings[i];
        }
        return _blessings[_blessings.Count - 1];
    }
}
