using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class BasicEnemy : EnemyBehaviour
    {
        [SerializeField] private EnemyScriptable _enemyInfo;
        [SerializeField] private SpriteRenderer _tierSprite;

        public override EnemyScriptable EnemyInfo { get { return _enemyInfo; } }
        protected override SpriteRenderer TierRenderer { get { return _tierSprite; } }
        protected override float MaxHealth { get; set; }
        protected override float CurrentHealth { get; set; }
        protected override int Tier { get; set; }

        public override void Setup(EnemySetup enemySetup)
        {
            MaxHealth = _enemyInfo.HealthMultiplier * enemySetup.Health;
            _speed = _enemyInfo.SpeedMultiplier * enemySetup.Speed;
            _globalData = enemySetup.GlobalData;
            _player = enemySetup.Target;
            Tier = enemySetup.Tier;
            CurrentHealth = MaxHealth;

            Startup();
            SetTierIcon();
        }

        protected override void Die()
        {
            if (CurrentHealth <= 0)
            {
                _globalData.AddPoints(_enemyInfo.PointValue);
                networkObject.Destroy();
            }
        }
    }
}