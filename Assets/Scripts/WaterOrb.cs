using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrb : OrbBehaviour, IOrb {

    [SerializeField] private BoxCollider2D _Collider;
    [SerializeField] private Animator _Anim;
    [SerializeField] private LineRenderer _AimLine;
    [SerializeField] private LayerMask _Mask;
    private float _Distance;

    public int Damage { get; private set; }
    public float IdleDelay { get; private set; } = 3f;
    public float MainAttackDelay { get; private set; } = 1f;
    public float SecondaryAttackDelay { get; private set; } = 2f;

	void Start () {
        _Collider = GetComponent<BoxCollider2D>();
        _Anim = GetComponent<Animator>();
        transform.localScale = _GlobalData.OrbSize;
        _Orb = this;
        _AimLine = GetComponent<LineRenderer>();
	}

    public void Setup(Vector2 offset, Transform player, GlobalDataHandler globalData, bool isIdle, float[] mainTimers, float[] secondTimers, int orbType)
    {
        _Player = player;
        _Offset = offset;
        _GlobalData = globalData;
        _IsIdle = isIdle;
        _MainAttackTimers = mainTimers;
        _SecondaryAttackTimers = secondTimers;
        _OrbType = orbType;
        Debug.Log($"Timers: Water {secondTimers[0]}\tFire {secondTimers[1]}");
        Startup();
    }

    public void SetIdle()
    {
        Damage = 10;
        _BeganAim = false;
        _IsAttacking = false;
        _AimLine.enabled = false;
    }

    public void MainAttack()
    {
        GetComponent<LineRenderer>().enabled = false;
        Damage = Mathf.FloorToInt(20 * _GlobalData.OrbDamage);
        transform.position += transform.up * 1.5f;
        ResetCollider();
        _Anim.SetTrigger("Move");
    }

    public void SecondaryAttack()
    {
        _AimLine.enabled = false;
        Damage = Mathf.RoundToInt(40 * _GlobalData.OrbDamage);
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
        _AimLine.enabled = true;
        _AimLine.SetPosition(1, new Vector3(0, _Distance, 0));
        _BeganAim = true;
    }

    public void UpdateAimLine()
    {
        transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
        _Distance = Mathf.Clamp(MouseDistance(), 2.5f, _GlobalData.OrbDistance * 5f);
        if (!_BeganAim) ActivateAimLine();
        _AimLine.SetPosition(1, new Vector3(0, _Distance, 0));
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
        transform.position += transform.up * gap * _Distance / 5f;
    }

    public void CanAttack()
    {
        _IsAttacking = false;
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
            collision.GetComponent<EnemyBehaviour>().TakeDamage(Damage);
        }
    }
}
