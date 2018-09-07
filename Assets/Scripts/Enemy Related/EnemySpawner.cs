using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;

namespace Enemy
{
    public class EnemySpawner : EnemySpawnerBehavior
    {
        public List<Transform> Players { get; set; }
        private Tilemap _map;
        private GlobalDataHandler _globalData;
        private float _spawnTimer;
        private float _spawnDelay;
        private float _multiplier;
        private float _multiplierTimer;
        private float _time;
        private int _pointMultiplier;
        private int _tier;

        void Start()
        {
            _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Tilemap>();
            _map.CompressBounds();
            _time = 0;
            _spawnTimer = 0;
            _multiplierTimer = _time + 30f;
            _multiplier = 1f;
            _pointMultiplier = 1;
        }

        void Update()
        {
            if (Players.Count == 0)
                LookForPlayers();

            if (!networkObject.IsServer)
            {
                return;
            }

            _time = Mathf.Floor(Time.timeSinceLevelLoad / 10f) * 10f;
            _tier = Mathf.FloorToInt(_time / 10f);

            if (_spawnTimer < Time.timeSinceLevelLoad)
            {
                SpawnEnemy(GetEnemyCount()-1);

                _spawnDelay = 1 / Mathf.Log(Mathf.Pow(_time + 10, 2), 100f);
                _spawnTimer = Time.timeSinceLevelLoad + _spawnDelay;
            }

            if (_multiplierTimer <= _time)
            {
                UpdateMultiplier();
            }

        }

        private int GetEnemyCount()
        {
            var count = 0;
            var enemyNetworkObject = NetworkManager.Instance.EnemyNetworkingNetworkObject;
            for (int i = 0; i < enemyNetworkObject.Length; i++)
            {
                var enemy = ((GameObject)enemyNetworkObject.GetValue(i)).GetComponent<EnemyBehaviour>();

                if(_tier * 10f >= enemy.EnemyInfo.StartingSpawnTime) count++;

            }
            return count;
        }

        private void UpdateMultiplier()
        {
            _multiplierTimer = _time + 30f;
            _multiplier += 0.3f;
            _pointMultiplier++;
            _globalData.PointsMultiplier = _pointMultiplier;
        }

        void SpawnEnemy(int index)
        {
            var bounds = _map.localBounds;
            var position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 0);
            while (Vector3.Distance(CenterOfPlayers(), position) <= 5)
            {
                position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 0);
            }

            var enemy = NetworkManager.Instance.InstantiateEnemyNetworking(index, position, Quaternion.Euler(Vector2.up));
            var setup = new EnemySetup(
                speed: Mathf.Log(_time + 100, 100F) * _multiplier,
                target: Players[Random.Range(0, Players.Count)],
                health: (Mathf.Log10(_time + 10) * _multiplier) * 10,
                tier: _tier % 35,
                globalData: _globalData
            );

            enemy.transform.position = position;
            enemy.GetComponent<EnemyBehaviour>().Setup(setup);
        }

        private Vector2 CenterOfPlayers()
        {
            Vector3 totalPositions = new Vector3();
            foreach (var position in Players)
                totalPositions += position.position;
            return totalPositions / Players.Count;
        }

        public void LookForPlayers()
        {
            Players = new List<Transform>();
            var playersFound = GameObject.FindGameObjectsWithTag("Player").ToList();
            foreach(var player in playersFound)
                Players.Add(player.transform);
        }
    }
}