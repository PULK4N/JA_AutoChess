using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class Attack : MonoBehaviour
{
    public Attack(Figure source, TargetingSystem targetingSystem, DamageType damageType, AbiliyType abiliyType,
        float speed, int castingRange, float damage, int abilityRange = 0, int bounceNumber = 0, List<Buff> buffs = null)
    {
        _source = source;
        //_target = target;
        _targeting = targetingSystem;
        _damageType = damageType;
        _abiliyType = abiliyType;
        _speed = speed;
        _castingRange = castingRange;
        _damage = damage;
        _abilityRange = abilityRange;
        _bounceNumber = bounceNumber;
        this.Buffs = buffs;
        Position = source.gameObject.transform.position;
    }

    private TargetingSystem _targeting;

    private DamageType _damageType;

    private AbiliyType _abiliyType;

    private Figure _target;

    private Figure _source;

    private float _speed;

    private int _castingRange;

    private float _damage;

    private int _abilityRange;

    private int _bounceNumber;

    public readonly List<Buff> Buffs;

    public Vector3 Position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float damageReturn = 0;
        if (Position == _target.gameObject.transform.position) // doubt this will work
            _source.DamageFeedback(_damageType, _target.TakeDamage(_damageType, _damage, out damageReturn));
        if (damageReturn > 0)
        {
            float auxiliary;
            _target.DamageFeedback(DamageType.Magical, _source.TakeDamage(DamageType.True, damageReturn, out auxiliary));
        }
    }
}
