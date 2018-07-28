using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrb : OrbBehaviour, IOrb {

    [SerializeField] private BoxCollider2D _Collider;
    [SerializeField] private Animator _Anim;
    [SerializeField] private float _Distance;
    [SerializeField] private LineRenderer _AimLine;
    [SerializeField] private LayerMask _Mask;

    public int Damage { get; private set; }
    public float IdleDelay { get; private set; } = 3f;
    public float SecondaryAttackDelay { get; private set; } = 2f;

	void Start () {
        _Collider = GetComponent<BoxCollider2D>();
        _Anim = GetComponent<Animator>();
        transform.localScale = WorldSettings.OrbSize;
        _Orb = this;
        _AimLine = GetComponent<LineRenderer>();
	}

    public void Setup(Vector2 offset, Transform player)
    {
        _Player = player;
        _Offset = offset;
    }

    public void MainAttack()
    {
        GetComponent<LineRenderer>().enabled = false;
        Damage = Mathf.FloorToInt((2 + _Distance * 5) * WorldSettings.OrbDamage);
        transform.position += transform.up * 1.5f;
        ResetCollider();
        _Anim.SetTrigger("Move");
    }

    public void SecondaryAttack()
    {
        GetComponent<LineRenderer>().enabled = false;
        Damage = Mathf.RoundToInt(2 * WorldSettings.OrbDamage);
        ResetCollider();
        _Anim.SetTrigger("Pull");
        var enemiesHit = Physics2D.OverlapCircleAll(transform.position, 2f, _Mask);
        foreach (Collider2D hit in enemiesHit)
        {
            hit.GetComponent<EnemyBehaviour>().KnockBack(10f, (transform.position - hit.transform.position).normalized);
        }
    }

    public void ActivateAimLine()
    {
        _Distance = 1.5f;
        _AimLine.enabled = true;
        //_AimLine.SetPosition(1, new Vector3(0, _Distance * 5 / WorldSettings.OrbSize.x, 0));
        _BeganAim = true;
    }

    public void UpdateAimLine()
    {
        if (!_BeganAim) ActivateAimLine();
        //_Distance = Mathf.Clamp(_Distance + Time.deltaTime * WorldSettings.ChargeRate, 0.5f, 3f);
        _AimLine.SetPosition(1, new Vector3(0, _Distance * 5 / WorldSettings.OrbSize.x, 0));
    }

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
        transform.position += transform.up * gap * _Distance;
    }

    public void CanAttack()
    {
        _IsAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBehaviour>().TakeDamage(Damage);
        }
    }
}
