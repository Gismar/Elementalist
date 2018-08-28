using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    protected IEnemy _enemy;
    protected Transform _player;
    protected float _speed;
    protected Rigidbody2D _rigidBody;
    protected GlobalDataHandler _globalData;

    protected void Startup()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _enemy.IsSlowed = false;
        _enemy.IsDrenched = false;
        _enemy.IsStunned = false;
        _enemy.IsKnockedBack = false;
        _enemy.SlowStrength = 1f;
        _enemy.InvincibilityTimer = Time.time + 0.5f;
        _enemy.IsInvincible = true;
    }

    private void Update()
    {
        CheckAndDoKnockback();
        CheckAndDoInvicibility();

        if (_enemy.IsStunned)
        {
            _enemy.IsStunned = Time.time > _enemy.StunDuration ? false : true;
            return;
        }

        MoveToPlayer();

        CheckAndDoSlow();
        CheckAndDoDrench();
    }

    public void KnockBack(float strength, Vector2 direction)
    {
        _rigidBody.velocity = direction * strength;
        _enemy.IsKnockedBack = true;
    }

    private void MoveToPlayer()
    {
        var direction = (_player.position - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.position += transform.up * Time.deltaTime * _speed * _enemy.SlowStrength;
    }

    public void TakeDamage(float dmg)
    {
        if (Time.time < _enemy.InvincibilityTimer) return;
        _enemy.CurrentHealth -= dmg;
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, _enemy.EnemyInfo.BaseColor, _enemy.CurrentHealth / _enemy.MaxHealth);
        _enemy.Die();
    }

    public void SetTierIcon()
    {
        _enemy.TierRenderer.color = _enemy.EnemyInfo.TierColors.Evaluate((_enemy.Tier % 7) / 7f);
        _enemy.TierRenderer.sprite = _enemy.EnemyInfo.Tiers[Mathf.FloorToInt(_enemy.Tier / 7f)];
    }

    #region Check and Do Status Effects
    private void CheckAndDoDrench()
    {
        if (_enemy.IsDrenched)
        {
            _enemy.IsDrenched = Time.time > _enemy.DrenchDuration ? false : true;
        }
    }

    private void CheckAndDoSlow()
    {
        if (_enemy.IsSlowed)
        {
            _enemy.IsSlowed = Time.time > _enemy.SlowDuration ? false : true;
            _enemy.SlowStrength = _enemy.IsSlowed ? _enemy.SlowStrength : 1f;
        }
    }

    private void CheckAndDoInvicibility()
    {
        if (_enemy.IsInvincible)
        {
            _enemy.IsInvincible = Time.time > _enemy.InvincibilityTimer ? false : true;
            GetComponent<SpriteRenderer>().color = _enemy.IsInvincible ?
                new Color(_enemy.EnemyInfo.BaseColor.r, _enemy.EnemyInfo.BaseColor.g, _enemy.EnemyInfo.BaseColor.b, 0.25f)
                : _enemy.EnemyInfo.BaseColor;
        }
    }

    private void CheckAndDoKnockback()
    {
        if (_enemy.IsKnockedBack)
        {
            _rigidBody.velocity = _rigidBody.velocity.magnitude >= 0.1f ? _rigidBody.velocity * 0.9f : Vector2.zero;
            _enemy.IsKnockedBack = _rigidBody.velocity.magnitude == 0 ? false : true;
        }
    }
    #endregion

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            KnockBack(20f, -transform.up);
            if (_enemy.IsInvincible) return;
            collision.transform.GetComponent<PlayerMovement>().TakeDamage(1);
        }
    }
}
