using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthOrb : OrbBehaviour{
    [SerializeField] private GameObject _mainAttackPrefab;
    [SerializeField] private GameObject _secondaryAttackPrefab;
    [SerializeField] private Animator _animator;

    private LineRenderer _aimLine;
    protected override float _damage { get; set; }
    protected override float _mainAttackDelay { get; } = 3f;
    protected override float _secondaryAttackDelay { get; } = 10f;

    void Start ()
    {
        _aimLine = GetComponentInChildren<LineRenderer>();
        _animator = GetComponent<Animator>();
    }
    protected override void Setup(OrbSetup orbSetup)
    {
        _decay = orbSetup.Decay;
        _player = orbSetup.Player;
        _globalData = orbSetup.GlobalData;
        _state = orbSetup.OrbState;
        _mainAttackTimers = orbSetup.MainAttackTimers;
        _secondaryAttackTimers = orbSetup.SecondaryAttackTimers;
        _orbType = orbSetup.OrbType;
        Startup();
    }

    protected override void MainAttack()
    {
        UpdateAimLine();
        _animator.SetTrigger("Attack");
        _aimLine.enabled = false;
        _damage = 10 * _globalData.OrbDamage;
        SpawnRavine(_damage, MouseAngle());
        _state = State.Idling;
    }
    protected override void SecondaryAttack()
    {
        _animator.SetTrigger("Attack");
        SpawnEarthquake(1 / (_globalData.OrbDamage + 1), _globalData.OrbSize * 2f * transform.localScale);
    }

    protected override void UpdateAimLine()
    {
        _aimLine.enabled = true;
        _aimLine.transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
        _aimLine.SetPosition(1, new Vector3(0, 5f * _globalData.OrbDistance, 0));
    }

    private void SpawnRavine(float damage, float rotation)
    {
        var temp = Instantiate(_mainAttackPrefab);
        temp.transform.position = transform.position;
        temp.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * 5f * _globalData.OrbDistance);
        temp.transform.rotation = Quaternion.Euler(0, 0, rotation);
        temp.GetComponent<EarthSplit>().Setup(damage, 2.5f);
    }

    private void SpawnEarthquake(float strength, Vector2 size)
    {
        var temp = Instantiate(_secondaryAttackPrefab);
        temp.transform.position = transform.position;
        temp.transform.localScale = size;
        temp.GetComponent<Earthquake>().Setup(strength, 5f);
    }
}
