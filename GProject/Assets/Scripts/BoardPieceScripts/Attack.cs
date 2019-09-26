using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class Attack : MonoBehaviour
{
    public Attack(Figure source, TargetingSystem targetingSystem, DamageType damageType,
        float projectileSpeed, int range, float projectileDamage, Spell spellWhichThisProjectileCarry = null, List<Buff> buffs = null)
    {
        _source = source;
        _targeting = targetingSystem;
        _damageType = damageType;
        _projectileSpeed = projectileSpeed;
        _range = range;
        _projectileDamage = projectileDamage;
        _spell = spellWhichThisProjectileCarry;
        this.Buffs = buffs;
        Position = source.gameObject.transform.position;
        _startingTime = Time.realtimeSinceStartup;
    }

    private TargetingSystem _targeting;
    private DamageType _damageType;
    private Figure _target;
    private Figure _source;
    private float _projectileSpeed;
    private int _range;
    public int Range { get => _range; }
    private float _projectileDamage;

    private Spell _spell;
    public readonly List<Buff> Buffs;

    public Vector3 Position;
    private float _startingTime;


    // Start is called before the first frame update
    void Start()
    {
        switch(_targeting)
        {
            case TargetingSystem.Target:
                _target = _source.Target;
                break;
            case TargetingSystem.Self:
                _target = _source;
                break;
            case TargetingSystem.ClosestEnemy:
            case TargetingSystem.HighestEnemyDps:
            case TargetingSystem.LowestAllyHp:
            default:
                _target = Dijkstra.EnemyInsideRange(_source, _range, -1, -1, _targeting);
                break;
        }

        if (_projectileSpeed == 0)
            this.Position = _target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float damageReturn = 0;
        Position = new Vector3(
            (_target.transform.position.x - _source.transform.position.x) * _projectileSpeed * ((Time.realtimeSinceStartup - _startingTime) / 1000),
            Position.y,
            (_target.transform.position.x - _source.transform.position.x) * _projectileSpeed * ((Time.realtimeSinceStartup - _startingTime) / 1000));

        if (Position == _target.gameObject.transform.position) // doubt this will work
        {
            _source.DamageFeedback(_damageType, _target.TakeDamage(_damageType, _projectileDamage, out damageReturn, Buffs));
            _spell.Activate();
            if (damageReturn > 0)
            {
                _target.DamageFeedback(DamageType.Magical, _source.TakeDamage(DamageType.True, damageReturn));
            }
            Destroy(this);
        }
    }
}
