using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private Transform _player;
    [SerializeField] private GameObject[] _enemyPrefab;
    [SerializeField] private Tilemap _map;
    private GlobalDataHandler _globalData;
    private float _spawnTimer;
    private float _spawnDelay;
    private float _multiplier;
    private float _multiplierTimer;
    private float _time;
    private int _pointMultiplier;

    // Use this for initialization
    void Start () {
        _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
        _time = 0;
        _map.CompressBounds();
        _spawnTimer = _time + 1;
        _multiplierTimer = _time + 30f;
        _multiplier = 1f;
        _pointMultiplier = 1;
	}
	
	// Update is called once per frame
	void Update () {
        _time = Mathf.Floor(Time.timeSinceLevelLoad / 10f) * 10f;
        if (_spawnTimer < Time.timeSinceLevelLoad) {
            var count = 0;
            foreach(GameObject enemy in _enemyPrefab)
            {
                if (Time.timeSinceLevelLoad > enemy.GetComponent<IEnemy>().EnemyInfo.StartingSpawnTime) count++;
            }
            SpawnEnemy(Random.Range(0, count));
            _spawnDelay = 1 / Mathf.Log(Mathf.Pow(_time + 10, 2), 100f);
            _spawnTimer = Time.timeSinceLevelLoad + _spawnDelay;
        }
        if(_multiplierTimer <= _time)
        {
            _multiplierTimer = _time + 30f;
            _multiplier += 0.3f;
            _pointMultiplier++;
            _globalData.PointsMultiplier = _pointMultiplier;
        }
	}

    void SpawnEnemy(int index)
    {
        var temp = Instantiate(_enemyPrefab[index], transform);
        var bounds = _map.localBounds;
        var position = new Vector3(Random.Range(bounds.min.x, bounds.max.x),  Random.Range(bounds.min.y, bounds.max.y),0);
        while(Vector3.Distance(_player.position, position) <= 5)
        {
            position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 0);
        }

        temp.transform.position = position;
        temp.GetComponent<IEnemy>().Setup(Mathf.Log(_time + 100, 100F) * _multiplier, //speed
            _player, (Mathf.Log10(_time + 10) * _multiplier) * 10, //player and health
            Mathf.FloorToInt(_time/10f) % 35, _globalData); //Tier and GlobalData

    }
}
