using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : EnemyBehaviour, IEnemy {
    [SerializeField] private EnemyScriptable _EnemyInfo;
    [SerializeField] private SpriteRenderer _TierSprite;
    [SerializeField] private GameObject _Offsprings;

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
            SpawnChildren();
            _GlobalData.Points += _EnemyInfo.PointValue;
        }
    }

    private void SpawnChildren()
    {
        for (int i =0; i < _EnemyInfo.SpawnAmount; i++)
        {
            var temp = Instantiate(_Offsprings, transform.parent);
            var angle = Random.Range(-1f, 1f);
            temp.transform.position = transform.position;
            temp.GetComponent<IEnemy>().Setup(_Speed, _Player, MaxHealth, Tier, _GlobalData);
            temp.GetComponent<IEnemy>().KnockBack(10f, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized);
            temp.GetComponent<IEnemy>().IsKnockedBack = true;
        }
        Destroy(transform.gameObject);
    }

    
}
