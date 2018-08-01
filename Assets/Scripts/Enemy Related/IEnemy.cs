using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {
    void Setup(float speed, Transform target, float health, int tier, GlobalDataHandler globalData);
    void Die();
    void TakeDamage(float dmg);
    void KnockBack(float strength, Vector2 direction);
    void SetTierIcon();

    EnemyScriptable EnemyInfo { get; }
    SpriteRenderer TierRenderer { get; }
    float MaxHealth { get; }
    float CurrentHealth { get; set; }
    bool IsDrenched { get; set; }
    float DrenchDuration { get; set; }
    bool IsSlowed { get; set; }
    float SlowDuration { get; set; }
    float SlowStrength { get; set; }
    bool IsKnockedBack { get; set; }
    bool IsStunned { get; set; }
    float StunDuration { get; set; }
    bool IsInvincible { get; set; }
    float InvincibilityTimer { get; set; }
    int Tier { get; set; }
}
