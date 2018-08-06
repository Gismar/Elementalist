using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrb : OrbBehaviour, IOrb {

    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Animator _animator;
    [SerializeField] private LineRenderer _aimLine;
    [SerializeField] private LayerMask[] _mask;
    private Rigidbody2D _rigidBody;
    private float _distance;

    public float Damage { get; private set; }
    public float MainAttackDelay { get; private set; } = 0.5f;
    public float SecondaryAttackDelay { get; private set; } = 2f;

	void Start () {
        _collider = GetComponent<BoxCollider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        transform.localScale = _globalData.OrbSize;
        _orb = this;
        _aimLine = GetComponent<LineRenderer>();
	}

    #region Main Functionality
    public void Setup(Vector2 offset, Transform player, GlobalDataHandler globalData, bool isIdle, float[] mainTimers, float[] secondTimers, int orbType)
    {
        _player = player;
        _offset = offset;
        _globalData = globalData;
        _isIdle = isIdle;
        _mainAttackTimers = mainTimers;
        _secondaryAttackTimers = secondTimers;
        _orbType = orbType;
        Damage = 5;
        Startup();
    }

    public void SetIdle()
    {
        Damage = 5;
        _beganAim = false;
        _isAttacking = false;
        _aimLine.enabled = false;
    }

    public void MainAttack()
    {
        UpdateAimLine();
        GetComponent<LineRenderer>().enabled = false;
        Damage = 15 * _globalData.OrbDamage;
        transform.position += transform.up;
        _distance -= 1f;
        ResetCollider();
        _animator.SetTrigger("Move");
    }

    public void SecondaryAttack()
    {
        _aimLine.enabled = false;
        Damage = 20 * _globalData.OrbDamage;
        ResetCollider();
        _animator.SetTrigger("Pull");
        var enemiesHit = Physics2D.OverlapCircleAll(transform.position, 2f * _globalData.OrbSize.x, _mask[0]);
        foreach (Collider2D hit in enemiesHit)
        {
            hit.GetComponent<IEnemy>().KnockBack(5f * Vector3.Distance(transform.position, hit.transform.position), 
                                                        (transform.position - hit.transform.position).normalized);
        }
    }

    public void ActivateAimLine()
    {
        _aimLine.enabled = true;
        _aimLine.SetPosition(1, new Vector3(0, _distance, 0));
    }

    public void UpdateAimLine()
    {
        transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
        _distance = Mathf.Clamp(MouseDistance(), 2f, _globalData.OrbDistance * 5f);
        if (!_beganAim) ActivateAimLine();
        _aimLine.SetPosition(1, new Vector3(0, _distance / _globalData.OrbSize.x, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<IEnemy>();
            enemy.TakeDamage(Damage);
        }
    }
    #endregion

    #region Additional Functions
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

    public void Move(float gap)
    {
        transform.position += transform.up * gap * _distance / 4f;
    }

    public void Reposition()
    {
        var orbHit = Physics2D.OverlapCircleAll(transform.position, _globalData.OrbSize.x/2f, _mask[1]);
        if (orbHit.Length < 2) return;
        Vector3 averagePos = Vector3.zero;
        foreach( Collider2D hit in orbHit)
        {
            averagePos += hit.transform.position;
        }
        averagePos = Vector3.Distance(averagePos, transform.position) < 0.5f ? new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) : averagePos / orbHit.Length;
        _rigidBody.velocity = (transform.position - averagePos).normalized * 10f;
        StartCoroutine(Knockback());
    }

    private IEnumerator Knockback()
    {
        while (_rigidBody.velocity.magnitude != 0)
        {
            _rigidBody.velocity = _rigidBody.velocity.magnitude >= 0.1f ? _rigidBody.velocity * 0.9f : Vector2.zero;
            yield return null;
        }
    }

    public void CanAttack()
    {
        Damage = 10;
        _isAttacking = false;
    }
    #endregion
}
