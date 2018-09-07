using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;
using System.Linq;

namespace Enemy
{
    public class SpawnerEnemy : EnemyBehaviour
    {
        [SerializeField] private EnemyScriptable _enemyInfo;
        [SerializeField] private SpriteRenderer _tierSprite;
        [SerializeField] private GameObject _offsprings;

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
            CurrentHealth = MaxHealth;
            Tier = enemySetup.Tier;

            Startup();
            SetTierIcon();
        }

        protected override void Die()
        {
            if (CurrentHealth <= 0)
            {
                SpawnChildren();
                _globalData.AddPoints(_enemyInfo.PointValue);
            }
        }

        private void SpawnChildren()
        {
            for (int i = 0; i < _enemyInfo.SpawnAmount; i++)
            {
                var enemyPrefabs = NetworkManager.Instance.EnemyNetworkingNetworkObject;

                var temp = NetworkManager.Instance.InstantiateEnemyNetworking(
                    System.Array.IndexOf(enemyPrefabs, enemyPrefabs.First(x => x.name == _offsprings.name)), 
                    transform.position, 
                    Quaternion.Euler(Vector2.up));

                var angle = Random.Range(-1f, 1f);
                var setup = new EnemySetup(_speed, _player, MaxHealth, Tier, _globalData);

                temp.GetComponent<EnemyBehaviour>().Setup(setup);
                temp.GetComponent<EnemyBehaviour>().KnockBack(10f, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized);
                temp.GetComponent<EnemyBehaviour>().Debuffs[DebuffType.Knockback].IsAffected = true;
            }
            networkObject.Destroy();
            Destroy(transform.gameObject);
        }
    }
}
