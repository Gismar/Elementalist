using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrb : OrbBehaviour, IOrb {

    [SerializeField] private BoxCollider2D _Collider;
    [SerializeField] private Animator _Anim;
    [SerializeField] private LineRenderer _AimLine;
    [SerializeField] private LayerMask[] _Mask;
    private Rigidbody2D _RB;
    private float _Distance;

    public float Damage { get; private set; }
    public float IdleDelay { get; private set; } = 3f;
    public float MainAttackDelay { get; private set; } = 0.5f;
    public float SecondaryAttackDelay { get; private set; } = 2f;

	void Start () {
        _Collider = GetComponent<BoxCollider2D>();
        _RB = GetComponent<Rigidbody2D>();
        _Anim = GetComponent<Animator>();
        transform.localScale = _GlobalData.OrbSize;
        _Orb = this;
        _AimLine = GetComponent<LineRenderer>();
	}

    #region //Main Functionality
    public void Setup(Vector2 offset, Transform player, GlobalDataHandler globalData, bool isIdle, float[] mainTimers, float[] secondTimers, int orbType)
    {
        _Player = player;
        _Offset = offset;
        _GlobalData = globalData;
        _IsIdle = isIdle;
        _MainAttackTimers = mainTimers;
        _SecondaryAttackTimers = secondTimers;
        _OrbType = orbType;
        Damage = 5;
        Startup();
    }

    public void SetIdle()
    {
        Damage = 5;
        _BeganAim = false;
        _IsAttacking = false;
        _AimLine.enabled = false;
    }

    public void MainAttack()
    {
        UpdateAimLine();
        GetComponent<LineRenderer>().enabled = false;
        Damage = 15 * _GlobalData.OrbDamage;
        transform.position += transform.up;
        _Distance -= 1f;
        ResetCollider();
        _Anim.SetTrigger("Move");
    }

    public void SecondaryAttack()
    {
        _AimLine.enabled = false;
        Damage = 20 * _GlobalData.OrbDamage;
        ResetCollider();
        _Anim.SetTrigger("Pull");
        var enemiesHit = Physics2D.OverlapCircleAll(transform.position, 2f * _GlobalData.OrbSize.x, _Mask[0]);
        foreach (Collider2D hit in enemiesHit)
        {
            hit.GetComponent<IEnemy>().KnockBack(5f * Vector3.Distance(transform.position, hit.transform.position), 
                                                        (transform.position - hit.transform.position).normalized);
        }
    }

    public void ActivateAimLine()
    {
        _AimLine.enabled = true;
        _AimLine.SetPosition(1, new Vector3(0, _Distance, 0));
    }

    public void UpdateAimLine()
    {
        transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
        _Distance = Mathf.Clamp(MouseDistance(), 2f, _GlobalData.OrbDistance * 5f);
        if (!_BeganAim) ActivateAimLine();
        _AimLine.SetPosition(1, new Vector3(0, _Distance / _GlobalData.OrbSize.x, 0));
    }

    public void Swap(int orbType)
    {
        switch (orbType)
        {
            case 1:
                var temp = Instantiate(_Player.GetComponent<PlayerMovement>().Orbs[orbType]);
                temp.transform.position = transform.position;
                temp.GetComponent<IOrb>().Setup(_Offset, _Player, _GlobalData, _IsIdle, _MainAttackTimers, _SecondaryAttackTimers, orbType);
                Destroy(transform.gameObject);
                break;
        }
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

    #region //Additional Functions
    private void ResetCollider()
    {
        _Collider.size = new Vector2(0, 0);
        _Collider.offset = new Vector2(0, 0);
    }

    public void UpdateBoxCollider(string size)
    {
        var sizes = Array.ConvertAll(size.Split(','), float.Parse);
        _Collider.offset = new Vector2(0, sizes[0]);
        _Collider.size = new Vector2(sizes[1], sizes[2]);
    }

    public void Move(float gap)
    {
        transform.position += transform.up * gap * _Distance / 4f;
    }

    public void Reposition()
    {
        var orbHit = Physics2D.OverlapCircleAll(transform.position, _GlobalData.OrbSize.x/2f, _Mask[1]);
        if (orbHit.Length < 2) return;
        Vector3 averagePos = Vector3.zero;
        foreach( Collider2D hit in orbHit)
        {
            averagePos += hit.transform.position;
        }
        averagePos = Vector3.Distance(averagePos, transform.position) < 0.5f ? new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) : averagePos / orbHit.Length;
        _RB.velocity = (transform.position - averagePos).normalized * 10f;
        StartCoroutine(Knockback());
    }

    private IEnumerator Knockback()
    {
        while (_RB.velocity.magnitude != 0)
        {
            _RB.velocity = _RB.velocity.magnitude >= 0.1f ? _RB.velocity * 0.9f : Vector2.zero;
            yield return null;
        }
    }

    public void CanAttack()
    {
        Damage = 10;
        _IsAttacking = false;
    }
    #endregion
}
