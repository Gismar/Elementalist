using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    protected IEnemy _Enemy;
    protected Transform _Player;
    protected float _Speed;
    protected Rigidbody2D _RB;
    protected GlobalDataHandler _GlobalData;

    protected void Startup()
    {
        _RB = GetComponent<Rigidbody2D>();
        _Enemy.IsSlowed = false;
        _Enemy.IsDrenched = false;
        _Enemy.IsStunned = false;
        _Enemy.IsKnockedBack = false;
        _Enemy.SlowStrength = 1f;
        _Enemy.InvincibilityTimer = Time.time + 0.5f;
        _Enemy.IsInvincible = true;
    }

    private void Update()
    {
        if (_Enemy.IsKnockedBack)
        {
            _RB.velocity = _RB.velocity.magnitude >= 0.1f ? _RB.velocity * 0.9f : Vector2.zero;
            _Enemy.IsKnockedBack = _RB.velocity.magnitude == 0 ? false : true;
        }

        if(_Enemy.IsInvincible)
        {
            _Enemy.IsInvincible = Time.time > _Enemy.InvincibilityTimer ? false : true;
            GetComponent<SpriteRenderer>().color = _Enemy.IsInvincible ?
                new Color(_Enemy.EnemyInfo.BaseColor.r, _Enemy.EnemyInfo.BaseColor.g, _Enemy.EnemyInfo.BaseColor.b, 0.25f)
                : _Enemy.EnemyInfo.BaseColor;
        }

        if (_Enemy.IsStunned)
        {
            _Enemy.IsStunned = Time.time > _Enemy.StunDuration ? false : true;
            return;
        }

        var direction = (_Player.position - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.position += transform.up * Time.deltaTime * _Speed * _Enemy.SlowStrength;
        if (_Enemy.IsSlowed)
        {
            _Enemy.IsSlowed = Time.time > _Enemy.SlowDuration ? false : true;
            _Enemy.SlowStrength = _Enemy.IsSlowed ? _Enemy.SlowStrength : 1f;
            Debug.Log($"Enemy is Slowed ({_Enemy.IsSlowed}) by {1 - _Enemy.SlowStrength} for {_Enemy.SlowDuration - Time.time}");
        }

        if (_Enemy.IsDrenched)
            _Enemy.IsDrenched = Time.time > _Enemy.DrenchDuration ? false : true;
    }

    public void KnockBack(float strength, Vector2 direction)
    {
        _RB.velocity = direction * strength;
        _Enemy.IsKnockedBack = true;
    }

    public void TakeDamage(float dmg)
    {
        if (Time.time < _Enemy.InvincibilityTimer) return;
        _Enemy.CurrentHealth -= dmg;
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, _Enemy.EnemyInfo.BaseColor, _Enemy.CurrentHealth / _Enemy.MaxHealth);
        _Enemy.Die();
    }

    public void SetTierIcon()
    {
        _Enemy.TierRenderer.color = _Enemy.EnemyInfo.TierColors.Evaluate((_Enemy.Tier % 7) / 7f);
        _Enemy.TierRenderer.sprite = _Enemy.EnemyInfo.Tiers[Mathf.FloorToInt(_Enemy.Tier / 7f)];
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            KnockBack(20f, -transform.up);
            if (_Enemy.IsInvincible) return;
            collision.transform.GetComponent<PlayerMovement>().TakeDamage(1);
        }
    }
}
