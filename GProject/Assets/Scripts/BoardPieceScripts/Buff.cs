using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    public void Initialize(Unit.Properties buffStats, float duration, int maxNumberOfStacks, Enums.DamageType damageType = Enums.DamageType.none)
    {
        _buffStats = buffStats;
        _duration = duration;
        _maxStacks = maxNumberOfStacks;
        _damageType = damageType;
    }

    private Unit.Properties _buffStats;
    public Unit.Properties BuffStats { get => _buffStats; }

    private float _duration;
    private float _startTime;

    private int _maxStacks;

    private Enums.DamageType _damageType;
    public Enums.DamageType DamageType { get => _damageType; }

    public abstract float AddStack();

    public void Commence()
    {
        _startTime = Time.time;
    }

    public delegate void Expire(Buff sender);
    public event Expire OnExpire;

    // Update is called once per frame
    void Update()
    {
        if (_startTime - Time.time > _duration)
        {
            Dispell();
        }
    }

    public void Dispell()
    {
        OnExpire(this);
        Destroy(this.gameObject);
    }
}
