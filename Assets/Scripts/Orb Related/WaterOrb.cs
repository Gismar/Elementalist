using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrb : OrbBehaviour{
    [SerializeField] private LayerMask _mask;
    private BoxCollider2D _collider;
    private Animator _animator;
    private LineRenderer _aimLine;
    private Rigidbody2D _rigidBody;
    private float _distance;
    private AnimationCurve _movementCurve;
    
    protected override float _damage { get; set; }
    protected override float _mainAttackDelay { get; } = 0.5f;
    protected override float _secondaryAttackDelay { get; } = 2f;

    void Start () {
        _collider = GetComponent<BoxCollider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _aimLine = GetComponent<LineRenderer>();
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
        _damage = 5;
        Startup();
    }

    protected override void MainAttack()
    {
        UpdateAimLine();
        GetComponent<LineRenderer>().enabled = false;
        _damage = 15 * _globalData.OrbDamage;
        transform.position += transform.up;
        _movementCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, _distance / 6f));
        ResetCollider();
        _animator.SetTrigger("Move");
    }

    protected override void SecondaryAttack()
    {
        _damage = 20 * _globalData.OrbDamage;
        ResetCollider();
        _animator.SetTrigger("Pull");
        var enemiesHit = Physics2D.OverlapCircleAll(transform.position, 2f * transform.localScale.x, _mask);
        foreach (Collider2D hit in enemiesHit)
        {
            hit.GetComponent<IEnemy>().KnockBack(5f * Vector3.Distance(transform.position, hit.transform.position), 
                                                        (transform.position - hit.transform.position).normalized);
        }
    }

    protected override void UpdateAimLine()
    {
        transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
        _distance = Mathf.Clamp(MouseDistance(), 2f, _globalData.OrbDistance * 5f);

        _aimLine.enabled = true;
        _aimLine.SetPosition(1, new Vector3(0, _distance / _globalData.OrbSize.x, 0));
        _aimLine.startWidth = transform.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<IEnemy>();
            enemy.TakeDamage(_damage);
        }
    }
    
    private void ResetCollider()
    {
        _collider.size = new Vector2(0, 0);
        _collider.offset = new Vector2(0, 0);
    }

    public void UpdateBoxCollider(string size)
    {
        var sizes = Array.ConvertAll(size.Split(','), float.Parse);
        _collider.offset = new Vector2(0, sizes[0]);
        _collider.size = new Vector2(sizes[1], sizes[2]);
    }

    public void Move(float time)
    {
        transform.position += transform.up * _movementCurve.Evaluate(time);
    }

    public void CanAttack()
    {
        _damage = 10;
        _state = State.Idling;
    }
}
