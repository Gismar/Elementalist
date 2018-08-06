using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBehaviour, IEnemy {
    [SerializeField] private EnemyScriptable _enemyInfo;
    [SerializeField] private SpriteRenderer _tierSprite;

    public EnemyScriptable EnemyInfo { get { return _enemyInfo; } }
    public SpriteRenderer TierRenderer { get { return _tierSprite; } }
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; set; }
    public bool IsDrenched { get; set; }
    public float DrenchDuration { get; set; }
    public bool IsSlowed { get; set; }
    public float SlowDuration { get; set; }
    public float SlowStrength { get; set; }
    public bool IsKnockedBack { get; set; }
    public bool IsStunned { get; set; }
    public float StunDuration { get; set; }
    public bool IsInvincible { get; set; }
    public float InvincibilityTimer { get; set; }
    public int Tier { get; set; }

    public void Setup(float speed, Transform target, float health, int tier, GlobalDataHandler globalData)
    {
        _enemy = this;
        Startup();
        _speed = _enemyInfo.SpeedMultiplier * speed;
        _player = target;
        MaxHealth = _enemyInfo.HealthMultiplier * health;
        CurrentHealth = MaxHealth;
        Tier = tier;
        SetTierIcon();
        _globalData = globalData;
    }

    public void Die()
    {
        if (CurrentHealth <= 0)
        {
            _globalData.AddPoints(_enemyInfo.PointValue);
            Destroy(transform.gameObject);
        }
    }

    
}
