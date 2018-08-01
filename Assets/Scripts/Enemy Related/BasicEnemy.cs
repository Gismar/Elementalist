using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBehaviour, IEnemy {
    [SerializeField] private EnemyScriptable _EnemyInfo;
    [SerializeField] private SpriteRenderer _TierSprite;

    public EnemyScriptable EnemyInfo { get { return _EnemyInfo; } }
    public SpriteRenderer TierRenderer { get { return _TierSprite; } }
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
        _Enemy = this;
        Startup();
        _Speed = _EnemyInfo.SpeedMultiplier * speed;
        _Player = target;
        MaxHealth = _EnemyInfo.HealthMultiplier * health;
        CurrentHealth = MaxHealth;
        Tier = tier;
        SetTierIcon();
        _GlobalData = globalData;
    }

    public void Die()
    {
        if (CurrentHealth <= 0)
        {
            _GlobalData.Points += _EnemyInfo.PointValue;
            Destroy(transform.gameObject);
        }
    }

    
}
